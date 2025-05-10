using System.Data;
using chatappapi.Models;
using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;

namespace chatappapi.Repositories
{
    public interface IUserRepository
    {
        Task<int> CreateUserAsync(User user);
        Task<User> GetUserByUsernameAsync(string username);
    }

    public class UserRepository : IUserRepository
    {
        IConfiguration _configuration;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IConfiguration config, ILogger<UserRepository> logger)
        {
            _configuration = config;
            _logger = logger;
        }

        public async Task<int> CreateUserAsync(User user)
        {
            //SQL
            const string sql = @"
                INSERT INTO Users (Username, PasswordHash)
                VALUES (@username, @password);
                SELECT SCOPE_IDENTITY();";

            try
            {
                //OPEN CONNECTION
                await using var connection = new SqlConnection(_configuration["DB:ConnectionString"]);

                await connection.OpenAsync();

                //COMMAND
                await using var command = new SqlCommand(sql, connection)
                {
                    CommandType = CommandType.Text
                };

                //SET PARAMETERS
                command.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar, 100) { Value = user.Username });
                command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar, 200) { Value = user.PasswordHash });



                //EXECUTE COMMAND
                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            string sql = @"
                SELECT Id, Username, PasswordHash
                FROM Users
                WHERE Username=@username;
            ";

            try
            {
                await using var connection = new SqlConnection(_configuration["DB:ConnectionString"]);

                await connection.OpenAsync();

                //COMMAND
                await using var command = new SqlCommand(sql, connection);

                //PARAMETERS
                command.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar, 100) { Value = username });

                //EXECUTE COMMANDd
                await using var reader = await command.ExecuteReaderAsync();
                if (!await reader.ReadAsync())
                {
                    return null;
                }

                //READ RESULTS
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash"))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error with username {username}", username);
                throw;
            }
        }
    }
}
