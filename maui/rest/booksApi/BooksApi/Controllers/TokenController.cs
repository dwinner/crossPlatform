using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BooksApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BooksApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController(IConfiguration config, BooksContext context) : ControllerBase
{
   public IConfiguration Configuration = config;

   [HttpPost]
   public async Task<IActionResult> Post(UserInfo aUserData)
   {
      var user = await GetUser(aUserData.Email, aUserData.Password);

      //create claims details based on the user information
      var claims = new[]
      {
         new Claim(JwtRegisteredClaimNames.Sub, Configuration["Jwt:Subject"]),
         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
         new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
         new Claim("Id", user.UserId.ToString()),
         new Claim("FirstName", user.FirstName),
         new Claim("LastName", user.LastName),
         new Claim("UserName", user.UserName),
         new Claim("Email", user.Email)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
      var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var token = new JwtSecurityToken(
         Configuration["Jwt:Issuer"],
         Configuration["Jwt:Audience"],
         claims,
         expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn
      );

      return Ok(new JwtSecurityTokenHandler().WriteToken(token));
   }

   private async Task<UserInfo> GetUser(string email, string password)
   {
      return await context.UserInfos.FirstOrDefaultAsync(u => u.Email == email && u.Password == password)
             ?? new UserInfo();
   }
}