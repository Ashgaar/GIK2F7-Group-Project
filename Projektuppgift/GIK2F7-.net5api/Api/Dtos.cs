using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public record GameDto(int Id, string Name, string Description, float Rating, string Image);
    public record CreateGameDto([Required] string Name, string Description, [Range(1, 10)] float Rating, string Image);
    public record UpdateGameDto([Required] string Name, string Description, [Range(1, 10)] float Rating, string Image);
}