using ERP_Stock_Movement.Inventory.Models;
using ERP_Stock_Movement.Inventory.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERP_Stock_Movement.Inventory;

[ApiController]
[Route("api/inventory")]
public class InventoryController : ControllerBase
{
    private readonly InventoryService _service;

    public InventoryController(InventoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{productId:int}")]
    public async Task<IActionResult> GetByProductId(int productId)
    {
        var item = await _service.GetByProductIdAsync(productId);
        if (item is null)
        {
            return NotFound(new { message = $"No inventory record for product {productId}." });
        }

        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> SetStock([FromBody] SetStockRequest request)
    {
        return Ok(await _service.SetStockAsync(request));
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddStock([FromBody] SetStockRequest request)
    {
        return Ok(await _service.AddStockAsync(request));
    }

    [HttpPost("try-deduct")]
    public async Task<IActionResult> TryDeduct([FromBody] TryDeductRequest request)
    {
        var result = await _service.TryDeductAsync(request);
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
