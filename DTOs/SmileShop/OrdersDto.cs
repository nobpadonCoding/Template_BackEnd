using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop;

namespace NetCoreAPI_Template_v3_with_auth.Models
{
	public class OrdersDto
	{
		public int Id { get; set; }
		public GetProductDetail Product { get; set; }

	}
}