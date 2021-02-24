using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_RPG.Dtos.Character;
using dotnet_RPG.Models;
namespace dotnet_RPG.Services
{
    public interface ICharacter
    {
         Task<ServiceResponse<IEnumerable<GetCharacterDto>>> GetAllCharacters();

        Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id);

          Task<ServiceResponse<IEnumerable<GetCharacterDto>>> AddCharacter(AddCharacterDto charac);

          Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateChararcterDto update);

          Task<ServiceResponse<IEnumerable<GetCharacterDto>>> DeleteCharacter(int id);

    }
}