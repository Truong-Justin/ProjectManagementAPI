using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.Repositories.EmployeeRepository;
using ProjectManagementAPI.Models.People;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }


        [Route("GetAllEmployees")]
        [HttpGet]
        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _employeeRepository.GetAllEmployeesAsync();
        }


        [Route("GetEmployeeById")]
        [HttpGet]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            Employee employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee.EmployeeId == 0)
            {
                return NotFound();
            }

            return employee;
        }


        [Route("GetEmployeeNames")]
        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> GetEmployeeNames()
        {
            IEnumerable<Employee> employees = await _employeeRepository.GetAllEmployeesAsync();
            return _employeeRepository.GetEmployeeNames(employees);
        }



        [Route("AddEmployee")]
        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            await _employeeRepository.AddEmployeeAsync(employee);
            return Ok();
        }


        [Route("UpdateEmployee")]
        [HttpPut]
        public async Task<ActionResult<Employee>> UpdateEmployee(int employeeId, string firstName, string lastName, DateOnly hireDate, string phone, string zip, string address, int projectId)
        {
            Employee employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            if (employee.EmployeeId == 0)
            {
                return NotFound();
            }

            await _employeeRepository.UpdateEmployeeAsync(employeeId, firstName, lastName, hireDate, phone, zip, address, projectId);
            return Ok();
        }


        [Route("DeleteEmployee")]
        [HttpDelete]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            Employee employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee.EmployeeId == 0)
            {
                return NotFound();
            }

            await _employeeRepository.DeleteEmployeeAsync(id);
            return Ok();
        }
    }
}

