using System;

namespace Orders.Api.Orders.Dtos;

public class RegisterOrderRequest
{
    public DateTime OrderDate { get; set; }
    public string PersonEmail { get; set; }
    public string OrderNo { get; set; }
    public string ProductName { get; set; }
    public int Total { get; set; }
    public decimal Price { get; set; }
}