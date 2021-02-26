using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_RPG.Data;
using dotnet_RPG.Dtos.Character;
using dotnet_RPG.Dtos.Character.CharacterSkill;
using dotnet_RPG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace dotnet_RPG.Services.CharacterSkill
{
    public class CharacterSkillRepo : ICharacterSkill
    {
        private readonly DataContext dbcontext;
        private IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        public CharacterSkillRepo(DataContext _dbcontext, IMapper _imapper,IHttpContextAccessor _httpcontextAccessor)
        {
            dbcontext= _dbcontext;
            mapper = _imapper;
            httpContextAccessor =_httpcontextAccessor;
            
        }

        private int GetUserId()=> int.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
           var response = new ServiceResponse<GetCharacterDto>(); 
            //  var serviceResponse = new ServiceResponse<IEnumerable<GetCharacterDto>>();
            // var charact = (mapper.Map<Character>(charac));
            // charact.users = await dbContext.users.FirstOrDefaultAsync(x=>x.Id == GetUserId());

            // await dbContext._characters.AddAsync(charact);
            // await dbContext.SaveChangesAsync();
            // serviceResponse.Data = (dbContext._characters.Where(x=>x.users.Id == GetUserId()).Select(c =>mapper.Map<GetCharacterDto>(c)));
            // return serviceResponse ;
            try
            {
                var _character = await dbcontext.characters.Include(c =>c.weapon).Include(c=>c.CharacterSkills).ThenInclude(cs =>cs.skill).FirstOrDefaultAsync(c =>c.Id == newCharacterSkill.CharacterId && c.users.Id == GetUserId());
                if(_character == null){
                    response.Success= false;
                    response.Meassgae="Character not found";
                }
                Skill _skill = await dbcontext.skills.FirstOrDefaultAsync(s =>s.Id == newCharacterSkill.SkillId);
                if(_skill == null ){
                    response.Success= false;
                    response.Meassgae="Skill not found";
                }

                var characterskill = new ChracterSkill{
                    character = _character,
                    skill = _skill
                };

                await dbcontext.chracterSkills.AddAsync(characterskill);
                await dbcontext.SaveChangesAsync();
                response.Data = mapper.Map<GetCharacterDto>(_character);
            }
            catch (System.Exception ex)
            {
                response.Success =false;
                response.Meassgae = ex.Message;
                 
            }
         
           return response;
        }
    }
}