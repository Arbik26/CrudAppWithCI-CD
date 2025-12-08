using Microsoft.AspNetCore.Mvc;
using CrudApp.Services;
using CrudApp.Models;

namespace CrudApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public ProductsController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _mongoDbService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _mongoDbService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description ?? "",
                Price = request.Price
            };
            await _mongoDbService.CreateProductAsync(product);
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] UpdateProductRequest request)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description ?? "",
                Price = request.Price
            };
            var result = await _mongoDbService.UpdateProductAsync(id, product);
            return result ? Ok("Updated") : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var result = await _mongoDbService.DeleteProductAsync(id);
            return result ? Ok("Deleted") : NotFound();
        }
    }

    public class CreateProductRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateProductRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}
