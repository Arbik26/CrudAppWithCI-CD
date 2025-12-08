
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

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var collection = GetProductsCollection();
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            var collection = GetProductsCollection();
            var objectId = MongoDB.Bson.ObjectId.Parse(id);
            return await collection.Find(p => p.Id == objectId).FirstOrDefaultAsync();
        }

        public async Task<string> CreateProductAsync(Product product)
        {
            var collection = GetProductsCollection();
            await collection.InsertOneAsync(product);
            return product.Id.ToString();
        }

        public async Task<bool> UpdateProductAsync(string id, Product product)
        {
            var collection = GetProductsCollection();
            var objectId = MongoDB.Bson.ObjectId.Parse(id);
            var filter = Builders<Product>.Filter.Eq(p => p.Id, objectId);
            var update = Builders<Product>.Update
                .Set(p => p.Name, product.Name)
                .Set(p => p.Description, product.Description)
                .Set(p => p.Price, product.Price);
            var result = await collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            var collection = GetProductsCollection();
            var objectId = MongoDB.Bson.ObjectId.Parse(id);
            var filter = Builders<Product>.Filter.Eq(p => p.Id, objectId);
            var result = await collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}

