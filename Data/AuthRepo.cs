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
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(Password, user.Password );

            if(user == null){
                response.Success = false;
                response.Meassgae = "The Username or password you entered is incorrect";
            }else if (!isValidPassword){
                response.Success = false;
                response.Meassgae = "The Username or password you entered is incorrect";
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
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
           
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
        private string CreateToken(User user){
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
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