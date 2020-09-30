using MongoDB.Driver;
using System.Threading.Tasks;

namespace SoapApi.Data.Repositories
{
    public class CommandRepository
    {
        private readonly IMongoDatabase database;
        public CommandRepository(IMongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            database = client.GetDatabase(settings.DatabaseName);
        }

        public async Task Post<T>(T model, string query)
        {
            string[] queries = query.Split(':');

            var collection = database.GetCollection<T>(queries[0]);

            await collection.InsertOneAsync(model);
        }

        public async Task Put<T>(T model, string query)
        {
            string[] queries = query.Split(':');

            var collection = database.GetCollection<T>(queries[0]);
            var filter = Builders<T>.Filter.Eq(queries[1], queries[2]);
            if(queries.Length > 3)
            {
                switch (queries[3])
                {
                    case "insertinto":
                        var insert = Builders<T>.Update.AddToSet(queries[4], queries[5]);
                        await collection.UpdateOneAsync(filter, insert);
                        break;

                    case "set":
                        var set = Builders<T>.Update.Set(queries[4], queries[5]);
                        await collection.UpdateOneAsync(filter, set);
                        break;
                }
            }

        }
    }
}
