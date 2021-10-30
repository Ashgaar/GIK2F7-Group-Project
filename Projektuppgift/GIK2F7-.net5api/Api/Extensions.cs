using Api.Dtos;
using Api.Entities;

namespace Api
{
    public static class Extensions
    {
        /// <summary>
        /// Creates game dto.
        /// </summary>
        /// <returns>Dto of game</returns>
        public static GameDto AsDto(this Game game)
        {
            return new GameDto(game.Id, game.Name, game.Description, game.Rating, game.Image);
        }
    }
}