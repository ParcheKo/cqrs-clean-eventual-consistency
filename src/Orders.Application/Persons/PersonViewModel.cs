using System;

namespace Orders.Application.Persons;

public class PersonViewModel
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public int OrderCount { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal? HighestOrderTotalPrice { get; set; }
    public DateTime? FirstShoppingDate { get; set; }
    public DateTime? LastShoppingDate { get; set; }
}