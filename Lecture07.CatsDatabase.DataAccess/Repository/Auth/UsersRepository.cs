using Dapper;
using Lecture07.CatsDatabase.DataAccess.Models.Auth;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Lecture07.CatsDatabase.DataAccess.Repository.Auth
{
    public sealed class UsersRepository(string connectionString, ILogger<UsersRepository> logger) : IUsersRepository
    {
        public async Task<User[]> GetAll(CancellationToken token)
        {
            const string sql = """
                SELECT id, uid, username, email, password, created_at AS createdat
                FROM auth.users
                ORDER BY id;
                """;

            logger.LogInformation("Executing query to get all users.");

            await using var connection = new NpgsqlConnection(connectionString);

            var command = new CommandDefinition(sql, cancellationToken: token);

            var users = (await connection.QueryAsync<User>(command)).ToArray();

            logger.LogInformation("Query to get all users returned {UsersCount} rows.", users.Length);

            return users;
        }

        public async Task<User?> GetByUid(Guid uid, CancellationToken token)
        {
            const string sql = """
                SELECT id, uid, username, email, password, created_at AS createdat
                FROM auth.users
                WHERE uid = @Uid
                LIMIT 1;
                """;

            logger.LogInformation("Executing query to get user by uid: {UserUid}.", uid);

            await using var connection = new NpgsqlConnection(connectionString);

            var command = new CommandDefinition(sql, new { Uid = uid }, cancellationToken: token);

            var user = await connection.QuerySingleOrDefaultAsync<User>(command);

            logger.LogInformation(user is null ? "User with uid {UserUid} was not found in repository." : "User with uid {UserUid} was found in repository.", uid);

            return user;
        }

        public async Task<User> Create(string username, string email, string password, CancellationToken token)
        {
            const string sql = """
                INSERT INTO auth.users (username, email, password)
                VALUES (@Username, @Email, @Password)
                RETURNING id, uid, username, email, password, created_at AS createdat;
                """;

            logger.LogInformation("Executing query to create user. Username: {Username}, Email: {Email}.", username, email);

            await using var connection = new NpgsqlConnection(connectionString);

            var command = new CommandDefinition(sql, new { Username = username, Email = email, Password = password }, cancellationToken: token);

            var user = await connection.QuerySingleAsync<User>(command);

            logger.LogInformation("Successfully created user with uid: {UserUid}.", user.Uid);

            return user;
        }

        public async Task<User?> Update(Guid uid, string username, string email, CancellationToken token)
        {
            const string sql = """
                UPDATE auth.users
                SET username = @Username, email = @Email
                WHERE uid = @Uid
                RETURNING id, uid, username, email, password, created_at AS createdat;
                """;

            logger.LogInformation("Executing query to update user. UserUid: {UserUid}, Username: {Username}, Email: {Email}.", uid, username, email);

            await using var connection = new NpgsqlConnection(connectionString);

            var command = new CommandDefinition(sql, new { Uid = uid, Username = username, Email = email }, cancellationToken: token);

            var user = await connection.QuerySingleOrDefaultAsync<User>(command);

            logger.LogInformation(user is null ? "User with uid {UserUid} was not found for update in repository." : "User with uid {UserUid} was updated in repository.", uid);

            return user;
        }

        public async Task<bool> Delete(Guid uid, CancellationToken token)
        {
            const string sql = """
                DELETE FROM auth.users
                WHERE uid = @Uid;
                """;

            logger.LogInformation("Executing query to delete user by uid: {UserUid}.", uid);

            await using var connection = new NpgsqlConnection(connectionString);

            var command = new CommandDefinition(sql, new { Uid = uid }, cancellationToken: token);

            var affectedRows = await connection.ExecuteAsync(command);

            logger.LogInformation("Delete user by uid {UserUid} affected {AffectedRows} rows.", uid, affectedRows);

            return affectedRows > 0;
        }
    }
}