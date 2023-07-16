using ProjectManagementAPI.Models;
using Microsoft.Data.Sqlite;
using System.Data;

namespace ProjectManagementAPI.Repositories
{
	public class ProjectRepository : IProjectRepository
	{

        private readonly string _connectionString;

        public ProjectRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("SQLiteDb");
		}


		public async Task<IEnumerable<Project>> GetAllProjectsAsync()
		{
			using (SqliteConnection connection = new SqliteConnection(_connectionString))
			{
                await connection.OpenAsync();
                List<Project> projects = new List<Project>();

                using (SqliteCommand command = new SqliteCommand("SELECT * FROM PROJECTS;", connection))
				{
					command.CommandType = CommandType.Text;

					using (SqliteDataReader reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							Project project = new Project()
							{
								ProjectId = Convert.ToInt32(reader["ProjectId"]),
								StartDate = DateOnly.Parse(reader["StartDate"].ToString()),
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
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
			{
				await connection.OpenAsync();
                Project project = new Project();

                using (SqliteCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						SELECT *
						FROM PROJECTS
						WHERE ProjectId = $id
					";

					command.Parameters.AddWithValue("$id", id);

					using (SqliteDataReader reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							project = new Project()
							{
								ProjectId = Convert.ToInt32(reader["ProjectId"]),
								StartDate = DateOnly.Parse(reader["StartDate"].ToString()),
								Description = (string)reader["Description"],
								Priority = (string)reader["Priority"],
								ProjectManagerId = Convert.ToInt32(reader["ProjectManagerId"])
							};
                        }
                    }
                }

				return project;
			}
		}


		public async Task AddProjectAsync(Project project)
		{
			using (SqliteConnection connection = new SqliteConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (SqliteCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						INSERT INTO PROJECTS (StartDate, Description, Priority, ProjectManagerId)
						VALUES ($startDate, $description, $priority, $projectManagerId)
					";

					command.Parameters.AddWithValue("$startDate", project.StartDate);
                    command.Parameters.AddWithValue("$description", project.Description);
                    command.Parameters.AddWithValue("$priority", project.Priority);
                    command.Parameters.AddWithValue("$projectManagerId", project.ProjectManagerId);

					await command.ExecuteNonQueryAsync();
                }
			}
		}


		public async Task UpdateProjectAsync(Project project)
		{
			using (SqliteConnection connection = new SqliteConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (SqliteCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						UPDATE PROJECTS SET
						StartDate = $startDate,
						Description = $description,
						Priority = $priority,
						ProjectManagerId = $projectManagerId
						WHERE ProjectId = $id
					";

					command.Parameters.AddWithValue("$startDate", project.StartDate);
                    command.Parameters.AddWithValue("$description", project.Description);
                    command.Parameters.AddWithValue("$priority", project.Priority);
                    command.Parameters.AddWithValue("$projectManagerId", project.ProjectManagerId);
                    command.Parameters.AddWithValue("$id", project.ProjectId);

					await command.ExecuteNonQueryAsync();
                }
			}
		}


		public async Task DeleteProjectAsync(int id)
		{
			using (SqliteConnection connection = new SqliteConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (SqliteCommand command = connection.CreateCommand())
				{
					command.CommandText =
					@"
						DELETE FROM PROJECTS
						WHERE ProjectID = $id
					";

					command.Parameters.AddWithValue("$id", id);

					await command.ExecuteNonQueryAsync();
				}
			}
		}
	}
}

