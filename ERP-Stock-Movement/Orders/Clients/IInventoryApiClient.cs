using ERP_Stock_Movement.Inventory.Models;

namespace ERP_Stock_Movement.Orders.Clients;

public interface IInventoryApiClient
{
    Task<TryDeductResponse> TryDeductAsync(TryDeductRequest request);
}
