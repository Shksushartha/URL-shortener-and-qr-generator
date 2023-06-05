using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using urlShortener.Data;
using urlShortener.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace urlShortener.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UrlDbContext _urlDbContext;
        private readonly IConfiguration _configuration;

        public LoginController(UrlDbContext urlDbContext, IConfiguration configuration)
        {
            _urlDbContext = urlDbContext;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Register")]

        public async Task<IActionResult> Register(User u)
        {
            _urlDbContext.users.Add(u);
            await _urlDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("Login")]

        public ActionResult<Result<Token>> Login([FromBody] UserLogin user)
        {
            var u = Authenticate(user);
            var response = new Result<Token>();
            if (u != null)
            {
                response.Success = true;
                response.data = new Token()
                {
                    userid = u.Id,
                    token = Generate(u)
                };
                return Ok(response);
            }
            else
            {
                response.message = "failed";
                return NotFound(response);
            }
        }


        private User? Authenticate(UserLogin u)
        {
            var x = _urlDbContext.users.FirstOrDefault(o => o.Username.ToLower() == u.username.ToLower() && o.Password == u.password);

            if (x != null)
            {
                return x;
            }
            else
                return null;
        }

        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signature = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Givenname),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.Role)

            };

            var token = new JwtSecurityToken(_configuration["jwt:Issuer"],
                _configuration["jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signature);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

