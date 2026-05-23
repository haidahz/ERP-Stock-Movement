using ERP_Stock_Movement.Inventory.Models;
using ERP_Stock_Movement.Clients;
using ERP_Stock_Movement.Orders.Clients;
using ERP_Stock_Movement.Orders.Models;
using ERP_Stock_Movement.Orders.Repositories;

namespace ERP_Stock_Movement.Orders.Services;

public class OrderService
{
    private readonly OrderRepository _repository;
    private readonly IProductApiClient _productClient;
    private readonly IInventoryApiClient _inventoryClient;

    public OrderService(
        OrderRepository repository,
        IProductApiClient productClient,
        IInventoryApiClient inventoryClient)
    {
        _repository = repository;
        _productClient = productClient;
        _inventoryClient = inventoryClient;
    }

    public Task<List<Order>> GetAllAsync() => _repository.GetAllAsync();

    public Task<Order?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

    public async Task<(bool Success, Order? Order, string? Error)> CreateAsync(CreateOrderRequest request)
    {
        if (request.Items.Count == 0)
        {
            return (false, null, "Order must contain at least one item.");
        }

        var orderLines = new List<OrderItem>();

        foreach (var line in request.Items)
        {
            if (line.Quantity <= 0)
            {
                return (false, null, $"Quantity must be greater than zero for product {line.ProductId}.");
            }

            var product = await _productClient.GetByIdAsync(line.ProductId);
            if (product is null)
            {
                return (false, null,
                    $"Product {line.ProductId} was not found in the product catalog. Use GET /api/product to see valid ids, or create the product first.");
            }

            orderLines.Add(new OrderItem
            {
                ProductId = line.ProductId,
                Quantity = line.Quantity,
                UnitPrice = product.Price
            });
        }

        var deductRequest = new TryDeductRequest
        {
            Items = request.Items.Select(i => new StockLineRequest
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };

        var deductResult = await _inventoryClient.TryDeductAsync(deductRequest);
        if (!deductResult.Success)
        {
            return (false, null, deductResult.Message ?? "Insufficient stock.");
        }

        var order = new Order
        {
            CreatedAt = DateTime.UtcNow,
            Status = "Completed",
            Items = orderLines
        };

        var created = await _repository.CreateAsync(order);
        return (true, created, null);
    }
}
