namespace PaymentService.Models;

public class Order
{
    public string OrderId { get; set; }
    public int Amount { get; set; }
    public string Status { get; set; }
}
