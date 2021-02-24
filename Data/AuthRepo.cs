using System.Threading.Tasks;
using dotnet_RPG.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_RPG.Data
{
    public class AuthRepo : IAuthRepo 
    {
        private readonly DataContext dbcontext;
        public AuthRepo(DataContext _dbcontext)
        {
            dbcontext = _dbcontext;
        }
        public async Task<ServiceResponse<string>> Login(string username, string Password)
        {
            var response = new ServiceResponse<string>();
            var user = await dbcontext.users.FirstOrDefaultAsync(x =>x.Username.ToLower() == username.ToLower());
            if(user == null){
                response.Success = false;
                response.Meassgae = "The Username not found";
            }else if(!VerifyPasswordHash(Password, user.PasswordHash, user.PasswordSalt)){
                response.Success = false;
                response.Meassgae = " password you entered is in correct";
            }else{
                response.Data = user.Id.ToString();
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var repsonse = new ServiceResponse<int> ();
            if(await UserExist(user.Username)){
                repsonse.Success = false;
                repsonse.Meassgae = "User exist";
                return repsonse;
            }
            CreatePasswordHash(password,out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt =passwordSalt;

            await dbcontext.users.AddAsync(user);
            await dbcontext.SaveChangesAsync();
            repsonse.Data = user.Id;
            return repsonse;
           
        }

        public async Task<bool> UserExist(string username)
        {
            if(await dbcontext.users.AnyAsync(x =>x.Username.ToLower() == username.ToLower())){
                return true;
            }
            return false;
        } 

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
                using (var hmac = new System.Security.Cryptography.HMACSHA512()){
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt){
            using(var hmac = new System.Security.Cryptography.HMACSHA512()){
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int a=0; a<computedHash.Length;a++){
                    if(computedHash[a] != passwordHash[a]){
                        return false;
                    }
                }
                return true;
            }
        }

    }
}