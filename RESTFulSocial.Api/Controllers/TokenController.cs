using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RESTFulSocial.Core.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RESTFulSocial.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Authentication(UserLogin login)
        {
            // si es un usuario valido
            if (IsValidUser(login))
            {
                var token = GenerateToken();
                return Ok(new { token });
            }
            return NotFound();
        }

        private bool IsValidUser(UserLogin login)
        {
            return true;
        }

        private string GenerateToken()
        {
            // Header
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            // Claims: es informacion que nosotros quremos validar, o queremos agregarle
            // en el cuerpo del mensaje las caracteristicas del usuario que estamos generando.
            var claims = new[] 
            { 
                new Claim(ClaimTypes.Name, "Adrian Garay"),
                new Claim(ClaimTypes.Email, "adriangaray@gmail.com"),
                new Claim(ClaimTypes.Role, "Administrador"),
            };

            //Payload
            var playload = new JwtPayload
            (
                _configuration["Authentication:Issuer"], 
                _configuration["Authentication:Audience"], 
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(10)
            );

            var token = new JwtSecurityToken(header, playload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
