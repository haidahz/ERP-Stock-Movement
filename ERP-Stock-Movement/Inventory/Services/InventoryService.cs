using ERP_Stock_Movement.Inventory.Models;
using ERP_Stock_Movement.Inventory.Repositories;

namespace ERP_Stock_Movement.Inventory.Services;

public class InventoryService
{
    private readonly InventoryRepository _repository;

    public InventoryService(InventoryRepository repository)
    {
        _repository = repository;
    }

    public Task<List<InventoryItem>> GetAllAsync() => _repository.GetAllAsync();

    public Task<InventoryItem?> GetByProductIdAsync(int productId) =>
        _repository.GetByProductIdAsync(productId);

    public Task<InventoryItem> SetStockAsync(SetStockRequest request) =>
        _repository.SetStockAsync(request.ProductId, request.Quantity);

    public Task<InventoryItem> AddStockAsync(SetStockRequest request) =>
        _repository.AddStockAsync(request.ProductId, request.Quantity);

    public Task<TryDeductResponse> TryDeductAsync(TryDeductRequest request) =>
        _repository.TryDeductAsync(request.Items);
}
