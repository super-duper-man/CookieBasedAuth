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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeByID(int id)
        {
            var employee = await dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmplyees(EmployeeDto addEmployeeDto)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeDto employee)
        {
            var _employee = await dbContext.Employees.FindAsync(id);
            if (_employee == null)
            {
                return NotFound();
            }

            await Task.Run(() => {
                _employee.Email = employee.Email;
                _employee.Name = employee.Name;
                _employee.Phone = employee.Phone;
                _employee.Salary = employee.Salary;
            });

            await dbContext.SaveChangesAsync();

            return Ok(_employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await dbContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            dbContext.Employees.Remove(employee);
            await dbContext.SaveChangesAsync();

            return Ok();
        }


    }
}
