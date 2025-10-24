using Microsoft.AspNetCore.Mvc;
using api.agroapp.model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using api_agroapp.model;
using api_agroapp.lib;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace api.agroapp.controllers
{


    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UsuarioController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly HashPasswordService _hashPasswordService;
        public UsuarioController(DataContext context, HashPasswordService hashPasswordService, IConfiguration config)
        {
            _context = context;
            _hashPasswordService = hashPasswordService;
            _config = config;
        }


        [HttpPost("/api/usuarios/login")]
        [AllowAnonymous]
        public IActionResult Login([FromForm] LoginData data)
        {
            try
            {
                string hashedPassword = _hashPasswordService.HashPassword(data.password);
                var user = _context.Usuarios
                    .FirstOrDefault(u => u.email == data.email);

                if (user != null && user.password == hashedPassword)
                {
                    var key = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(_config["Jwt:Key"])
                    );
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var claims = new[]
                    //info que se va a guardar en el token
                    {
                        new Claim("Id", user.id_usuario.ToString()),
                        new Claim("FullName", user.nombre),
                        new Claim("Email", user.email)
                    };

                    var token = new JwtSecurityToken(
                        issuer: _config["Jwt:Issuer"],
                        audience: _config["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(90),
                        signingCredentials: creds
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Usuario o contraseña incorrectos");

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("/api/usuarios/logged")]
        public IActionResult getUsuarioLoggeado()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                if (userId == null)
                {
                    return Unauthorized();
                }
                var user = _context.Usuarios.Find(int.Parse(userId));
                var userWithoutPassword = new
                {
                    user.id_usuario,
                    user.nombre,
                    user.email,
                    user.telefono,
                    user.fecha_registro
                };
                return Ok(userWithoutPassword);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("/api/usuarios/CambiarPassword")]
        public IActionResult CambiarPassword([FromForm] ChangePasswordData data)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                if (userId == null)
                {
                    return Unauthorized("No tienes permiso para acceder a este recurso.");
                }
                var user = _context.Usuarios.Find(int.Parse(userId));
                string hashedOldPassword = _hashPasswordService.HashPassword(data.currentPassword);
                if (user.password != hashedOldPassword)
                {
                    return BadRequest("La contraseña actual es incorrecta.");
                }
                user.password = _hashPasswordService.HashPassword(data.newPassword);
                _context.SaveChanges();
                return Ok(new { message = "Contraseña cambiada exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}