using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementAPI.Models.People;

namespace ProjectManagementAPI.Repositories.EmployeeRepository
{
    // Interface defines a contract that requires
    // the EmployeeRepository class to implement all
    // declared methods defined here
    public interface IEmployeeRepository
	{
		Task<IEnumerable<Employee>> GetAllEmployeesAsync();
		IEnumerable<SelectListItem> GetEmployeeNames(IEnumerable<Employee> employees);
		Task<Employee> GetEmployeeByIdAsync(int id);
		Task AddEmployeeAsync(Employee employee);
		Task UpdateEmployeeAsync(int employeeId, string firstName, string lastName, DateOnly hireDate, string phone, string zip, string address, int projectId);
		Task DeleteEmployeeAsync(int id);
	}
}

