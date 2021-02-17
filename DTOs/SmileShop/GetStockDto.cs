using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
	public class GetStockDto
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public int ProductStockCount { get; set; }
		public FilterProductNameDto Product { get; set; }
		public string CreatedByUsername { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Qty { get; set; }
		public int StockAfter { get; set; }
		public string StoreType { get; set; }

		[StringLength(100)]
		public string Remark { get; set; }
	}
}