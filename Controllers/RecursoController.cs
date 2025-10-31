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

                var recurso = _context.Recurso.Find(id_recurso);
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


    }
}