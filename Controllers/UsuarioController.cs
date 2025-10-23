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


        [HttpPost("/usuarios/api/login")]
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

        [HttpGet("/usuarios/api/logged")]
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
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}