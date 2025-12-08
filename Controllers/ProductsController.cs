using Microsoft.AspNetCore.Mvc;
using CrudApp.Services;
using CrudApp.Models;

namespace CrudApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly MongoDbService _mongoDbService;

    public ProductsController(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {
            var products = await _mongoDbService.GetAllProductsAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        try
        {
            var product = await _mongoDbService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound("Product not found");
            return Ok(product);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Product name is required");

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description ?? "",
                Price = request.Price
            };

            var id = await _mongoDbService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id }, product);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    // PUT: api/products/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(string id, [FromBody] UpdateProductRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Product ID is required");

            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Product name is required");

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description ?? "",
                Price = request.Price
            };

            var result = await _mongoDbService.UpdateProductAsync(id, product);
            
            if (!result)
                return NotFound("Product not found");

            return Ok(new { message = "Product updated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Product ID is required");

            var result = await _mongoDbService.DeleteProductAsync(id);
            
            if (!result)
                return NotFound("Product not found");

            return Ok(new { message = "Product deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
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

