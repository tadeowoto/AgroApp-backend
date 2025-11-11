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

        [HttpGet("/api/campos/{id_campo}")]
        public IActionResult GetCampoById(int id_campo)
        {
            var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
            if (user == null || user.id_usuario == null)
            {
                return Forbid("No tienes permiso para acceder a este recurso.");
            }
            var campo = _context.Campo.Find(id_campo);
            if (campo == null)
            {
                return NotFound("Campo no encontrado.");
            }
            if (campo.id_usuario != user.id_usuario)
            {
                return Forbid("No tienes permiso para acceder a este campo.");
            }
            return Ok(campo);
        }


        [HttpPost("/api/campos/agregar")]
        public IActionResult AgregarCampo([FromBody] CampoData campoData)
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }
                campoData.id_usuario = user.id_usuario;
                Campo newCampo = new Campo
                (
                    campoData.id_usuario,
                    campoData.nombre,
                    campoData.ubicacion,
                    campoData.extension_ha,
                    campoData.longitud,
                    campoData.latitud
                );

                _context.Campo.Add(newCampo);
                _context.SaveChanges();
                return Ok(new { message = "Campo agregado exitosamente." });

            }
            catch (Exception ex)
            {
                return BadRequest("Error al agregar el campo: " + ex.Message);
            }

        }


        [HttpPost("/api/campos/editar/{id_campo}")]
        public IActionResult EditarCampo(int id_campo, [FromBody] CampoData campoData)
        {

            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }
                var campoAEditar = _context.Campo.Find(id_campo);
                if (campoAEditar.id_usuario != user.id_usuario)
                {
                    return Forbid("No tienes permiso para editar este campo.");
                }

                campoAEditar.nombre = campoData.nombre;
                campoAEditar.ubicacion = campoData.ubicacion;
                campoAEditar.extension_ha = campoData.extension_ha;
                campoAEditar.longitud = campoData.longitud;
                campoAEditar.latitud = campoData.latitud;

                _context.SaveChanges();
                return Ok(campoAEditar);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al editar el campo: " + ex.Message);
            }

        }

        [HttpGet("/api/campos/cantidad")]
        public IActionResult GetCantidadCampos()
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                var cantidadCampos = _context.Campo
                    .Count(c => c.id_usuario == user.id_usuario);

                return Ok(cantidadCampos);
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al obtener la cantidad de campos: " + ex.Message);
            }
        }
    }
}