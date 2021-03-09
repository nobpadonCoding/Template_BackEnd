using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI_Template_v3_with_auth.Models
{
	[Table("Order")]
	public class Orders
	{
		[Key]
		public int Id { get; set; }
		public Product Product { get; set; }
		public int ProductId { get; set; }
		public double ProductPrice { get; set; }
		public Boolean status { get; set; } = true;
		public int Quantity { get; set; }
		public DateTime CreatedDate { get; set; }
		public User CreatedBy { get; set; }
		public Guid CreatedById { get; set; }
		public OrderNo OrderNo { get; set; }
		public int OrderNoId { get; set; }

	}
}