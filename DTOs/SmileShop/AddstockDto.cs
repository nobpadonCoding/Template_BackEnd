using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
	public class AddstockDto
	{
		public int ProductId { get; set; }
		// public int ProductGroupId { get; set; }
		// public int AmountQty { get; set; }
		public int Qty { get; set; }
		public string StoreType { get; set; }

		[StringLength(100)]
		public string Remark { get; set; }
	}
}