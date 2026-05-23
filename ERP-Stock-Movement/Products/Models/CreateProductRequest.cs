namespace ERP_Stock_Movement.Products.Models;

public class CreateProductRequest
{
    public int ProductId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
}
