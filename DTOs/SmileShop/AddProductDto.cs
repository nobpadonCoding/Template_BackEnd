using System.ComponentModel.DataAnnotations;
using NetCoreAPI_Template_v3_with_auth.Validations;

namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
    public class AddProductDto
    {
        [FirstLetterUpperCaseAttribute]
        [Required]
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int StockCount { get; set; }
        public int ProductGroupId { get; set; }
    }
}