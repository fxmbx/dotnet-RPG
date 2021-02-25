using System.Threading.Tasks;
using dotnet_RPG.Data;
using Microsoft.AspNetCore.Mvc;
using dotnet_RPG.Dtos.Character.User;
using dotnet_RPG.Models;
using System.Collections.Generic;

namespace dotnet_RPG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo authRepo;
        public AuthController(IAuthRepo _authrepo)
        {
            authRepo = _authrepo;
        }
        
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegistrationDto request){
            var repsonse = await authRepo.Register(
                new Models.User {Username = request.username}, request.password
            );
            if(!repsonse.Success){
                return BadRequest(repsonse);
            }
            return Ok(repsonse);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto request){
            var response = await authRepo.Login(
                request.username, request.password
            );
            if(!response.Success){
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}