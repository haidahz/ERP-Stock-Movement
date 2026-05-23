using ERP_Stock_Movement.Products.Services;
using Microsoft.AspNetCore.Mvc;
using ERP_Stock_Movement.Products.Models;

namespace ERP_Stock_Movement.Products
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetById(id);
            if (product is null)
            {
                return NotFound(new { message = $"Product {id} was not found." });
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            var (success, product, error) = await _service.CreateAsync(request);
            if (!success)
            {
                return BadRequest(new { message = error });
            }

            return CreatedAtAction(nameof(GetById), new { id = product!.Id }, product);
        }
    }
}
