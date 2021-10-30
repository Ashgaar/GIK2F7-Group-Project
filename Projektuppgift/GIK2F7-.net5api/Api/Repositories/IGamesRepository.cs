using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Repositories
{
    /// <summary>
    /// Interface for GamesRepository.
    /// </summary>
    public interface IGamesRepository
    {
        Task<Game> GetGameAsync(int id);
        Task<IEnumerable<Game>> GetGamesAsync();
        Task<Game> CreateGameAsync(Game game);
        Task<Game> UpdateGameAsync(Game game);
        Task<bool> DeleteGameAsync(int id);
    }
}