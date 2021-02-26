using System;
using System.Threading.Tasks;
using dotnet_RPG.Data;
using dotnet_RPG.Dtos.Character.FIght;
using dotnet_RPG.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_RPG.Services.Fight
{
    public class FightRepo : IFight
    {
        private readonly DataContext dbcontext;
        public FightRepo(DataContext _dbcontext)
        {
            dbcontext = _dbcontext;
        }
        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker =await dbcontext.characters
                .Include(c =>c.weapon)
                .FirstOrDefaultAsync(c =>c.Id == request.AttackerId);

                var opponent = await dbcontext.characters
                .FirstOrDefaultAsync(c => c.Id == request.OpponentId);
                int damage = attacker.weapon.Damage + (new Random().Next(attacker.Strength));
                damage -= new Random().Next(opponent.Defense);
                if(damage > 0)
                     opponent.HitPoints -= damage;
                if(opponent.HitPoints <=0)
                      response.Meassgae = String.Format("{0} has been defeated", opponent.Name);
                
                dbcontext.characters.Update(opponent);
                await dbcontext.SaveChangesAsync();
                response.Data = new AttackResultDto{
                    Attacker = attacker.Name,
                    Opponent=  opponent.Name,
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
    }
}