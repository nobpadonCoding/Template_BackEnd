namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
    public class AddOrderDto
    {
        public int ProductId { get; set; }
        public double ProductPrice { get; set; }
        public double ProductDiscount { get; set; }
    }
}