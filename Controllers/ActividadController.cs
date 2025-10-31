using api_agroapp.model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api_agroapp.lib;
using api.agroapp.model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace api_agroapp.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ActividadController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly HashPasswordService _hashPasswordService;

        public ActividadController(DataContext context, HashPasswordService hashPasswordService, IConfiguration config)
        {
            _context = context;
            _hashPasswordService = hashPasswordService;
            _config = config;
        }


        [HttpPost("api/actividades/agregar")]
        public IActionResult AgregarActividad([FromBody] Actividad actividad)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(userId));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }
                _context.Actividad.Add(actividad);
                _context.SaveChanges();
                return Ok(new { message = "Actividad agregada exitosamente." });
            }
            catch (System.Exception)
            {
                return StatusCode(500, new { message = "Error al agregar la actividad." });
            }
        }

        [HttpGet("api/actividades/lotes")]
        public IActionResult getActividadesDeUsuario()
        {
            try
            {
                var userIdString = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (string.IsNullOrEmpty(userIdString))
                {
                    return BadRequest("El Id de usuario en el token no es valido.");
                }

                var userId = int.Parse(userIdString);

                //este plano define la consulta para obtener los campos del usuario
                var camposQuery = _context.Campo
                    .Where(c => c.id_usuario == userId)
                    .Select(c => c.id_campo);

                // 2. Define la CONSULTA de Lotes basada en los campos del usuario
                var lotesQuery = _context.Lote
                    .Where(l => camposQuery.Contains(l.id_campo))
                    .Select(l => l.id_lote);

                // 3. Obtén las actividades basadas en los lotes obtenidos
                var actividades = _context.Actividad
                    .Where(a => lotesQuery.Contains(a.Id_lote))
                    .ToList();

                //lo bueno de esto es que se traduce a una sola consulta SQL eficiente, y no tengo que llamar a traer todo los campos, lotes y actividades a memoria para filtrarlos en C#.

                return Ok(actividades);
            }
            catch (FormatException)
            {
                return BadRequest("El Id de usuario en el token no es válido.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener las actividades." });
            }
        }


        [HttpGet("api/actividades/lotes/{id_lote}")]
        public IActionResult getActividadesDeLote(int id_lote)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(userid));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }
                var actividades = _context.Actividad.Where(a => a.Id_lote == id_lote).ToList();
                return Ok(actividades);
            }
            catch (System.Exception)
            {
                return StatusCode(500, new { message = "Error al obtener las actividades." });
            }
        }

        [HttpGet("api/actividades/{id_actividad}")]
        public IActionResult getActividadPorId(int id_actividad)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(userid));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }
                var actividad = _context.Actividad.Find(id_actividad);
                if (actividad == null)
                {
                    return NotFound(new { message = "Actividad no encontrada." });
                }
                return Ok(actividad);
            }
            catch (System.Exception)
            {
                return StatusCode(500, new { message = "Error al obtener la actividad." });
            }
        }

        [HttpPut("api/actividades/editar/{id_actividad}")]
        public IActionResult editarActividad(int id_actividad, [FromBody] Actividad actividadActualizada)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(userid));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }
                var actividad = _context.Actividad.Find(id_actividad);
                if (actividad == null)
                {
                    return NotFound(new { message = "Actividad no encontrada." });
                }
                actividad.descripcion = actividadActualizada.descripcion;
                actividad.fecha_inicio = actividadActualizada.fecha_inicio;
                actividad.fecha_fin = actividadActualizada.fecha_fin;
                actividad.cantidad_insumo = actividadActualizada.cantidad_insumo;
                actividad.costo = actividadActualizada.costo;
                _context.SaveChanges();
                return Ok(actividad);
            }
            catch (System.Exception)
            {
                return StatusCode(500, new { message = "Error al editar la actividad." });
            }

        }
    }
}