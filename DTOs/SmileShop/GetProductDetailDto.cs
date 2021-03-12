using NetCoreAPI_Template_v3_with_auth.Models;

namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
	public class GetProductDetailDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
	}
}