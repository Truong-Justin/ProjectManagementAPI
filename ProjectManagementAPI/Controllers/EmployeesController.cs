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


        // IEmployeeRepository service is dependency-injected
        // into class constructor so the entire EmployeesController
        // class can implement the methods defined by the supplied
        // IEmployeeRepository interface
        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }


        [Route("GetAllEmployees")]
        [HttpGet]
        public async Task<ActionResult> GetAllEmployees()
        {
            IEnumerable<Employee> employees = await _employeeRepository.GetAllEmployeesAsync();
            return Ok(employees);
        }


        [Route("GetEmployeeById")]
        [HttpGet]
        public async Task<ActionResult> GetEmployeeById(int employeeId)
        {
            Employee employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            if (employee.EmployeeId == 0)
            {
                return NotFound("An employee with the given ID doesn't exist.");
            }

            return Ok(employee);
        }


        [Route("GetEmployeeNames")]
        [HttpGet]
        public async Task<ActionResult> GetEmployeeNames()
        {
            IEnumerable<Employee> employees = await _employeeRepository.GetAllEmployeesAsync();
            IEnumerable<SelectListItem> employeeNames = _employeeRepository.GetEmployeeNames(employees);
            return Ok(employeeNames);
        }



        [Route("AddEmployee")]
        [HttpPost]
        public async Task<ActionResult> AddEmployee(Employee employee)
        {
            await _employeeRepository.AddEmployeeAsync(employee);
            return Ok("A new employee has been added.");
        }


        [Route("UpdateEmployee")]
        [HttpPut]
        public async Task<ActionResult> UpdateEmployee(int employeeId, string phone, string zip, string address, int projectId)
        {
            Employee employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            if (employee.EmployeeId == 0)
            {
                return NotFound("An employee with the given ID doesn't exist.");
            }

            await _employeeRepository.UpdateEmployeeAsync(employeeId, phone, zip, address, projectId);
            return Ok("The employee has been updated.");
        }


        [Route("DeleteEmployee")]
        [HttpDelete]
        public async Task<ActionResult> DeleteEmployee(int employeeId)
        {
            Employee employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            if (employee.EmployeeId == 0)
            {
                return NotFound("An employee with the given ID doesn't exist.");
            }

            await _employeeRepository.DeleteEmployeeAsync(employeeId);
            return Ok("The employee has been deleted.");
        }
    }
}

