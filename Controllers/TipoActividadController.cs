using api.agroapp.model;
using api_agroapp.lib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace api.agroapp.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TipoActividadController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;


        public TipoActividadController(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet("/api/tipoactividades/{id_tipo_actividad}")]
        public IActionResult GetTipoActividadPorId(int id_tipo_actividad)
        {
            try
            {

                var tipoActividad = _context.TipoActividad.Find(id_tipo_actividad);
                if (tipoActividad == null)
                {
                    return NotFound("Tipo de actividad no encontrado.");
                }
                return Ok(tipoActividad);
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al obtener el tipo de actividad: " + ex.Message);
            }
        }

        [HttpGet("/api/tipoactividades")]
        public IActionResult GetTipoActividades()
        {
            try
            {
                //no valido porque los tipo de actividades son publicos
                var tipoActividades = _context.TipoActividad.ToList();
                return Ok(tipoActividades);
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al obtener los tipos de actividades: " + ex.Message);
            }
        }

    }
}