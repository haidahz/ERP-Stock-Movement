using ERP_Stock_Movement.Inventory.Data;
using ERP_Stock_Movement.Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP_Stock_Movement.Inventory.Repositories;

public class InventoryRepository
{
    private readonly InventoryDbContext _context;

    public InventoryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<InventoryItem>> GetAllAsync()
    {
        return await _context.InventoryItems.AsNoTracking().ToListAsync();
    }

    public async Task<InventoryItem?> GetByProductIdAsync(int productId)
    {
        return await _context.InventoryItems.AsNoTracking()
            .FirstOrDefaultAsync(i => i.ProductId == productId);
    }

    public async Task<InventoryItem> SetStockAsync(int productId, int quantity)
    {
        var item = await _context.InventoryItems.FirstOrDefaultAsync(i => i.ProductId == productId);
        if (item is null)
        {
            item = new InventoryItem { ProductId = productId, Quantity = quantity };
            _context.InventoryItems.Add(item);
        }
        else
        {
            item.Quantity = quantity;
        }

        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<InventoryItem> AddStockAsync(int productId, int quantity)
    {
        var item = await _context.InventoryItems.FirstOrDefaultAsync(i => i.ProductId == productId);
        if (item is null)
        {
            item = new InventoryItem { ProductId = productId, Quantity = quantity };
            _context.InventoryItems.Add(item);
        }
        else
        {
            item.Quantity += quantity;
        }

        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<TryDeductResponse> TryDeductAsync(List<StockLineRequest> items)
    {
        if (items.Count == 0)
        {
            return new TryDeductResponse { Success = false, Message = "No items to deduct." };
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        var productIds = items.Select(i => i.ProductId).Distinct().ToList();
            var inventoryItems = await _context.InventoryItems
                .Where(i => productIds.Contains(i.ProductId))
                .ToListAsync();

            foreach (var line in items)
            {
                if (line.Quantity <= 0)
                {
                    await transaction.RollbackAsync();
                    return new TryDeductResponse
                    {
                        Success = false,
                        Message = $"Quantity must be greater than zero for product {line.ProductId}."
                    };
                }

                var item = inventoryItems.FirstOrDefault(i => i.ProductId == line.ProductId);
                if (item is null || item.Quantity < line.Quantity)
                {
                    await transaction.RollbackAsync();
                    return new TryDeductResponse
                    {
                        Success = false,
                        Message = $"Insufficient stock for product {line.ProductId}."
                    };
                }
            }

            foreach (var line in items)
            {
                var item = inventoryItems.First(i => i.ProductId == line.ProductId);
                item.Quantity -= line.Quantity;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

        return new TryDeductResponse { Success = true, Message = "Stock deducted successfully." };
    }
}
