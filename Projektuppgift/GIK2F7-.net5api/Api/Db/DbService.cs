using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using System.IO;
using Api.Entities;

namespace Api.Db
{
    /// <summary>
    /// Database services
    /// </summary>
    public class DbService : IDbService
    {
        private DbConfig databaseConfig;

        public DbService(DbConfig dbConfig)
        {
            databaseConfig =  dbConfig;
        }

        /// <summary>
        /// Setup database connection
        /// </summary>
        public void Setup()
        {
            using (SqliteConnection connection = new SqliteConnection(databaseConfig.Name))
            {
                var table = connection.Query<string>("SELECT Name FROM sqlite_master WHERE type='table' AND name = 'Games'");
                var tableName = table.FirstOrDefault();
                if(!string.IsNullOrEmpty(tableName) && tableName == "Games")
                {
                    return;
                }
                using(var sr = new StreamReader(databaseConfig.StructureFile))
                {
                    var queries = sr.ReadToEnd();
                    connection.Execute(queries);
                }
            }
        }

        /// <summary>
        /// Add games to database
        /// </summary>
        /// <param name="game">game object</param>
        /// <returns>Game object</returns>
        public async Task<Game> Add(Game game)
        {
            using(var connection = new SqliteConnection(databaseConfig.Name))
            {
                var result = await connection.ExecuteAsync("INSERT INTO Games (Name, Description, Rating, Image) VALUES (@Name, @Description, @Rating, @Image)", game);
                var lInsert = await connection.QueryAsync<Game>("SELECT Id, Name, Description, Rating, Image FROM Games ORDER BY Id DESC");
                return new Game()
                {
                    Name = game.Name,
                    Description = game.Description,
                    Id = lInsert.FirstOrDefault<Game>().Id,
                    Rating = game.Rating,
                    Image = game.Image
                };
            }
        }

        /// <summary>
        /// Update game in database
        /// </summary>
        /// <param name="game">Game object</param>
        /// <returns>Game object</returns>
        public async Task<Game> Update(Game game)
        {
            using(var connection = new SqliteConnection(databaseConfig.Name))
            {
                var result = await connection.ExecuteAsync("UPDATE Games Set Name=@Name, Description=@Description, Rating=@Rating, Image=@Image WHERE Id=@Id", game);
                return game;
            }
        }

        /// <summary>
        /// Get data from database
        /// </summary>
        /// <returns>Game object</returns>
        public async Task<IEnumerable<Game>> Get()
        {
            using(var connection = new SqliteConnection(databaseConfig.Name))
            {
                var result = await connection.QueryAsync<Game>("SELECT Id, Name, Description, Rating, Image FROM Games ORDER BY Id ASC");
                return result;
            }
        }

        /// <summary>
        /// Get specified data from index id
        /// </summary>
        /// <param name="Id">database index id</param>
        /// <returns>Game object</returns>
        public async Task<Game> Get(int Id)
        {
            using(var connection = new SqliteConnection(databaseConfig.Name))
            {
                var result = await connection.QueryAsync<Game>("SELECT Id, Name, Description, Rating, Image FROM Games WHERE Id=@Id", new { Id });
                return result.FirstOrDefault<Game>();
            }
        }

        /// <summary>
        /// Delete data from database
        /// </summary>
        /// <param name="Id">Index id</param>
        /// <returns>bool</returns>
        public async Task<bool> Delete(int Id)
        {
            using(var connection = new SqliteConnection(databaseConfig.Name))
            {
                var result = await connection.ExecuteAsync("DELETE FROM Games WHERE Id=@Id", new { Id });
                if(result > -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
