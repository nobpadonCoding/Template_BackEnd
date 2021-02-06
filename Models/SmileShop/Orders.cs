using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI_Template_v3_with_auth.Models
{
    [Table("Order")]
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public double ProductPrice { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
        public double TotalAmount { get; set; }
        public int ItemCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedBy { get; set; }
        public Product Products { get; set; }
    }
}