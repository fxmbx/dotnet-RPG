using System.Threading.Tasks;
using dotnet_RPG.Dtos.Character;
using dotnet_RPG.Dtos.Character.Weapon;
using dotnet_RPG.Models;
using dotnet_RPG.Services.Weapons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_RPG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WeaponController : ControllerBase
    {
        private readonly IWeapon weapon;
        public WeaponController(IWeapon _weapon)
        {
            weapon =_weapon;
        }
        [HttpPost]
        public async Task<IActionResult> AddWeapon(AddWeaponDto addWeapon){
             
            return Ok( await weapon.AddWeapon(addWeapon));
        }
    }
}