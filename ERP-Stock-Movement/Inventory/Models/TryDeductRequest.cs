namespace ERP_Stock_Movement.Inventory.Models;

public class TryDeductRequest
{
    public List<StockLineRequest> Items { get; set; } = [];
}
