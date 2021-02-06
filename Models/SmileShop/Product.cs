using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI_Template_v3_with_auth.Models
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int StockCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }
        public List<Orders> Orders { get; set; }
    }
}