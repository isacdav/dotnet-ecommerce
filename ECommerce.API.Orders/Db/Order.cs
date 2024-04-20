namespace ECommerce.API.Orders.Db;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItem> Items { get; set; } = [];
    public decimal Total { get; set; }
}
