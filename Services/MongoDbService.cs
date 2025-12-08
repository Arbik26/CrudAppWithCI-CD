using MongoDB.Driver;
using CrudApp.Models;

namespace CrudApp.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("MongoDb");
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("CrudDB");
        }

        public IMongoCollection<Product> GetProductsCollection()
        {
            return _database.GetCollection<Product>("products");
        }
    }
}
