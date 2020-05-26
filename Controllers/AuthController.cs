
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using DatingApp.API.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Text;
using DatingApp.API.Dtos;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]  
    public class AuthController : ControllerBase
    {
     private readonly IAuthRepository _repo;
     private readonly IConfiguration _config;
    public AuthController(IAuthRepository repo ,IConfiguration config )
    {
      _repo = repo; 
      _config = config;
    }
    

    [HttpPost("register")]
    public async Task<IActionResult> Register (UserForRegisterDTO UserforRegisterDto)
    {
        UserforRegisterDto.UserName = UserforRegisterDto.UserName.ToLower();

         if (await _repo.UserExists(UserforRegisterDto.UserName))
         {
             return BadRequest("Username already Exists") ;
         }

         var UserToCreate = new DatingApp.API.Models.User
         {
             Name = UserforRegisterDto.UserName
         };
          
          var createdUser = await _repo.Register(UserToCreate,UserforRegisterDto.Password);

          return StatusCode(201);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login (UserForLoginDTO userforLoginDto)
    {
        var userFromRepo = await _repo.Login(userforLoginDto.UserName.ToLower(),userforLoginDto.Password);
        if(userFromRepo == null)
        {
            return Unauthorized();
        }

        var claims = new []
        {
             new System.Security.Claims.Claim(ClaimTypes.NameIdentifier as string,userFromRepo.ID.ToString() as string),
             new System.Security.Claims.Claim(ClaimTypes.Name as string,userFromRepo.Name as string)
        };

        var key =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
         var tokenDescriptor = new SecurityTokenDescriptor
         {
             Subject = new ClaimsIdentity(claims),
             Expires = DateTime.Now.AddDays(1),
             SigningCredentials = creds

         };

         var tokenHandler = new JwtSecurityTokenHandler();

         var token  = tokenHandler.CreateToken(tokenDescriptor);
        
        return Ok (new {
            token = tokenHandler.WriteToken(token)
        });


    }

    }
        
}