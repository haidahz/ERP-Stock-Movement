namespace ERP_Stock_Movement.Orders.Models;

public class CreateOrderRequest
{
    public List<CreateOrderLineRequest> Items { get; set; } = [];
}

public class CreateOrderLineRequest
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }
}
