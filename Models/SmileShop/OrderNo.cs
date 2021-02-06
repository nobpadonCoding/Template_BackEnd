using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI_Template_v3_with_auth.Models
{
     [Table("OrderNo")]
    public class OrderNo
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}