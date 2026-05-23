using ERP_Stock_Movement.Orders.Models;
using ERP_Stock_Movement.Orders.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERP_Stock_Movement.Orders;

[ApiController]
[Route("api/order")]
public class OrderController : ControllerBase
{
    private readonly OrderService _service;

    public OrderController(OrderService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _service.GetByIdAsync(id);
        if (order is null)
        {
            return NotFound(new { message = $"Order {id} was not found." });
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var (success, order, error) = await _service.CreateAsync(request);
        if (!success)
        {
            return BadRequest(new { message = error });
        }

        return CreatedAtAction(nameof(GetById), new { id = order!.Id }, order);
    }
}
