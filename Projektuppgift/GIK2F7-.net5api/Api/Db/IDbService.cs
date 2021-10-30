using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Entities;



namespace Api.Db
{
    public interface IDbService
    {
        void Setup();
        Task<Game> Add(Game game);
        Task<Game> Update(Game game);
        Task<IEnumerable<Game>> Get();
        Task<Game> Get(int id);
        Task<bool> Delete(int id);
    }
}