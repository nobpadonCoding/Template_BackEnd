using System;

namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
	public class GetOrderFilterDto
	{
		public int Id { get; set; }
		public int ProductQuantity { get; set; }
		public double Discount { get; set; }
		public double Total { get; set; }
		public double TotalAmount { get; set; }
		public Guid CreatedBy { get; set; }
		public Boolean status { get; set; } = true;
		public DateTime CreatedDate { get; set; }
	}
}