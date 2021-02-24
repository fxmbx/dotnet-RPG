using Microsoft.AspNetCore.Mvc;
using dotnet_RPG.Models;
using System.Collections.Generic;
using System.Linq;
using dotnet_RPG.Services;
using System.Threading.Tasks;
using dotnet_RPG.Dtos.Character;

namespace dotnet_RPG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacter _icharac;
        public CharacterController(ICharacter icharac)
        {
            _icharac = icharac;
        }

        // [Route("GetAll")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
          return Ok(await _icharac.GetAllCharacters());
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetSingle(int id){
           return Ok(await _icharac.GetCharacterById(id));
             
        }

        [HttpPost]
        public async Task<IActionResult> AddCharacter(AddCharacterDto charac){
          
            return Ok( await _icharac.AddCharacter(charac));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCharatcer(UpdateChararcterDto update)
        {
            ServiceResponse<GetCharacterDto> response =await _icharac.UpdateCharacter(update);
            if(response.Data == null){
                return NotFound();
            }
            return Ok(response);
        }

        
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int id){
           ServiceResponse<IEnumerable<GetCharacterDto>> response = await _icharac.DeleteCharacter(id);
           if(response == null){
               return NotFound();
           }
           return Ok(response);
             
        }
    }
}