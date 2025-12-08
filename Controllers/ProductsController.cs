using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using CrudApp.Models;
using CrudApp.Services;

namespace CrudApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMongoCollection<Product> _productsCollection;

        public ProductsController(MongoDbService mongoDbService)
        {
            _productsCollection = mongoDbService.GetProductsCollection();
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _productsCollection.Find(_ => true).ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, new ObjectId(id));
            var product = await _productsCollection.Find(filter).FirstOrDefaultAsync();
            
            if (product == null)
                return NotFound();
            
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productsCollection.InsertOneAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] Product product)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, new ObjectId(id));
            var update = Builders<Product>.Update
                .Set(p => p.Name, product.Name)
                .Set(p => p.Description, product.Description)
                .Set(p => p.Price, product.Price);
            
            await _productsCollection.UpdateOneAsync(filter, update);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, new ObjectId(id));
            await _productsCollection.DeleteOneAsync(filter);
            return Ok();
        }
    }
}
