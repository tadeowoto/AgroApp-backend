using Microsoft.AspNetCore.Mvc;
using api.agroapp.model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;


namespace api.agroapp.controllers
{


    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UsuarioController : ControllerBase
    {
        private readonly DataContext _context;

        public UsuarioController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/usuario/hello")]
        [AllowAnonymous]
        public string Hello()
        {
            return "Hello World";
        }
    }
}