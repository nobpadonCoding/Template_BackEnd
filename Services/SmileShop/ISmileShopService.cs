using System.Collections.Generic;
using System.Threading.Tasks;
using NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop;
using NetCoreAPI_Template_v3_with_auth.Models;

namespace NetCoreAPI_Template_v3_with_auth.Services.SmileShop
{
    public interface ISmileShopService
    {
        Task<ServiceResponse<List<GetProductDto>>> GetAllProducts();
        Task<ServiceResponse<GetProductDto>> GetProductById(int ProductId);
        Task<ServiceResponse<List<GetProductGroupDto>>> GetAllProductGroups();
        Task<ServiceResponse<GetProductDto>> AddProduct(AddProductDto newProduct);
        Task<ServiceResponse<GetProductGroupDto>> AddProductGroup(AddProductGroupDto newProductGroup);
        Task<ServiceResponse<List<Product>>> GetProductFilter(ProductFilterDto ProductFilter);
        Task<ServiceResponse<List<ProductGroup>>> GetProducGrouptFilter(ProductGroupFilterDto ProductGroupFilter);
        Task<ServiceResponse<GetProductDto>> EditProduct(int editProductId, EditProductDto editProduct);
        Task<ServiceResponse<GetProductGroupDto>> EditProductGroup(int editProductGroupId, EditProductGroupDto editProductGroup);
        Task<ServiceResponse<GetProductDto>> DeleteProduct(int ProductId);
        Task<ServiceResponse<GetProductGroupDto>> DeleteProductGroup(int ProductGroupId);
        // Task<ServiceResponse<List<GetOrderDto>>> GetOrder();
        // Task<ServiceResponse<GetOrderDto>> AddOrder(List<AddOrderDto> newOrder);
        Task<ServiceResponse<GetProductGroupDto>> GetProductGroupById(int ProductGroupId);
        Task<ServiceResponse<GetStockDto>> GetStock();
        Task<ServiceResponse<GetStockDto>> AddStock(AddstockDto newStock);
        Task<ServiceResponse<List<GetStockDto>>> GetStoreFilter(StoreFilterDto StoreFilter);
    }
}