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
        Task<ServiceResponse<GetProductDto>> EditProduct(EditProductDto editProduct);
    }
}