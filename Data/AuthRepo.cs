using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_RPG.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace dotnet_RPG.Data
{
    public class AuthRepo : IAuthRepo 
    {
        private readonly DataContext dbcontext;
        private readonly IConfiguration configuration;
        public AuthRepo(DataContext _dbcontext, IConfiguration _configuration)
        {
            dbcontext = _dbcontext;
            configuration = _configuration;
        }
        public async Task<ServiceResponse<string>> Login(string username, string Password)
        {
            var response = new ServiceResponse<string>();
            var user = await dbcontext.users.FirstOrDefaultAsync(x =>x.Username.ToLower() == username.ToLower());
            if(user == null){
                response.Success = false;
                response.Meassgae = "The Username not found";
            }else if(!VerifyPasswordHash(Password, user.PasswordHash, user.PasswordSalt,out byte[] computedhash)){
                response.Success = false;
                response.Meassgae = String.Format(" password you entered is in correct {0} and \ncomputerd hash: {1}", user.PasswordHash.ToString(), computedhash.ToString());
            }else{
                response.Data = CreateToken(user);
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
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt, out byte[] computedhash){
            using(var hmac = new System.Security.Cryptography.HMACSHA512()){
                  computedhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int a=0; a<computedhash.Length;a++){
                    if(computedhash[a] != passwordHash[a]){
                        return false;
                    }
                }
                return true;
            }
        }
        private string CreateToken(User user){
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };
            var  key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value)
            );

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDecription = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };
            var tokenHandler = new  JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDecription);
            return tokenHandler.WriteToken(token);
        }

    }
}