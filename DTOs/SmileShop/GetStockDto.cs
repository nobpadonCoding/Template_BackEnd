using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
	public class GetStockDto
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public int ProductGroupId { get; set; }
		public string ProductGroupName { get; set; }
		public int OnHand { get; set; }
		public int AmountBefore { get; set; }
		public int AmountAfter { get; set; }
		public string StoreType { get; set; }

		[StringLength(100)]
		public string Remark { get; set; }
	}
}