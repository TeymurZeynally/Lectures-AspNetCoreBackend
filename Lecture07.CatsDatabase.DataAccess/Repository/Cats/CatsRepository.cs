using Dapper;
using Lecture07.CatsDatabase.DataAccess.Models.Cats;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Lecture07.CatsDatabase.DataAccess.Repository.Cats
{
    public class CatsRepository(string connectionString, ILogger<CatsRepository> logger) : ICatsRepository
    {
        // ПРИМЕР ТОГО КАК МОЖНО ОТПРАВЛЯТЬ ЗАПРОСЫ БЕЗ DAPPER-А
        //public async Task<Cat[]> GetAll(CancellationToken token)
        //{
        //    var sql = @"
        //        SELECT c.id, c.uid, c.user_id, u.uid, c.name, c.breed, c.age, c.created_at
        //        FROM cats.cats c
        //        INNER JOIN auth.users u ON u.id = c.user_id
        //        ORDER BY id;";

        //    await using var connection = new NpgsqlConnection(connectionString);
        //    await connection.OpenAsync(token);


        //    await using var command = new NpgsqlCommand(sql, connection);
        //    await using var reader = await command.ExecuteReaderAsync(token);

        //    var cats = new List<Cat>();

        //    while (await reader.ReadAsync(token))
        //    {
        //        cats.Add(new Cat
        //        {
        //            Id = reader.GetInt64(reader.GetOrdinal("id")),
        //            Uid = reader.GetGuid(reader.GetOrdinal("uid")),
        //            UserId = reader.GetInt64(reader.GetOrdinal("user_id")),
        //            UserUid = reader.GetGuid(reader.GetOrdinal("uid")),
        //            Name = reader.GetString(reader.GetOrdinal("name")),
        //            Breed = reader.GetString(reader.GetOrdinal("breed")),
        //            Age = reader.GetInt32(reader.GetOrdinal("age")),
        //            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
        //        });
        //    }

        //    return cats.ToArray();
        //}

        public async Task<Cat[]> GetAll(CancellationToken token)
        {
            const string sql = @"
                SELECT c.id, c.uid, c.user_id, u.uid as useruid, c.name, c.breed, c.age, c.created_at as createdat
                FROM cats.cats c
                INNER JOIN auth.users u ON u.id = c.user_id
                ORDER BY id;";

            logger.LogInformation("Executing query to get all cats.");

            await using var connection = new NpgsqlConnection(connectionString);

            var cats = (await connection.QueryAsync<Cat>(new CommandDefinition(sql, cancellationToken: token))).ToArray();

            logger.LogInformation("Query to get all cats returned {CatsCount} rows.", cats.Length);

            return cats;
        }

        //public async Task<Cat[]> SearchByName(string name, CancellationToken token)
        //{
        //    logger.LogWarning(@"
        //        SELECT c.id, c.uid, c.user_id, u.uid AS useruid, c.name, c.breed, c.age, c.created_at AS createdat
        //        FROM cats.cats c
        //        INNER JOIN auth.users u ON u.id = c.user_id
        //        WHERE c.name ILIKE '%{name}%'
        //        ORDER BY id;", name);

        //    var sql = $@"
        //        SELECT c.id, c.uid, c.user_id, u.uid AS useruid, c.name, c.breed, c.age, c.created_at AS createdat
        //        FROM cats.cats c
        //        INNER JOIN auth.users u ON u.id = c.user_id
        //        WHERE c.name ILIKE '%{name}%' ← НИКОГДА! НИ ПРИ КАКИХ ОБСТОЯТЕЛЬСТВАХ! НЕЛЬЗЯ ИНТЕРПОЛИРОВАТЬ СТРОКИ В SQL ЗАПРОСЫ (И В ЛЮБЫЕ ДРУГИЕ ЯЗЫКИ ЗАПРОСОВ)
        //        ORDER BY id;";

        // ВОЗМОЖНА SQL-ИНЪЕКЦИЯ ТИПА:
        //      ' AND 1 = 0 UNION SELECT id, uid, -1, uid, email, password, -1, '2000-01-01' FROM auth.users --

        //    await using var connection = new NpgsqlConnection(connectionString);

        //    return (await connection.QueryAsync<Cat>(sql)).ToArray();
        //}


        public async Task<Cat[]> SearchByName(string name, CancellationToken token)
        {
            const string sql = $@"
                SELECT c.id, c.uid, c.user_id, u.uid AS useruid, c.name, c.breed, c.age, c.created_at AS createdat
                FROM cats.cats c
                INNER JOIN auth.users u ON u.id = c.user_id
                WHERE c.name ILIKE @SearchPattern
                ORDER BY id;"; //  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑ ЕСЛИ НЕОБХОДИМО ПЕРЕДАВАТЬ ДАННЫЕ В ЗАПРОСЫ - ИСПОЛЬЗУЙТЕ ПАРАМЕТРЫ ЗАПРОСОВ

            logger.LogInformation("Executing query to search cats by name: {CatName}.", name);

            await using var connection = new NpgsqlConnection(connectionString);

            var cats = (await connection.QueryAsync<Cat>(new CommandDefinition(sql, new { SearchPattern = $"%{name}%" }, cancellationToken: token))).ToArray();

            logger.LogInformation("Search by name {CatName} returned {CatsCount} rows.", name, cats.Length);

            return cats;
        }


        public async Task<Cat?> GetByUid(Guid uid, CancellationToken token)
        {
            const string sql = """
                SELECT c.id, c.uid, c.user_id, u.uid AS useruid, c.name, c.breed, c.age, c.created_at AS createdat
                FROM cats.cats c
                INNER JOIN auth.users u ON u.id = c.user_id
                WHERE c.uid = @Uid
                LIMIT 1;
             """;

            logger.LogInformation("Executing query to get cat by uid: {CatUid}.", uid);

            await using var connection = new NpgsqlConnection(connectionString);

            var cat = await connection.QuerySingleOrDefaultAsync<Cat>(new CommandDefinition(sql, new { Uid = uid }, cancellationToken: token));

            logger.LogInformation(cat is null ? "Cat with uid {CatUid} was not found in repository." : "Cat with uid {CatUid} was found in repository.", uid);

            return cat;
        }

        public async Task<Cat> Create(Guid userUid, string name, string breed, int age, CancellationToken token)
        {
            const string sql = """
                INSERT INTO cats.cats (user_id, name, breed, age)
                SELECT u.id, @Name, @Breed, @Age
                FROM auth.users u
                WHERE u.uid = @UserUid
                RETURNING id, uid, user_id, @UserUid AS useruid, name, breed, age, created_at AS createdat;
                """;

            logger.LogInformation("Executing query to create cat. UserUid: {UserUid}, Name: {Name}, Breed: {Breed}, Age: {Age}.", userUid, name, breed, age);

            await using var connection = new NpgsqlConnection(connectionString);

            var command = new CommandDefinition(
                sql,
                new
                { UserUid = userUid, Name = name, Breed = breed, Age = age },
                cancellationToken: token);

            var cat = await connection.QuerySingleOrDefaultAsync<Cat>(command);

            if (cat is null)
            {
                logger.LogWarning("Failed to create cat because user with uid {UserUid} was not found.", userUid);
                throw new InvalidOperationException($"User with uid '{userUid}' was not found.");
            }

            logger.LogInformation("Successfully created cat with uid: {CatUid}.", cat.Uid);

            return cat;
        }

        public async Task<Cat?> Update(Guid uid, string name, string breed, int age, CancellationToken token)
        {
            const string sql = """
                UPDATE cats.cats c
                SET name = @Name, breed = @Breed, age = @Age
                WHERE c.uid = @Uid
                RETURNING
                    c.id,
                    c.uid,
                    c.user_id,
                    (SELECT u.uid FROM auth.users u WHERE u.id = c.user_id) AS useruid,
                    c.name,
                    c.breed,
                    c.age,
                    c.created_at AS createdat;
                """;

            logger.LogInformation("Executing query to update cat. CatUid: {CatUid}, Name: {Name}, Breed: {Breed}, Age: {Age}.", uid, name, breed, age);

            await using var connection = new NpgsqlConnection(connectionString);

            var command = new CommandDefinition(
                sql,
                new
                { Uid = uid, Name = name, Breed = breed, Age = age },
                cancellationToken: token);

            var cat = await connection.QuerySingleOrDefaultAsync<Cat>(command);

            logger.LogInformation(cat is null ? "Cat with uid {CatUid} was not found for update in repository." : "Cat with uid {CatUid} was updated in repository.", uid);

            return cat;
        }

        public async Task<bool> Delete(Guid uid, CancellationToken token)
        {
            const string sql = """
                DELETE FROM cats.cats
                WHERE uid = @Uid;
                """;

            logger.LogInformation("Executing query to delete cat by uid: {CatUid}.", uid);

            await using var connection = new NpgsqlConnection(connectionString);

            var command = new CommandDefinition(sql, new { Uid = uid }, cancellationToken: token);

            var affectedRows = await connection.ExecuteAsync(command);

            logger.LogInformation("Delete cat by uid {CatUid} affected {AffectedRows} rows.", uid, affectedRows);

            return affectedRows > 0;
        }
    }
}
