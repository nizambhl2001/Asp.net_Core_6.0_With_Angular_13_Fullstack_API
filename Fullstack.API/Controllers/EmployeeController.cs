using Fullstack.API.Data;
using Fullstack.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fullstack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDbContext _DbContext;

        public EmployeeController(EmployeeDbContext dbContext)
        {
            _DbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           var employee =  await _DbContext.employees.ToListAsync();
            return Ok(employee);
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            employee.Id = Guid.NewGuid();
            await _DbContext.employees.AddAsync(employee);
            await _DbContext.SaveChangesAsync();
            return Ok(employee);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id)
        {
            var employee = await _DbContext.employees.FirstOrDefaultAsync(x => x.Id == id);
            if(employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, Employee Upemployee)
        {
            var employee = await _DbContext.employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            employee.Name = Upemployee.Name;
            employee.Email = Upemployee.Email;
            employee.Salary =Upemployee.Salary;
            employee.Phone = Upemployee.Phone;
            employee.Department = Upemployee.Department;

            await _DbContext.SaveChangesAsync();
            return Ok(employee);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var employee = await _DbContext.employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            _DbContext.employees.Remove(employee);
            await _DbContext.SaveChangesAsync();
            return Ok(employee);
        }
    }
}
