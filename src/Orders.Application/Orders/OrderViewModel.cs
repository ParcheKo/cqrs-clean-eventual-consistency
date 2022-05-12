using System;

namespace Orders.Application.Orders;

public class OrderViewModel
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string CreatedBy { get; set; }
    public string OrderNo { get; set; }
    public string ProductName { get; set; }
    public int Total { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice { get; set; }
    public string PersonName { get; set; }
}