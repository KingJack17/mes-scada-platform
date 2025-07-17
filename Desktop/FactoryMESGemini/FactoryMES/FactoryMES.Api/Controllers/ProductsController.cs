using FactoryMES.Business.Interfaces;
using FactoryMES.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FactoryMES.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

       
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            var (newProduct, errorMessage) = await _productService.CreateProductAsync(createProductDto);

            if (errorMessage != null)
            {
                return Conflict(errorMessage); // Servisten gelen hata mesajını dönüyoruz.
            }

            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            var (updatedProduct, errorMessage) = await _productService.UpdateProductAsync(id, updateProductDto);

            if (errorMessage != null)
            {
                return NotFound(errorMessage); // Servis ürün bulamadıysa 404 dön.
            }

            // İşlem başarılı. Standartlara uygun olarak 204 No Content dönüyoruz.
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProductAsync(id);

            if (!success)
            {
                // Servis false döndüyse, ürün bulunamamıştır.
                return NotFound();
            }

            // Başarılı silme sonrası 204 No Content yanıtı dönüyoruz.
            return NoContent();
        }
    }
}