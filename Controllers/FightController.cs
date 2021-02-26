using System.Threading.Tasks;
using dotnet_RPG.Dtos.Character.FIght;
using dotnet_RPG.Services.Fight;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_RPG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class FightController : ControllerBase
    {
        private readonly IFight ifight;
        public FightController(IFight _ifight)
        {
            ifight = _ifight;
        }
        [HttpPost("Weapon")]
        public async Task<IActionResult> WeaponAttack(WeaponAttackDto request){

            return Ok(await ifight.WeaponAttack(request));
            
        }
        [HttpPost("Skill")]
         public async Task<IActionResult> SkillAttac(SkillAttackDto request){

            return Ok(await ifight.SkillAttack(request));
            
        }
        [HttpPost]
        public async Task<IActionResult> Fight(FightRequestDto request){
            return Ok(await ifight.Fight(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetHighScore(){
            return Ok(await ifight.GetHighScore());
        }
    }
}