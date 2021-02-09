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
        public string CreatedBy { get; set; }
        public Boolean status { get; set; } = true;
        public DateTime CreatedDate { get; set; }
    }
}