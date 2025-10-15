using CookieBasedAuth.Data;
using CookieBasedAuth.Models;
using CookieBasedAuth.Models.Entities;
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

        [HttpPost]
        public async Task<IActionResult> AddEmplyees(AddEmployeeDto addEmployeeDto)
        {
            var employeeEntity = new Employee()
            {
                Name = addEmployeeDto.Name,
                Email = addEmployeeDto.Email,
                Phone = addEmployeeDto.Phone,
                Salary = addEmployeeDto.Salary
            };

            await dbContext.Employees.AddAsync(employeeEntity);
            await dbContext.SaveChangesAsync();

            return Ok(employeeEntity);
        }
    }
}
