using Microsoft.AspNetCore.Mvc;
using api.agroapp.model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using api_agroapp.lib;


namespace api.agroapp.controllers
{


    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CampoController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly HashPasswordService _hashPasswordService;
        public CampoController(DataContext context, HashPasswordService hashPasswordService, IConfiguration config)
        {
            _context = context;
            _hashPasswordService = hashPasswordService;
            _config = config;
        }

        [HttpGet("/api/campos/{idUsuario}")]
        public IActionResult GetCamposByUsuarioId(int idUsuario)
        {
            var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));

            if (user == null || user.id_usuario != idUsuario)
            {
                return Forbid("No tienes permiso para acceder a este recurso.");
            }
            var campos = _context.Campo.Where(c => c.id_usuario == idUsuario).ToList();
            return Ok(campos);
        }





    }
}