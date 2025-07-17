using FactoryMES.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FactoryMES.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MachineTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMachineTypes()
        {
            var types = await _context.MachineTypes.ToListAsync();
            return Ok(types);
        }
    }
}