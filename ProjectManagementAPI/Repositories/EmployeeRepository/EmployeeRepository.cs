using ProjectManagementAPI.Models.People;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectManagementAPI.Repositories.EmployeeRepository
{
	public class EmployeeRepository : IEmployeeRepository
	{
		private readonly string _connectionString;


        public EmployeeRepository(IConfiguration configuration)
        {
			_connectionString = configuration["SQLCONNSTR_CONNECTION"];
        }


        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				List<Employee> employees = new List<Employee>();

				using (SqlCommand command = new SqlCommand("SELECT * FROM EMPLOYEES", connection))
				{
					command.CommandType = CommandType.Text;

					using (SqlDataReader reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							Employee employee = new Employee()
							{
								EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
								FirstName = (string)reader["FirstName"],
								LastName = (string)reader["LastName"],
								HireDate = DateOnly.FromDateTime((DateTime)reader["HireDate"]),
								Phone = (string)reader["Phone"],
								Zip = (string)reader["Zip"],
								Address = (string)reader["Address"],
                                ProjectId = Convert.ToInt32(reader["ProjectId"])
                        };

							employees.Add(employee);
						}
					}
				}

                return employees;
            }
        }


		public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				Employee employee = new Employee();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						SELECT * FROM EMPLOYEES
						WHERE EmployeeId = @employeeId
					";

					command.Parameters.AddWithValue("@employeeId", employeeId);

					using (SqlDataReader reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							employee.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
							employee.FirstName = (string)reader["FirstName"];
							employee.LastName = (string)reader["LastName"];
							employee.HireDate = DateOnly.FromDateTime((DateTime)reader["HireDate"]);
							employee.Phone = (string)reader["Phone"];
							employee.Zip = (string)reader["Zip"];
							employee.Address = (string)reader["Address"];
							employee.ProjectId = Convert.ToInt32(reader["ProjectId"]);
						}
					}
				}

				return employee;
			}
		}


		public IEnumerable<SelectListItem> GetEmployeeNames(IEnumerable<Employee> employees)
		{
			return employees.Select(employee => new SelectListItem { Value = employee.EmployeeId.ToString(), Text = employee.FirstName + " " + employee.LastName });
		}


		public async Task AddEmployeeAsync(Employee employee)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						INSERT INTO EMPLOYEES (FirstName, LastName, HireDate, Phone, Zip, Address, ProjectId)
						VALUES (@firstName, @lastName, @hireDate, @phone, @zip, @address, @projectId)
					";

					command.Parameters.AddWithValue("@firstName", employee.FirstName);
                    command.Parameters.AddWithValue("@lastName", employee.LastName);
                    command.Parameters.AddWithValue("@hireDate", employee.HireDate);
                    command.Parameters.AddWithValue("@phone", employee.Phone);
                    command.Parameters.AddWithValue("@zip", employee.Zip);
                    command.Parameters.AddWithValue("@address", employee.Address);
                    command.Parameters.AddWithValue("@projectId", employee.ProjectId);

					await command.ExecuteNonQueryAsync();
                }
			}
		}


		public async Task UpdateEmployeeAsync(int employeeId, string firstName, string lastName, DateOnly hireDate, string phone, string zip, string address, int projectId)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						UPDATE EMPLOYEES SET
						FirstName = @firstName,
						LastName = @lastName,
						HireDate = @hireDate,
						Phone = @phone,
						Zip = @zip,
						Address = @address,
						ProjectId = @projectId
						WHERE EmployeeId = @employeeId
					";

					command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@lastName", lastName);
                    command.Parameters.AddWithValue("@hireDate", hireDate);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@zip", zip);
                    command.Parameters.AddWithValue("@address", address);
                    command.Parameters.AddWithValue("@projectId", projectId);
                    command.Parameters.AddWithValue("@employeeId", employeeId);

					await command.ExecuteNonQueryAsync();
                }
			}
		}


		public async Task DeleteEmployeeAsync(int id)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						DELETE FROM EMPLOYEES
						WHERE EmployeeId = @id
					";

					command.Parameters.AddWithValue("@id", id);

					await command.ExecuteNonQueryAsync();
				}
			}
		}
    }
}

