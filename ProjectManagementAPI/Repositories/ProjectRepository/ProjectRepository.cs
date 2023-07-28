using System.Data;
using ProjectManagementAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectManagementAPI.Repositories
{
	public class ProjectRepository : IProjectRepository
	{
		private readonly string _connectionString;


        public ProjectRepository(IConfiguration configuration)
        {
            _connectionString = configuration["SQLCONNSTR_CONNECTION"];
        }


        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
		{
			Console.WriteLine(_connectionString);
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


        public async Task UpdateProjectAsync(int projectId, DateOnly startDate, string projectTitle, string description, string priority, int projectManagerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
						UPDATE PROJECTS SET
						StartDate = @startDate,
						ProjectTitle = @projectTitle,
						Description = @description,
						Priority = @priority,
						ProjectManagerId = @projectManagerId
						WHERE ProjectId = @id
					";

                    command.Parameters.AddWithValue("@startDate", startDate);
					command.Parameters.AddWithValue("@projectTitle", projectTitle);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@priority", priority);
                    command.Parameters.AddWithValue("@projectManagerId", projectManagerId);
                    command.Parameters.AddWithValue("@id", projectId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


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

