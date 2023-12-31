﻿using System.Data;
using ProjectManagementAPI.Models;
using ProjectManagementAPI.Models.People;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ProjectManagementAPI.Repositories
{
    // Repository class holds the data-access logic
    // used to read and manipulate records from the
    // Projects table
    public class ProjectRepository : IProjectRepository
	{
		private readonly string _connectionString;


        // IConfiguration service is dependency-injected
        // into repository class constructor, and used to
        // retrieve the connection string from host env variable
        public ProjectRepository(IConfiguration configuration)
        {
			_connectionString = configuration.GetConnectionString("CONNECTION");
        }


		// Method returns a list of all Project
		// records from the Projects table 
        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
                await connection.OpenAsync();
                List<Project> projects = new List<Project>();

                using (SqlCommand command = new SqlCommand("SELECT * FROM PROJECTS;", connection))
				{
					command.CommandType = CommandType.Text;

					using (SqlDataReader reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							Project project = new Project()
							{
								ProjectId = Convert.ToInt32(reader["ProjectId"]),
								Date = DateOnly.FromDateTime((DateTime)reader["StartDate"]),
								ProjectTitle = (string)reader["ProjectTitle"],
								Description = (string)reader["Description"],
								Priority = (string)reader["Priority"],
								ProjectManagerId = Convert.ToInt32(reader["ProjectManagerId"])
							};

							projects.Add(project);
						}
					}
				}

                return projects;
            }
		}


		// Method returns all bugs a project has
		// using an Inner join between the
		// Bugs amd Projects table using the projectId
		// supplied by method caller
		public async Task<IEnumerable<Bug>> GetAllBugsForProjectAsync(int projectId)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				List<Bug> bugsList = new List<Bug>();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						SELECT Bugs.BugId, Bugs.Description
						FROM Bugs INNER JOIN Projects
						ON Bugs.ProjectId = Projects.ProjectId
						WHERE Projects.ProjectId = @projectId
					";

					command.Parameters.AddWithValue("@projectId", projectId);

					using (SqlDataReader reader = await command.ExecuteReaderAsync())
					{
						while (reader.Read())
						{
							Bug newBug = new Bug();

							newBug.BugId = Convert.ToInt32(reader["BugId"]);
							newBug.Description = Convert.ToString(reader["Description"]);

							bugsList.Add(newBug);
						}
					}
				}

				return bugsList.AsReadOnly();
			}
		}


		// Method returns all employees a project has
		// using an Inner Join between the
		// Employees and Projects table using the projectId
		// supplied by the method caller
		public async Task<IEnumerable<Employee>> GetAllEmployeesForProjectAsync(int projectId)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				List<Employee> employeesList = new List<Employee>();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						SELECT Employees.EmployeeId, Employees.FirstName, Employees.LastName
						FROM Employees INNER JOIN Projects
						ON Employees.ProjectId = Projects.ProjectId
						WHERE Projects.ProjectId = @projectId
					";

					command.Parameters.AddWithValue("@projectId", projectId);

					using (SqlDataReader reader = await command.ExecuteReaderAsync())
					{
						while (reader.Read())
						{
							Employee newEmployee = new Employee();

							newEmployee.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
							newEmployee.FirstName = Convert.ToString(reader["FirstName"]);
							newEmployee.LastName = Convert.ToString(reader["LastName"]);

							employeesList.Add(newEmployee);
						}
					}
				}

				return employeesList.AsReadOnly();
			}
		}


		// Method returns a specfic Project record
		// from the Projects table using the given Id
		// supplied by the method caller
		public async Task<Project> GetProjectByIdAsync(int id)
		{
            using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				Project project = new Project();

                using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						SELECT *
						FROM PROJECTS
						WHERE ProjectId = @id
					";

					command.Parameters.AddWithValue("@id", id);

					using (SqlDataReader reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							project.ProjectId = Convert.ToInt32(reader["ProjectId"]);
							project.Date = DateOnly.FromDateTime((DateTime)reader["StartDate"]);
							project.ProjectTitle = (string)reader["ProjectTitle"];
							project.Description = (string)reader["Description"];
							project.Priority = (string)reader["Priority"];
							project.ProjectManagerId = Convert.ToInt32(reader["ProjectManagerId"]);
                        }
                    }
                }

				return project;
			}
		}


		// Method returns a collection of SelectListItem values
		// that contain all the Project Titles and their associated
		// Ids from the Projects table
        public IEnumerable<SelectListItem> GetProjectTitles(IEnumerable<Project> projects)
        {
            return projects.Select(project => new SelectListItem { Value = project.ProjectId.ToString(), Text = project.ProjectTitle });
        }


        public async Task AddProjectAsync(Project project)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						INSERT INTO PROJECTS (StartDate, ProjectTitle, Description, Priority, ProjectManagerId)
						VALUES (@startDate, @projectTitle, @description, @priority, @projectManagerId)
					";

					command.Parameters.AddWithValue("@startDate", project.Date);
					command.Parameters.AddWithValue("@projectTitle", project.ProjectTitle);
                    command.Parameters.AddWithValue("@description", project.Description);
                    command.Parameters.AddWithValue("@priority", project.Priority);
                    command.Parameters.AddWithValue("@projectManagerId", project.ProjectManagerId);

					await command.ExecuteNonQueryAsync();
                }
			}
		}


		// Method updates a Project record from the
		// Projects table with the attributes supplied
		// by the method caller
        public async Task UpdateProjectAsync(int projectId, string projectTitle, string description, string priority, int projectManagerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
						UPDATE PROJECTS SET
						ProjectTitle = @projectTitle,
						Description = @description,
						Priority = @priority,
						ProjectManagerId = @projectManagerId
						WHERE ProjectId = @id
					";

					command.Parameters.AddWithValue("@projectTitle", projectTitle);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@priority", priority);
                    command.Parameters.AddWithValue("@projectManagerId", projectManagerId);
                    command.Parameters.AddWithValue("@id", projectId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


		// Method deletes a specific Project record
		// from the Projects table using the Id
		// supplied by method caller
        public async Task DeleteProjectAsync(int id)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						DELETE FROM PROJECTS
						WHERE ProjectID = @id
					";

					command.Parameters.AddWithValue("@id", id);

					await command.ExecuteNonQueryAsync();
				}
			}
		}
	}
}

