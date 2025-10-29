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





    }


}