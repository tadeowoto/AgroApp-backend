
using api.agroapp.model;
using api_agroapp.lib;
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


    }
}