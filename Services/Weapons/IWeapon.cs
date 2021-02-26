using System.Threading.Tasks;
using dotnet_RPG.Dtos.Character;
using dotnet_RPG.Dtos.Character.Weapon;
using dotnet_RPG.Models;

namespace dotnet_RPG.Services.Weapons
{
    public interface IWeapon
    
    {
         Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto addWeapon);
    }
}