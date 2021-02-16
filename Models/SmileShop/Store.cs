using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI_Template_v3_with_auth.Models
{
	[Table("Store")]
	public class Store
	{
		[Key]
		public int Id { get; set; }
		public int Qty { get; set; }
		public int StockAfter { get; set; }
		public string StoreType { get; set; }
		public DateTime CreatedDate { get; set; }
		public Guid CreatedById { get; set; }
		public User CreatedBy { get; set; }

		[StringLength(100)]
		public string Remark { get; set; }
		public Product Product { get; set; }
		public int ProductId { get; set; }
		public int ProductStockCount { get; set; }

	}
}