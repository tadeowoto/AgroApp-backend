
using api.agroapp.model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace api.agroapp.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CosechaController : ControllerBase
    {
        private readonly DataContext _context;

        public CosechaController(DataContext context, IConfiguration config)
        {
            _context = context;
        }


        [HttpGet("/api/cosechas/{id_lote}")]
        public IActionResult GetCosechasPorLote(int id_lote)
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                var cosechas = _context.Cosecha.Where(c => c.id_lote == id_lote).ToList();
                if (cosechas == null || cosechas.Count == 0)
                {
                    return NotFound("No se encontraron cosechas para el lote especificado.");
                }

                return Ok(cosechas);
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al obtener las cosechas: " + ex.Message);
            }

        }

        [HttpPost("/api/cosechas/agregar")]
        public IActionResult agregarCosecha([FromBody] Cosecha nuevaCosecha)
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                _context.Cosecha.Add(nuevaCosecha);
                _context.SaveChanges();

                return Ok(new { message = "Cosecha agregada exitosamente." });
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al agregar la cosecha: " + ex.Message);
            }
        }


        [HttpPut("/api/cosechas/actualizar/{id_cosecha}")]
        public IActionResult actualizarCosecha(int id_cosecha, [FromBody] Cosecha updatedCosecha)
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                var cosecha = _context.Cosecha.Find(id_cosecha);
                if (cosecha == null)
                {
                    return NotFound("Cosecha no encontrada.");
                }
                cosecha.fecha_inicio = updatedCosecha.fecha_inicio;
                cosecha.fecha_fin = updatedCosecha.fecha_fin;
                cosecha.rendimiento = updatedCosecha.rendimiento;
                cosecha.observaciones = updatedCosecha.observaciones;

                _context.SaveChanges();

                return Ok(new { message = "Cosecha actualizada exitosamente." });
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al actualizar la cosecha: " + ex.Message);
            }
        }

    }

}