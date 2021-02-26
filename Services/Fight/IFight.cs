using System.Threading.Tasks;
using dotnet_RPG.Dtos.Character.FIght;
using dotnet_RPG.Models;

namespace dotnet_RPG.Services.Fight
{
    public interface IFight
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
    }
}