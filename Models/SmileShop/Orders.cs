using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI_Template_v3_with_auth.Models
{
    [Table("Order")]
    public class Orders
    {
        public int OrderNoId { get; set; }
        public OrderNo OrderNo { get; set; }
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public double ProductPrice { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
        public double TotalAmount { get; set; }
        public int ItemCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Boolean status { get; set; } = true;
        public Product Product { get; set; }
    }
}