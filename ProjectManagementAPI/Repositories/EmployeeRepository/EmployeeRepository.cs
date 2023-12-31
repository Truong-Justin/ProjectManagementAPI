﻿using ProjectManagementAPI.Models.People;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectManagementAPI.Repositories.EmployeeRepository
{
    // Repository class holds the data-access logic
    // used to read and manipulate records from the
    // Employees table
    public class EmployeeRepository : IEmployeeRepository
	{
		private readonly string _connectionString;


        // IConfiguration service is dependency-injected
        // into repository class constructor, and used to
        // retrieve the connection string from host env variable
        public EmployeeRepository(IConfiguration configuration)
        {
			_connectionString = configuration.GetConnectionString("CONNECTION");
        }


		// Method returns a list of all Employee records from
		// the employees table
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


		// Method returns a specific Employee record
		// using the given Id supplied by caller of method
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


		// Method returns a collection of SelectListItem values that
		// contain all the first and last names of employees from the
		// Employees table
		public IEnumerable<SelectListItem> GetEmployeeNames(IEnumerable<Employee> employees)
		{
			return employees.Select(employee => new SelectListItem { Value = employee.EmployeeId.ToString(), Text = employee.FirstName + " " + employee.LastName });
		}


		// Method adds an employee record to the Employees table
		// with attributes supplied by caller of method
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


		// Method updates an employee record from the employees table
		// with attributes supplied by the caller of method
		public async Task UpdateEmployeeAsync(int employeeId, string phone, string zip, string address, int projectId)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						UPDATE EMPLOYEES SET
						Phone = @phone,
						Zip = @zip,
						Address = @address,
						ProjectId = @projectId
						WHERE EmployeeId = @employeeId
					";

                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@zip", zip);
                    command.Parameters.AddWithValue("@address", address);
                    command.Parameters.AddWithValue("@projectId", projectId);
                    command.Parameters.AddWithValue("@employeeId", employeeId);

					await command.ExecuteNonQueryAsync();
                }
			}
		}


		// Method deletes a specific bug record
		// from the Employees table with Id
		// supplied by the method caller
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

