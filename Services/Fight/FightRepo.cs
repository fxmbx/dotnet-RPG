using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_RPG.Data;
using dotnet_RPG.Dtos.Character.FIght;
using dotnet_RPG.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_RPG.Services.Fight
{
    public class FightRepo : IFight
    {
        private readonly DataContext dbcontext;
        private readonly IMapper mapper;
        public FightRepo(DataContext _dbcontext, IMapper _mapper)
        {
            mapper =_mapper;
            dbcontext = _dbcontext;
        }
        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
        {
           var respone =new ServiceResponse<FightResultDto> ();
           try
           {
               List<Character> Characters = 
                    await dbcontext.characters
                    .Include(x =>x.weapon)
                    .Include(x =>x.CharacterSkills)
                    .ThenInclude(cs =>cs.skill)
                    .Where(c => request.CharacterIds.Contains(c.Id)).ToListAsync();

                    bool defaeted = false;
                    while(!defaeted){
                        foreach(var attacker in Characters){
                            List<Character> opponents = Characters.Where(c =>c.Id != attacker.Id).ToList();
                            var opponent = opponents[new Random().Next(opponents.Count)];

                            int damage = 0;
                            string attackUsed = string.Empty;

                            bool useWeapon =new Random().Next(2) == 0;
                            if(useWeapon)
                            { 
                                attackUsed = attacker.weapon.Name;
                                damage = DoWeaponAttack(attacker,opponent);
                            }else{
                                    int randdSkill = new Random().Next(attacker.CharacterSkills.Count);
                                    attackUsed = attacker.CharacterSkills[randdSkill].skill.Name;
                                    damage = DoSkillAttack(attacker, opponent, attacker.CharacterSkills[randdSkill]);
                            }
                            respone.Data.Log.Add(String.Format("{0} attacks {1} using {2} with {3} damage", attacker.Name, opponent.Name,attackUsed, (damage>0 ? damage : 0) ));
                            if(opponent.HitPoints <=0){
                                defaeted = true;
                                attacker.Victories++;
                                opponent.Defeats++;
                            respone.Data.Log.Add(String.Format("{0} has been deafeated ", opponent.Name));
                            respone.Data.Log.Add(String.Format("{0} wins with {1} HP left", attacker.Name, attacker.HitPoints ));
                            break;
                            }
                        }

                    }
                    Characters.ForEach(c => {
                        c.Fight++;
                        c.HitPoints = 100;
                    });
                    dbcontext.characters.UpdateRange(Characters);
                    await dbcontext.SaveChangesAsync();
           }

           catch (System.Exception ex) 
           {
               
               respone.Success= false;
               respone.Meassgae= ex.Message;
           }
           return respone;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
             var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await dbcontext.characters
                .Include(c => c.CharacterSkills)
                .ThenInclude(cs => cs.skill)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponent = await dbcontext.characters
                .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                var characterskill = attacker.CharacterSkills.FirstOrDefault(c => c.skill.Id == request.SkillId);
                if (characterskill == null)
                {
                    response.Success = false;
                    response.Meassgae = String.Format("{0} doesnt know skill", attacker.Name);
                }

                DoSkillAttack(attacker, opponent, characterskill);
                if (opponent.HitPoints <= 0)
                    response.Meassgae = String.Format("{0} has been defeated", opponent.Name);

                dbcontext.characters.Update(opponent);
                await dbcontext.SaveChangesAsync();
                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OppnentHP = opponent.HitPoints
                };
            }
            catch (System.Exception ex)
            {     
               response.Success= false;
               response.Meassgae =ex.Message;
            }
            return response;
        }

        private static int DoSkillAttack(Character attacker, Character opponent, ChracterSkill characterskill)
        {
            int damage = characterskill.skill.Damage + (new Random().Next(attacker.Intelligence));
            damage -= new Random().Next(opponent.Defense);
            if (damage > 0)
                opponent.HitPoints -= damage;
         return damage;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await dbcontext.characters
                .Include(c => c.weapon)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponent = await dbcontext.characters
                .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                DoWeaponAttack(attacker, opponent);
                if (opponent.HitPoints <= 0)
                    response.Meassgae = String.Format("{0} has been defeated", opponent.Name);

                dbcontext.characters.Update(opponent);
                await dbcontext.SaveChangesAsync();
                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OppnentHP = opponent.HitPoints
                };
            }
            catch (System.Exception ex)
            {     
               response.Success= false;
               response.Meassgae =ex.Message;
            }
            return response;
        }

        private static int DoWeaponAttack(Character attacker, Character opponent)
        {
            int damage = attacker.weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defense);

            if (damage > 0)
                opponent.HitPoints -= damage;
            return damage;
        }

        public async Task<ServiceResponse<List<HighScoreDto>>> GetHighScore()
        {
            List<Character> characters = await dbcontext.characters
            .Where(x=>x.Fight>0)
            .OrderByDescending(x=>x.Victories)
            .ThenBy(x=>x.Defeats)
            .ToListAsync();

            var respone = new ServiceResponse<List<HighScoreDto>>{
                Data = characters.Select(c => mapper.Map<HighScoreDto>(c)).ToList()
            };
            return respone;

        }
        
    }
}