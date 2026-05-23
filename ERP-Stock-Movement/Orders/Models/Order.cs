namespace ERP_Stock_Movement.Orders.Models;

public class Order
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Status { get; set; } = string.Empty;

    public List<OrderItem> Items { get; set; } = [];
}
