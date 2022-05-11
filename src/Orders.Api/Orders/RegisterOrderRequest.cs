using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SampleProject.Application.Orders;

namespace SampleProject.API.Orders
{
    public class RegisterOrderRequest
    {
        public DateTime OrderDate { get; set; }
        public string CreatedBy { get; set; }
        public string OrderNo { get; set; }
        public string ProductName { get; set; }
        public int Total { get; set; }
        public decimal Price { get; set; }
    }
}