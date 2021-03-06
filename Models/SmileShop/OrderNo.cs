using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI_Template_v3_with_auth.Models
{
    [Table("OrderNo")]
    public class OrderNo
    {
        [Key]
        public int Id { get; set; }
        public int ProductQuantity { get; set; }
        public double Discount { get; set; }
		public double Total { get; set; }
		public double TotalAmount { get; set; }
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public Boolean status { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public List<Orders> Orders { get; set; }
    }
}