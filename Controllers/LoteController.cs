using api_agroapp.lib;
using api.agroapp.model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace api.agroapp.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LoteController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly HashPasswordService _hashPasswordService;

        public LoteController(DataContext context, HashPasswordService hashPasswordService, IConfiguration config)
        {
            _context = context;
            _hashPasswordService = hashPasswordService;
            _config = config;
        }

        [HttpGet("/api/lotes/{id_campo}")]
        public IActionResult getLotesPorIdCampo(int id_campo)
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
                //verifico que el campo pertenezca al usuario
                var campo = _context.Campo.Find(id_campo);
                if (campo == null || campo.id_usuario != user.id_usuario)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                var lotes = _context.Lote.Where(l => l.id_campo == id_campo).ToList();
                return Ok(lotes);
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al obtener los lotes: " + ex.Message);
            }
        }

        [HttpPost("/api/lotes/editar/{id_lote}")]
        public IActionResult EditarLote(int id_lote, [FromBody] Lote loteData)
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
                if (lote == null)
                {
                    return NotFound("Lote no encontrado.");
                }

                var campo = _context.Campo.Find(lote.id_campo);
                if (campo == null || campo.id_usuario != user.id_usuario)
                {
                    return Forbid("No tienes permiso para editar este lote.");
                }

                lote.nombre = loteData.nombre;
                lote.superficie_ha = loteData.superficie_ha;
                lote.cultivo = loteData.cultivo;
                lote.fecha_creacion = loteData.fecha_creacion;

                _context.Lote.Update(lote);
                _context.SaveChanges();

                return Ok(lote);
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al editar el lote: " + ex.Message);
            }

        }

        [HttpPost]
        [Route("/api/lotes/agregar")]
        public IActionResult AgregarLote([FromBody] Lote loteData)
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                var campo = _context.Campo.Find(loteData.id_campo);
                if (campo == null || campo.id_usuario != user.id_usuario)
                {
                    return Forbid("No tienes permiso para agregar un lote a este campo.");
                }

                Lote newLote = new Lote
                (
                    loteData.id_campo,
                    loteData.nombre,
                    loteData.superficie_ha,
                    loteData.cultivo
                );

                _context.Lote.Add(newLote);
                _context.SaveChanges();

                return Ok(new { message = "Lote agregado exitosamente." });
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al agregar el lote: " + ex.Message);
            }
        }





    }


}