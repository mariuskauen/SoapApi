using AutoMapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoapApi.Data.Repositories
{
    public class QueryRepository
    {
        private readonly IMongoDatabase database;

        public QueryRepository(IMongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            database = client.GetDatabase(settings.DatabaseName);
        }
        public async Task<E> GetSingle<T, E>(T first, E second, string query) where T : class, new() where E : class, new()
        {
            string[] queries = query.Split(':');

            var collection = database.GetCollection<T>(queries[0]);

            var filter = Builders<T>.Filter.Eq(queries[1], queries[2]);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<T, E>());
            var mapper = new Mapper(config);
            if (queries.Length > 2)
            {
                filter = Builders<T>.Filter.Eq(queries[1], queries[2]);
            }
            else
            {
                filter = Builders<T>.Filter.Empty;
            }
            return mapper.Map<E>(await collection.Find(filter).FirstOrDefaultAsync());

        }

        public async Task<List<E>> GetList<T, E>(T source, E destination, string query) where T : class, new() where E : class, new()
        {
            string[] queries = query.Split(':');

            var collection = database.GetCollection<T>(queries[0]);

            FilterDefinition<T> filter;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<T, E>());
            var mapper = new Mapper(config);

            if (queries.Length > 2)
            {
                filter = Builders<T>.Filter.Eq(queries[1], queries[2]);
            }
            else
            {
                filter = Builders<T>.Filter.Empty;
            }
            return mapper.Map<List<E>>(await collection.Find(filter).ToListAsync());
        }
    }
}
