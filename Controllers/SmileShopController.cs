using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop;
using NetCoreAPI_Template_v3_with_auth.Services.SmileShop;

namespace NetCoreAPI_Template_v3_with_auth.Controllers
{
	// [Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class SmileShopController : ControllerBase
	{
		private readonly ISmileShopService _smileService;
		public SmileShopController(ISmileShopService smileService)
		{
			_smileService = smileService;

		}

		[HttpGet("Products")]
		public async Task<IActionResult> GetAllProducts()
		{
			return Ok(await _smileService.GetAllProducts());
		}

		[HttpGet("Product/{ProductId}")]
		public async Task<IActionResult> GetEmployeeById(int ProductId)
		{
			return Ok(await _smileService.GetProductById(ProductId));
		}

		[HttpPost("Products")]
		public async Task<IActionResult> AddProduct(AddProductDto newProduct)
		{
			return Ok(await _smileService.AddProduct(newProduct));
		}

		[HttpPut("Product/Update/{editProductId}")]
		public async Task<IActionResult> EditProduct(int editProductId, EditProductDto editProduct)
		{
			return Ok(await _smileService.EditProduct(editProductId, editProduct));
		}

		[HttpDelete("Product/{ProductId}")]
		public async Task<IActionResult> DeleteProduct(int ProductId)
		{
			return Ok(await _smileService.DeleteProduct(ProductId));
		}

		[HttpGet("Products/filter")]
		public async Task<IActionResult> GetProductFilter([FromQuery] ProductFilterDto ProductFilter)
		{
			return Ok(await _smileService.GetProductFilter(ProductFilter));
		}

		//Order

		// [HttpPost("Order")]
		// public async Task<IActionResult> AddOrder(List<AddOrderDto> newOrder)
		// {
		//     return Ok(await _smileService.AddOrder(newOrder));
		// }

		//product Group

		[HttpGet("ProductGroups")]
		public async Task<IActionResult> GetAllProductGroups()
		{
			return Ok(await _smileService.GetAllProductGroups());
		}

		[HttpGet("ProductGroups/{ProductGroupId}")]
		public async Task<IActionResult> GetProductGroupById(int ProductGroupId)
		{
			return Ok(await _smileService.GetProductGroupById(ProductGroupId));
		}

		[HttpPost("ProductGroups")]
		public async Task<IActionResult> AddProductGroup(AddProductGroupDto newProductGroup)
		{
			return Ok(await _smileService.AddProductGroup(newProductGroup));
		}

		[HttpPut("ProductGroup/Update/{editProductGroupId}")]
		public async Task<IActionResult> EditProductGroup(int editProductGroupId, EditProductGroupDto editProductGroup)
		{
			return Ok(await _smileService.EditProductGroup(editProductGroupId, editProductGroup));
		}

		[HttpDelete("ProductGroup/{ProductGroupId}")]
		public async Task<IActionResult> DeleteProductGroup(int ProductGroupId)
		{
			return Ok(await _smileService.DeleteProductGroup(ProductGroupId));
		}

		[HttpGet("ProductGroup/filter")]
		public async Task<IActionResult> GetProductFroupFilter([FromQuery] ProductGroupFilterDto ProductGroupFilter)
		{
			return Ok(await _smileService.GetProducGrouptFilter(ProductGroupFilter));
		}

		[HttpPost("Stock")]
		public async Task<IActionResult> AddStock(AddstockDto newStock)
		{
			return Ok(await _smileService.AddStock(newStock));
		}

		[HttpGet("Stock/filter")]
		public async Task<IActionResult> GetStoreFilter([FromQuery] StoreFilterDto StoreFilter)
		{
			return Ok(await _smileService.GetStoreFilter(StoreFilter));
		}

		[HttpPost("Order")]
		public async Task<IActionResult> AddOrder(AddOrderDto newOrder)
		{
			return Ok(await _smileService.AddOrder(newOrder));
		}

		[HttpGet("Orders/filter")]
		public async Task<IActionResult> GetOrderFilter([FromQuery] OrderFilterDto OrderFilter)
		{
			return Ok(await _smileService.GetOrderFilter(OrderFilter));
		}

        [HttpGet("Order/{orderNumber}")]
		public async Task<IActionResult> GetOrder(int orderNumber)
		{
			return Ok(await _smileService.GetOrder(orderNumber));
		}

	}
}