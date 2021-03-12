using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop;

namespace NetCoreAPI_Template_v3_with_auth.Models
{
	public class OrdersProductDetailDto
	{
		public int Id { get; set; }
		public int Quantity { get; set; }
		public int ProductId { get; set; }
		public GetProductDetailDto Product { get; set; }

	}
}