using NetCoreAPI_Template_v3_with_auth.Validations;

namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
    public class AddProductGroupDto
    {
        [FirstLetterUpperCaseAttribute]
        public string ProductGroupName { get; set; }
    }
}