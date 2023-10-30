using ProjectManagementAPI.Models.People;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementAPI.Models;

namespace ProjectManagementAPI.Repositories.ProjectManagerRepository
{
    // Repository class holds the data-access logic
    // used to read and manipulate records from the
    // ProjectManagers table
    public class ProjectManagerRepository : IProjectManagerRepository
	{
		private readonly string _connectionString;


        // IConfiguration service is dependency-injected
        // into repository class constructor, and used to
        // retrieve the connection string from host env variable
        public ProjectManagerRepository(IConfiguration configuration)
        {
			_connectionString = configuration.GetConnectionString("CONNECTION");
        }


		// Method returns a list of all Project Manager records
		// from the ProjectManagers table
        public async Task<IEnumerable<ProjectManager>> GetAllProjectManagersAsync()
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				List<ProjectManager> projectManagers = new List<ProjectManager>();

				using (SqlCommand command = new SqlCommand("SELECT * FROM ProjectManagers", connection))
				{
					command.CommandType = CommandType.Text;

					using (SqlDataReader reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							ProjectManager projectManager = new ProjectManager
							{
								ProjectManagerId = Convert.ToInt32(reader["ProjectManagerId"]),
								FirstName = (string)reader["FirstName"],
								LastName = (string)reader["LastName"],
								HireDate = DateOnly.FromDateTime((DateTime)reader["HireDate"]),
								Phone = (string)reader["Phone"],
								Zip = (string)reader["Zip"],
								Address = (string)reader["Address"]
							};

							projectManagers.Add(projectManager);
						}
					}
				}

				return projectManagers;
			}
		}


		// Method returns a specific Project Manager
		// record using the given Id supplied by method caller
		public async Task<ProjectManager> GetProjectManagerByIdAsync(int projectManagerId)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				ProjectManager projectManager = new ProjectManager();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						SELECT * FROM PROJECTMANAGERS
						WHERE ProjectManagerId = @id
					";

					command.Parameters.AddWithValue("@id", projectManagerId);

					using (SqlDataReader reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							projectManager.ProjectManagerId = Convert.ToInt32(reader["ProjectManagerId"]);
							projectManager.FirstName = (string)reader["FirstName"];
							projectManager.LastName = (string)reader["LastName"];
							projectManager.HireDate = DateOnly.FromDateTime((DateTime)reader["HireDate"]);
							projectManager.Phone = (string)reader["Phone"];
							projectManager.Zip = (string)reader["Zip"];
							projectManager.Address = (string)reader["Address"];
						}
					}
				}

				return projectManager;
			}
		}


		// Method returns all a list of all the projects
		// that a Project Manager is in charge of using the
		// given project manager Id supplied by method caller
        public async Task<IEnumerable<Project>> GetAllProjectsForManagerAsync(int projectManagerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
				List<Project> projectsList = new List<Project>();

                using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						SELECT Projects.ProjectTitle, Projects.ProjectId
						FROM Projects INNER JOIN ProjectManagers
						ON Projects.ProjectManagerId = ProjectManagers.ProjectManagerId
						WHERE Projects.ProjectManagerId = @projectManagerId
					";

					command.Parameters.AddWithValue("@projectManagerId", projectManagerId);

					using (SqlDataReader reader = await command.ExecuteReaderAsync())
					{
						while (reader.Read())
						{
							Project newProject = new Project();

							newProject.ProjectTitle = Convert.ToString(reader["ProjectTitle"]);
							newProject.ProjectId = Convert.ToInt32(reader["ProjectId"]);

							projectsList.Add(newProject);
						}
					}
				}

                return projectsList.AsReadOnly();
            }
        }


        // Method returns a collection of SelectListItem values
        // that contain all the names of Project Managers and their
        // associated Ids from the ProjectManagers table
        public IEnumerable<SelectListItem> GetProjectManagerNames(IEnumerable<ProjectManager> projectManagers)
		{
			return projectManagers.Select(projectManager => new SelectListItem { Value = projectManager.ProjectManagerId.ToString(), Text = projectManager.FirstName + " " + projectManager.LastName });
		}


		// Method adds a Project Manager record to the ProjectManagers
		// table with attributes supplied by the method caller
		public async Task AddProjectManagerAsync(ProjectManager projectManager)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						INSERT INTO PROJECTMANAGERS (FirstName, LastName, HireDate, Phone, Zip, Address)
						VALUES (@firstName, @lastName, @hireDate, @phone, @zip, @address)
					";

					command.Parameters.AddWithValue("@firstName", projectManager.FirstName);
					command.Parameters.AddWithValue("@lastName", projectManager.LastName);
					command.Parameters.AddWithValue("@hireDate", projectManager.HireDate);
					command.Parameters.AddWithValue("@phone", projectManager.Phone);
					command.Parameters.AddWithValue("@zip", projectManager.Zip);
					command.Parameters.AddWithValue("@address", projectManager.Address);

					await command.ExecuteNonQueryAsync();
				}
			}
		}


        // Method updates a Project Manager record from
        // the ProjectManagers table with the attributes
        // supplied by the method caller
        public async Task UpdateProjectManagerAsync(int projectManagerId, string phone, string zip, string address)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						UPDATE PROJECTMANAGERS SET
						Phone = @phone,
						Zip = @zip,
						Address = @address
						WHERE ProjectManagerId = @id
					";

					command.Parameters.AddWithValue("@phone", phone);
					command.Parameters.AddWithValue("@zip", zip);
					command.Parameters.AddWithValue("@address", address);
					command.Parameters.AddWithValue("@id", projectManagerId);

					await command.ExecuteNonQueryAsync();
				}
			}
		}


		// Method deletes a Project Manager
		// from the ProjectManagers table
		// using the Id supplied by method caller
		public async Task DeleteProjectManagerAsync(int id)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
                    @"
						DELETE FROM PROJECTMANAGERS
						WHERE ProjectManagerId = @id
					";

					command.Parameters.AddWithValue("@id", id);

					await command.ExecuteNonQueryAsync();
				}
			}
		}
	}
}

