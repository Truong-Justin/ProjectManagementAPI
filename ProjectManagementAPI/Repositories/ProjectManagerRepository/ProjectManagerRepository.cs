using ProjectManagementAPI.Models.People;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace ProjectManagementAPI.Repositories.ProjectManagerRepository
{
	public class ProjectManagerRepository : IProjectManagerRepository
	{
		private readonly string _connectionString;


        public ProjectManagerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CONNECTION");
        }


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


		public IEnumerable<SelectListItem> GetProjectManagerNames(IEnumerable<ProjectManager> projectManagers)
		{
			return projectManagers.Select(projectManager => new SelectListItem { Value = projectManager.ProjectManagerId.ToString(), Text = projectManager.FirstName + " " + projectManager.LastName });
		}


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


		public async Task UpdateProjectManagerAsync(int projectManagerId, string firstName, string lastName, DateOnly hireDate, string phone, string zip, string address)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						UPDATE PROJECTMANAGERS SET
						FirstName = @firstName,
						LastName = @lastName,
						HireDate = @hireDate,
						Phone = @phone,
						Zip = @zip,
						Address = @address
						WHERE ProjectManagerId = @id
					";

					command.Parameters.AddWithValue("@firstName", firstName);
					command.Parameters.AddWithValue("@lastName", lastName);
					command.Parameters.AddWithValue("@hireDate", hireDate);
					command.Parameters.AddWithValue("@phone", phone);
					command.Parameters.AddWithValue("@zip", zip);
					command.Parameters.AddWithValue("@address", address);
					command.Parameters.AddWithValue("@id", projectManagerId);

					await command.ExecuteNonQueryAsync();
				}
			}
		}


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

