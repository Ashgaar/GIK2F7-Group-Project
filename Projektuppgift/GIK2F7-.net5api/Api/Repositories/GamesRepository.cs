using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Entities;
using Api.Repositories;
using Api.Db;

namespace Api.Repositories
{
    public class GamesRepository : IGamesRepository
    {
        private readonly DbConfig databaseConfig;
        private readonly IDbService db;

        /// <summary>
        /// Constructor to initialize variables.
        /// </summary>
        public GamesRepository(DbConfig dbConfig, IDbService db)
        {
            databaseConfig = dbConfig;
            this.db = db;
        }

        /// <summary>
        /// Adds newly created game to database.
        /// </summary>
        /// <returns>Game</returns>
        public async Task<Game> CreateGameAsync(Game game)
        {
            return await db.Add(game);
        }

        /// <summary>
        /// Updates game in database.
        /// </summary>
        /// <returns>Game</returns>
        public async Task<Game> UpdateGameAsync(Game game)
        {
            return await db.Update(game);
        }

        /// <summary>
        /// Delete selected game from database.
        /// </summary>
        /// <returns>Game</returns>
        public async Task<bool> DeleteGameAsync(int id)
        {
            return await db.Delete(id);
        }

        /// <summary>
        /// Gets a selected game from database.
        /// </summary>
        /// <returns>Game</returns>
        public async Task<Game> GetGameAsync(int id)
        {
            return await db.Get(id);
        }

        /// <summary>
        /// Gets all games from database.
        /// </summary>
        /// <returns>Game</returns>
        public async Task<IEnumerable<Game>> GetGamesAsync()
        {
            var games = await db.Get();
            return games;
        }
    }
}