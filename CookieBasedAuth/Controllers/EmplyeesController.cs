using CookieBasedAuth.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookieBasedAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmplyeesController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public EmplyeesController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmplyees()
        {
            var employees = await this.dbContext.Employees.ToListAsync();

            return Ok(employees);
        }
    }
}
