using System.Collections.Generic;

namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
	public class AddOrderDto
	{
		public double Total { get; set; }
		public double TotalAmount { get; set; }
		public double Discount { get; set; }
		public List<OrderDetail> OrderDetail {get;set;}
	}
}