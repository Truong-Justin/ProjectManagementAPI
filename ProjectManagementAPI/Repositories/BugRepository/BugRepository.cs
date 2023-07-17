using Microsoft.Data.Sqlite;
using ProjectManagementAPI.Models;
using System.Data;

namespace ProjectManagementAPI.Repositories.BugRepository
{
	public class BugRepository : IBugRepository
	{
        private readonly string _connectionString;

        public BugRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SQLiteDb");
        }


        public async Task<IEnumerable<Bug>> GetAllBugsAsync()
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                List<Bug> bugs = new List<Bug>();

                using (SqliteCommand command = new SqliteCommand("SELECT * FROM BUGS;", connection))
                {
                    command.CommandType = CommandType.Text;

                    using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Bug bug = new Bug()
                            {
                                BugId = Convert.ToInt32(reader["BugId"]),
                                Date = DateOnly.Parse((string)reader["Date"]),
                                Description = (string)reader["Description"],
                                Priority = (string)reader["Priority"],
                                Assignment = (string)reader["Assignment"],
                                ProjectId = Convert.ToInt32(reader["ProjectId"])
                            };

                            bugs.Add(bug);
                        }
                    }
                }

                return bugs;
            }
        }


        public async Task<Bug> GetBugByIdAsync(int id)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                Bug bug = new Bug();

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
						SELECT *
						FROM BUGS
						WHERE BugId = $id
					";

                    command.Parameters.AddWithValue("$id", id);

                    using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            bug.BugId = Convert.ToInt32(reader["BugId"]);
                            bug.Date = DateOnly.Parse((string)reader["Date"]);
                            bug.Description = (string)reader["Description"];
                            bug.Priority = (string)reader["Priority"];
                            bug.Assignment = (string)reader["Assignment"];
                            bug.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                        }
                    }
                }

                return bug;
            }
        }


        public async Task AddBugAsync(Bug bug)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
        	            INSERT INTO BUGS (Date, Description, Priority, Assignment, ProjectId)
                        VALUES ($date, $description, $priority, $assignment, $projectId)
                    ";

                    command.Parameters.AddWithValue("$date", bug.Date);
                    command.Parameters.AddWithValue("$description", bug.Description);
                    command.Parameters.AddWithValue("$priority", bug.Priority);
                    command.Parameters.AddWithValue("$assignment", bug.Assignment);
                    command.Parameters.AddWithValue("$projectId", bug.ProjectId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


        public async Task UpdateBugAsync(int bugId, DateOnly date, string description, string priority, string assignment)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
        	            UPDATE BUGS SET
                        Date = $date,
                        Description = $description,
                        Priority = $priority,
                        Assignment = $assignment
                        WHERE BugId = $id
        	            
                    ";

                    command.Parameters.AddWithValue("$date", date);
                    command.Parameters.AddWithValue("$description", description);
                    command.Parameters.AddWithValue("$priority", priority);
                    command.Parameters.AddWithValue("$assignment", assignment);
                    command.Parameters.AddWithValue("$id", bugId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


        public async Task DeleteBugAsync(int id)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
        	            DELETE FROM BUGS
        	            WHERE BugId = $id
                    ";

                    command.Parameters.AddWithValue("$id", id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

