
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
    }
}