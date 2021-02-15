using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI_Template_v3_with_auth.Models.SmileShop
{
    [Table("Store")]
    public class Store
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductGroupId { get; set; }
        public int OnHand { get; set; }
        public int AmountBefore { get; set; }
        public int AmountAfter { get; set; }

        [StringLength(100)]
        public string Remark { get; set; }
        public Product Product { get; set; }
        public ProductGroup ProductGroup { get; set; }
    }
}