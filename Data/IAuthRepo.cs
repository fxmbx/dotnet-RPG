using System.Threading.Tasks;
using dotnet_RPG.Models;

namespace dotnet_RPG.Data
{
    public interface IAuthRepo
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string Password);
        Task<bool> UserExist(string username);
    }
}