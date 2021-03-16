using System;

namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
	public class OrderFilterDto : PaginationDto
	{
		public string OrderNumber { get; set; }
		public DateTime? StartDate { get; set; } = null;
		public DateTime? EndDate { get; set; } = null;

		//Ordering
		public string OrderingField { get; set; }
		public bool AscendingOrder { get; set; } = true;
	}
}