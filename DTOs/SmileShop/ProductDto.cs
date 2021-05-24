using System;
using NetCoreAPI_Template_v3_with_auth.Models;

namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
	public class ProductDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Price { get; set; }
		public int StockCount { get; set; }
		public DateTime CreatedDate { get; set; }
		public UserDto CreatedBy { get; set; }
		// public string CreatedById { get; set; }
		public Boolean Status { get; set; }
	}
}