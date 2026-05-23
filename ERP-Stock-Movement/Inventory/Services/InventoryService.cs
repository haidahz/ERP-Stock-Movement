using ERP_Stock_Movement.Clients;
using ERP_Stock_Movement.Inventory.Models;
using ERP_Stock_Movement.Inventory.Repositories;

namespace ERP_Stock_Movement.Inventory.Services;

public class InventoryService
{
    private readonly InventoryRepository _repository;
    private readonly IProductApiClient _productClient;

    public InventoryService(InventoryRepository repository, IProductApiClient productClient)
    {
        _repository = repository;
        _productClient = productClient;
    }

    public Task<List<InventoryItem>> GetAllAsync() => _repository.GetAllAsync();

    public Task<InventoryItem?> GetByProductIdAsync(int productId) =>
        _repository.GetByProductIdAsync(productId);

    public async Task<(bool Success, InventoryItem? Item, string? Error)> SetStockAsync(SetStockRequest request)
    {
        var product = await _productClient.GetByIdAsync(request.ProductId);
        if (product is null)
        {
            return (false, null,
                $"Product {request.ProductId} was not found. Create the product first and use the id returned from POST /api/product.");
        }

        var item = await _repository.SetStockAsync(request.ProductId, request.Quantity);
        return (true, item, null);
    }

    public async Task<(bool Success, InventoryItem? Item, string? Error)> AddStockAsync(SetStockRequest request)
    {
        var product = await _productClient.GetByIdAsync(request.ProductId);
        if (product is null)
        {
            return (false, null,
                $"Product {request.ProductId} was not found. Create the product first and use the id returned from POST /api/product.");
        }

        var item = await _repository.AddStockAsync(request.ProductId, request.Quantity);
        return (true, item, null);
    }

    public async Task<TryDeductResponse> TryDeductAsync(TryDeductRequest request)
    {
        var result = await _repository.TryDeductAsync(request.Items);

        if (!result.Success && result.FailedProductId is int productId)
        {
            result.Message = await BuildInsufficientStockMessageAsync(productId);
        }

        return result;
    }

    private async Task<string> BuildInsufficientStockMessageAsync(int productId)
    {
        var product = await _productClient.GetByIdAsync(productId);
        var productName = product?.Name ?? "Unknown";
        return $"Insufficient stock for product {productId} - {productName}.";
    }
}
