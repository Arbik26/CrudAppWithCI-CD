using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using CrudApp.Models;
using CrudApp.Services;

namespace CrudApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly MongoDbService _mongoService;

        public ProductsController(MongoDbService mongoService)
        {
            _mongoService = mongoService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var collection = _mongoService.GetProductsCollection();
            var products = await collection.Find(_ => true).ToListAsync();
            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var collection = _mongoService.GetProductsCollection();
            var product = await collection.Find(p => p.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
            if (product == null) return NotFound();
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            var collection = _mongoService.GetProductsCollection();
            await collection.InsertOneAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Product product)
        {
            var collection = _mongoService.GetProductsCollection();
            var result = await collection.ReplaceOneAsync(
                p => p.Id == ObjectId.Parse(id),
                product
            );
            if (result.MatchedCount == 0) return NotFound();
            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var collection = _mongoService.GetProductsCollection();
            var result = await collection.DeleteOneAsync(p => p.Id == ObjectId.Parse(id));
            if (result.DeletedCount == 0) return NotFound();
            return NoContent();
        }
    }
}
