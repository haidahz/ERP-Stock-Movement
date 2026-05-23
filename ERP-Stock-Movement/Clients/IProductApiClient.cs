using ERP_Stock_Movement.Products.Models;

namespace ERP_Stock_Movement.Clients;

public interface IProductApiClient
{
    Task<Product?> GetByIdAsync(int productId);
}
