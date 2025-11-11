
using api.agroapp.model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
                var lote = _context.Lote.Find(id_lote);
                if (lote == null) return NotFound("Lote no encontrado.");
                var campo = _context.Campo.Find(lote.id_campo);
                if (campo == null || campo.id_usuario != user.id_usuario)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                var cosechas = _context.Cosecha.Where(c => c.id_lote == id_lote).ToList();
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

                var lote = _context.Lote.Find(nuevaCosecha.id_lote);
                if (lote == null) return NotFound("El lote especificado no existe.");

                var campo = _context.Campo.Find(lote.id_campo);

                if (campo == null || campo.id_usuario != user.id_usuario)
                {
                    return Forbid("No tienes permiso para agregar una cosecha a este lote.");
                }

                Cosecha cosecha = new Cosecha
                {
                    id_lote = nuevaCosecha.id_lote,
                    fecha_inicio = nuevaCosecha.fecha_inicio,
                    fecha_fin = nuevaCosecha.fecha_fin,
                    rendimiento = nuevaCosecha.rendimiento,
                    observaciones = nuevaCosecha.observaciones
                };

                _context.Cosecha.Add(cosecha);
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
                if (cosecha == null) return NotFound("Cosecha no encontrada.");

                var lote = _context.Lote.Find(cosecha.id_lote);
                if (lote == null) return NotFound("Lote asociado no encontrado.");


                var campo = _context.Campo.Find(lote.id_campo);
                if (campo == null || campo.id_usuario != user.id_usuario)
                {
                    return Forbid("No tienes permiso para actualizar esta cosecha.");
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

        [HttpGet("/api/cosechas/proximas")]
        public IActionResult GetCosechasProximas()
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                var today = System.DateTime.Today;
                var cosechasProximas = _context.Cosecha
                    .Join(_context.Lote,
                        cosecha => cosecha.id_lote,
                        lote => lote.id_lote,
                        (cosecha, lote) => new { Cosecha = cosecha, Lote = lote })
                    .Join(_context.Campo,
                        joined => joined.Lote.id_campo,
                        campo => campo.id_campo,
                        (joined, campo) => new { joined.Cosecha, joined.Lote, Campo = campo })
                    .Where(x => x.Campo.id_usuario == user.id_usuario && x.Cosecha.fecha_fin >= today)
                    .Select(x => x.Cosecha)
                    .ToList();

                return Ok(cosechasProximas);
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al obtener las cosechas próximas: " + ex.Message);
            }
        }

    }

}