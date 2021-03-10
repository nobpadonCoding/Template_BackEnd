using System.Collections.Generic;
using NetCoreAPI_Template_v3_with_auth.Models;

namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
	public class GetOrderDto
	{
		public int Id { get; set; }
		public double Total { get; set; }
		public double TotalAmount { get; set; }
		public double Discount { get; set; }
		public List<OrdersDto> Orders {get;set;}
	}
}