using System.Threading.Tasks;
using dotnet_RPG.Dtos.Character;
using dotnet_RPG.Dtos.Character.CharacterSkill;
using dotnet_RPG.Services.CharacterSkill;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_RPG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CharacterSkillController  : ControllerBase
    {
        private readonly ICharacterSkill icharacterskill;
        public CharacterSkillController(ICharacterSkill _characterskill)
        {
            icharacterskill =_characterskill;
        }

        [HttpPost]
        public async Task<IActionResult> AddCharacter(AddCharacterSkillDto  addCharacter){

            return Ok(await icharacterskill.AddCharacterSkill(addCharacter));
        }
    }
}