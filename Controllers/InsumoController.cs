
using api.agroapp.model;
using api_agroapp.lib;
using api_agroapp.model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace api.agroapp.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InsumoController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly HashPasswordService _hashPasswordService;

        public InsumoController(DataContext context, HashPasswordService hashPasswordService, IConfiguration config)
        {
            _context = context;
            _hashPasswordService = hashPasswordService;
            _config = config;
        }


        [HttpGet("/api/insumos/{id_insumo}")]
        public IActionResult GetInsumoPorId(int id_insumo)
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

                var insumo = _context.Insumo.Find(id_insumo);
                if (insumo == null)
                {
                    return NotFound("Insumo no encontrado.");
                }

                return Ok(insumo);
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al obtener el insumo: " + ex.Message);
            }

        }

        [HttpGet("/api/insumos")]
        public IActionResult GetInsumos()
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

                var insumos = _context.Insumo.Where(i => i.id_usuario == user.id_usuario).ToList();
                if (insumos == null || insumos.Count == 0)
                {
                    return NotFound("No se encontraron insumos para el usuario especificado.");
                }
                return Ok(insumos);
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al obtener los insumos: " + ex.Message);
            }
        }

        [HttpPut("/api/insumos/actualizar/{id_insumo}")]
        public IActionResult editarInsumo(int id_insumo, [FromBody] Insumo insumoActualizado)
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                var insumoExistente = _context.Insumo.Find(id_insumo);
                if (insumoExistente == null)
                {
                    return NotFound("Insumo no encontrado.");
                }

                if (insumoExistente.id_usuario != user.id_usuario)
                {
                    return Forbid("No tienes permiso para editar este insumo.");
                }
                insumoExistente.nombre = insumoActualizado.nombre;
                insumoExistente.tipo = insumoActualizado.tipo;
                insumoExistente.unidad = insumoActualizado.unidad;
                insumoExistente.stock_actual = insumoActualizado.stock_actual;
                insumoExistente.fecha_vencimiento = insumoActualizado.fecha_vencimiento;

                _context.SaveChanges();

                return Ok(new { message = "Insumo actualizado correctamente." });

            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al actualizar el insumo: " + ex.Message);
            }
        }

        [HttpPost("/api/insumos/crear")]
        public IActionResult AgregarInsumo([FromBody] Insumo nuevoInsumo)
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                nuevoInsumo.id_usuario = user.id_usuario;

                _context.Insumo.Add(nuevoInsumo);
                _context.SaveChanges();

                return Ok(nuevoInsumo);
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al agregar el insumo: " + ex.Message);
            }
        }

        [HttpGet("/api/insumos/cantidad")]
        public IActionResult GetCantidadInsumos()
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                var cantidadInsumos = _context.Insumo.Count(i => i.id_usuario == user.id_usuario);

                return Ok(cantidadInsumos);
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al obtener la cantidad de insumos: " + ex.Message);
            }
        }

        [HttpPut("api/insumos/restarInsumo/{id_insumo}")]
        public IActionResult RestarInsumo(int id_insumo, [FromBody] decimal cantidadARestar)
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }

                var insumoExistente = _context.Insumo.Find(id_insumo);
                if (insumoExistente == null)
                {
                    return NotFound("Insumo no encontrado.");
                }

                if (insumoExistente.id_usuario != user.id_usuario)
                {
                    return Forbid("No tienes permiso para modificar este insumo.");
                }

                insumoExistente.stock_actual -= cantidadARestar;
                _context.SaveChanges();

                return Ok(new { message = "Stock de insumo actualizado correctamente.", stock_actual = insumoExistente.stock_actual });

            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al restar el insumo: " + ex.Message);
            }
        }

        [HttpGet("/api/insumos/stockActual/{id_insumo}")]
        public IActionResult GetStockActual(int id_insumo, [FromQuery] decimal cantidadARestar)
        {
            try
            {
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var user = _context.Usuarios.Find(int.Parse(idUsuarioClaim));
                if (user == null || user.id_usuario == null)
                {
                    return Forbid("No tienes permiso para acceder a este recurso.");
                }
                var insumoExistente = _context.Insumo.Find(id_insumo);
                if (insumoExistente == null)
                {
                    return NotFound("Insumo no encontrado.");
                }
                if (insumoExistente.id_usuario != user.id_usuario)
                {
                    return Forbid("No tienes permiso para ver este insumo.");
                }
                if (insumoExistente.stock_actual < cantidadARestar)
                {
                    return StatusCode(409, new { message = "No hay suficiente stock disponible" });
                }
                return Ok(insumoExistente.stock_actual);
            }
            catch (System.Exception ex)
            {
                return BadRequest("Error al obtener el stock actual: " + ex.Message);
            }


        }

    }
}