using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI_Template_v3_with_auth.Models
{
	[Table("ProductGroup")]
	public class ProductGroup
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime CreatedDate { get; set; }
		public Guid CreatedById { get; set; }
		public User CreatedBy { get; set; }
		public bool Status { get; set; } = true;
		public List<Product> Products { get; set; }
	}
}