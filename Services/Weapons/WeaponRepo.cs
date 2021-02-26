using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_RPG.Data;
using dotnet_RPG.Dtos.Character;
using dotnet_RPG.Dtos.Character.Weapon;
using dotnet_RPG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace dotnet_RPG.Services.Weapons
{
    public class WeaponRepo : IWeapon
    {
        private readonly DataContext dbcontext;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        
        public WeaponRepo(DataContext _dbcontext, IMapper _mapper, IHttpContextAccessor _httpContextAccessor)
        {
            dbcontext = _dbcontext;
            mapper = _mapper;
            httpContextAccessor = _httpContextAccessor;
        }
        private int GetUserId() => int.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto addWeapon)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                if(addWeapon != null)
                {
                    var _character =  await dbcontext.characters.FirstOrDefaultAsync(c =>c.Id == addWeapon.CharacterId && c.users.Id == GetUserId());
                    if(_character == null)
                    {
                        response.Success = false;
                        response.Meassgae = "Character not found";
                    }
                    Weapon weapon = new Weapon 
                    {
                        Name = addWeapon.Name,
                        Damage = addWeapon.Damage,
                        character = _character,
                        CharacterId = addWeapon.CharacterId
                    
                    };
                    //Or use Auto Mapper
                    // var weapon = mapper.Map<Weapon>(addWeapon);

                    await dbcontext.weapons.AddAsync(weapon);
                    await dbcontext.SaveChangesAsync();
                    response.Data = mapper.Map<GetCharacterDto>(_character);
                }

            }
            catch (System.Exception ex)
            {   
                response.Success = false;
                response.Meassgae = ex.Message;
            }
            return response;
            
        }

        
    }
}