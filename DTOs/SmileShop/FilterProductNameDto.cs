namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
    public class FilterProductNameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StockCount { get; set; }
		public FilterProductGroupNameDto ProductGroup { get; set; }
    }
}