namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
    public class ProductFilterDto : PaginationDto
    {
        public string ProductName { get; set; }
        public string ProductGroupName { get; set; }
        // public string CreatedByUsername { get; set; }
        // public UserDto CreatedBy { get; set; }

        //Ordering
        public string OrderingField { get; set; }
        public bool AscendingOrder { get; set; } = true;
    }
}