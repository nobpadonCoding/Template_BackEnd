using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop;
using NetCoreAPI_Template_v3_with_auth.Services.SmileShop;

namespace NetCoreAPI_Template_v3_with_auth.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SmileShopController : ControllerBase
    {
        private readonly ISmileShopService _smileService;
        public SmileShopController(ISmileShopService smileService)
        {
            _smileService = smileService;

        }
        //test
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

        [HttpGet("ProductGroups")]
        public async Task<IActionResult> GetAllProductGroups()
        {
            return Ok(await _smileService.GetAllProductGroups());
        }

        [HttpPost("Products")]
        public async Task<IActionResult> AddProduct(AddProductDto newProduct)
        {
            return Ok(await _smileService.AddProduct(newProduct));
        }

        [HttpPut("Product/Update/{editProductId}")]
        public async Task<IActionResult> EditEmployee(EditProductDto editProduct)
        {
            return Ok(await _smileService.EditProduct(editProduct));
        }

        [HttpPost("ProductGroups")]
        public async Task<IActionResult> AddProductGroup(AddProductGroupDto newProductGroup)
        {
            return Ok(await _smileService.AddProductGroup(newProductGroup));
        }

        [HttpGet("Products/filter")]
        public async Task<IActionResult> GetProductFilter([FromQuery] ProductFilterDto ProductFilter)
        {
            return Ok(await _smileService.GetProductFilter(ProductFilter));
        }
    }
}