using Microsoft.AspNetCore.Mvc;
using api.agroapp.model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using api_agroapp.lib;
using api_agroapp.model;


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

        [HttpGet("/api/campos/usuario")]
        public IActionResult GetCamposByUsuarioId()
        {
            var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));

            if (user == null || user.id_usuario == null)
            {
                return Forbid("No tienes permiso para acceder a este recurso.");
            }
            var campos = _context.Campo.Where(c => c.id_usuario == user.id_usuario).ToList();
            if (campos == null || campos.Count == 0)
            {
                return NotFound("No se encontraron campos para el usuario especificado.");
            }
            return Ok(campos);
        }

        [HttpPost("/api/campos/agregar")]
        public IActionResult AgregarCampo([FromBody] Campo campo)
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }
                campo.id_usuario = user.id_usuario;
                Console.WriteLine(campo.nombre + " " + campo.ubicacion + " " + campo.extension_ha);
                _context.Campo.Add(campo);
                _context.SaveChanges();
                return Ok(campo);

            }
            catch (Exception ex)
            {
                return BadRequest("Error al agregar el campo: " + ex.Message);
            }

        }
    }
}