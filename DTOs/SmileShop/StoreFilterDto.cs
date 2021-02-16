namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
	public class StoreFilterDto : PaginationDto
	{
		public string ProductName { get; set; }

		//Ordering
		public string OrderingField { get; set; }
		public bool AscendingOrder { get; set; } = true;
	}
}