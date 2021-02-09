using System;
using System.Collections.Generic;
using NetCoreAPI_Template_v3_with_auth.Models;

namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
    public class GetOrderDto
    {
        public int OrderNoId { get; set; }
        public int ProductId { get; set; }
        public double ProductPrice { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
        public double TotalAmount { get; set; }
        public int ItemCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public List<Product> Product { get; set; }
    }
}