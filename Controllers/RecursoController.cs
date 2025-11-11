using api.agroapp.model;
using api_agroapp.lib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace api.agroapp.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RecursoController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly HashPasswordService _hashPasswordService;

        public RecursoController(DataContext context, HashPasswordService hashPasswordService, IConfiguration config)
        {
            _context = context;
            _hashPasswordService = hashPasswordService;
            _config = config;
        }

        [HttpGet("/api/recursos/{id_recurso}")]
        public IActionResult GetRecursoPorId(int id_recurso)
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                //verifico que el usuario exista 
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                var recurso = _context.Recurso.FirstOrDefault(r => r.id_recurso == id_recurso && r.id_usuario == user.id_usuario);
                if (recurso == null)
                {
                    return NotFound("Recurso no encontrado.");
                }

                return Ok(recurso);
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al obtener el recurso: " + ex.Message);
            }
        }

        [HttpGet("/api/recursos")]
        public IActionResult GetRecursosDelUsuarioLogeado()
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                //verifico que el usuario exista 
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                var recursos = _context.Recurso
                    .Where(r => r.id_usuario == user.id_usuario)
                    .ToList();

                return Ok(recursos);
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al obtener los recursos: " + ex.Message);
            }

        }

        [HttpPost("/api/recursos/agregar")]
        public IActionResult AgregarRecurso([FromBody] Recurso nuevoRecurso)
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                nuevoRecurso.id_usuario = user.id_usuario;

                _context.Recurso.Add(nuevoRecurso);
                _context.SaveChanges();

                return Ok(new { message = "Recurso agregado exitosamente." });
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al agregar el recurso: " + ex.Message);
            }
        }

        [HttpPut("/api/recursos/actualizar/{id_recurso}")]
        public IActionResult ActualizarRecurso(int id_recurso, [FromBody] Recurso recursoActualizado)
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                var recursoExistente = _context.Recurso.Find(id_recurso);
                if (recursoExistente == null)
                {
                    return NotFound("Recurso no encontrado.");
                }

                if (recursoExistente.id_usuario != user.id_usuario)
                {
                    return Forbid("No tienes permiso para actualizar este recurso.");
                }

                recursoExistente.nombre = recursoActualizado.nombre;
                recursoExistente.tipo = recursoActualizado.tipo;
                recursoExistente.marca = recursoActualizado.marca;
                recursoExistente.modelo = recursoActualizado.modelo;
                recursoExistente.estado = recursoActualizado.estado;

                _context.Recurso.Update(recursoExistente);
                _context.SaveChanges();

                return Ok(new { message = "Recurso actualizado exitosamente." });
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al actualizar el recurso: " + ex.Message);
            }
        }

        [HttpGet("/api/recursos/cantidad")]
        public IActionResult GetCantidadRecursosDelUsuarioLogeado()
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                var cantidadRecursos = _context.Recurso.Count(r => r.id_usuario == user.id_usuario);

                return Ok(cantidadRecursos);
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al obtener la cantidad de recursos: " + ex.Message);
            }
        }


    }
}