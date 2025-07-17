using FactoryMES.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FactoryMES.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitOfMeasuresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UnitOfMeasuresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUnitOfMeasures()
        {
            var units = await _context.UnitOfMeasures.ToListAsync();
            return Ok(units);
        }
    }
}