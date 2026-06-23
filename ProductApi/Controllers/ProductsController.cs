using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Core.DTOs;
using ProductApi.Core.Entities;
using ProductApi.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ProductApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetAllAsync();
            var result = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                CategoryName = p.Category?.Name ?? "Kategorisiz",
                CreatedAt = p.CreatedAt
            }).ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            var result = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryName = product.Category?.Name ?? "Kategorisiz",
                CreatedAt = product.CreatedAt
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductCreateDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId
            };
            var created = await _productRepository.AddAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new ProductResponseDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                Price = created.Price,
                Stock = created.Stock,
                CategoryName = created.Category?.Name ?? "Kategorisiz",
                CreatedAt = created.CreatedAt
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductCreateDto dto)
        {
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Price = dto.Price;
            existing.Stock = dto.Stock;
            existing.CategoryId = dto.CategoryId;

            var updated = await _productRepository.UpdateAsync(existing);
            return Ok(new ProductResponseDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Description = updated.Description,
                Price = updated.Price,
                Stock = updated.Stock,
                CategoryName = updated.Category?.Name ?? "Kategorisiz",
                CreatedAt = updated.CreatedAt
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
