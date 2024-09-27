using Microsoft.AspNetCore.Mvc;

namespace SistemaTareas.Controllers
{
    using DB.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    namespace SistemaTareas.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class AuthController : ControllerBase
        {
            private readonly SistemaTareasContext _context;

            public AuthController(SistemaTareasContext context)
            {
                _context = context;
            }


            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] LoginModel login)
            {

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == login.Username);

                if (login.Username == usuario.NombreUsuario  && login.Password == usuario.PassUsuario) 
                {

                    var claims = new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Sub, login.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };


                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("k6VwneyHMqDfFAvW9nPOz616zAAvifH5")); 
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                    var token = new JwtSecurityToken(
                        issuer: "SistemaTareas",  
                        audience: "Usuario",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }

                return Unauthorized();
            }
        }

        // Modelo para el login
        public class LoginModel
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }
    }

}
