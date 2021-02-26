using System.Threading.Tasks;
using dotnet_RPG.Dtos.Character;
using dotnet_RPG.Dtos.Character.CharacterSkill;
using dotnet_RPG.Models;

namespace dotnet_RPG.Services.CharacterSkill
{
    public interface ICharacterSkill
    {
         Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill);
    }
}