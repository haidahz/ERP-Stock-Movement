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

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            return Ok(await _service.Create(product));
        }
    }
}
