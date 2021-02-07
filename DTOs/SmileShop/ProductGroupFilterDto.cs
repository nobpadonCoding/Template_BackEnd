namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
    public class ProductGroupFilterDto : PaginationDto
    {
        public string ProductGroupName { get; set; }

        //Ordering
        public string OrderingField { get; set; }
        public bool AscendingOrder { get; set; } = true;
    }
}