using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCoreAPI_Template_v3_with_auth.Data;
using NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop;
using NetCoreAPI_Template_v3_with_auth.Models;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using NetCoreAPI_Template_v3_with_auth.Helpers;
using System.Security.Claims;

namespace NetCoreAPI_Template_v3_with_auth.Services.SmileShop
{
    public class SmileShopService : ServiceBase,ISmileShopService
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<SmileShopService> _log;
        private readonly IHttpContextAccessor _httpContext;

        public SmileShopService(AppDBContext dbContext, IMapper mapper, ILogger<SmileShopService> log, IHttpContextAccessor httpContext) 
            : base(dbContext, mapper, httpContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _log = log;
            _httpContext = httpContext;
        }

        public async Task<ServiceResponse<GetProductDto>> AddProduct(AddProductDto newProduct)
        {
            try
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Name == newProduct.ProductName);
                if (!(product is null))
                {
                    return ResponseResult.Failure<GetProductDto>("Position duplicate!");
                }

                var productGroup = await _dbContext.ProductGroups.FirstOrDefaultAsync(x => x.Id == newProduct.ProductGroupId);
                if (productGroup is null)
                {
                    return ResponseResult.Failure<GetProductDto>($"ProductGroup id {newProduct.ProductGroupId} not Found!");
                }
                // var strDate = _serviceBase.Now().ToString("MM/dd/yyyy HH:mm:ss");
                // DateTime CreatedDate = DateTime.Parse(strDate);
                var product_new = new Product
                {
                    Name = newProduct.ProductName,
                    Price = newProduct.Price,
                    StockCount = newProduct.StockCount,
                    CreatedDate = Now(),
                    // CreatedBy = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Name),
                    CreatedBy = GetUsername(),
                    ProductGroupId = newProduct.ProductGroupId
                };

                _dbContext.Products.Add(product_new);
                await _dbContext.SaveChangesAsync();

                var product_retuen = _mapper.Map<GetProductDto>(product_new);
                _log.LogInformation($"Add Product Success");
                return ResponseResult.Success(product_retuen, "Success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetProductDto>(ex.Message);
            }

        }

        public async Task<ServiceResponse<GetProductGroupDto>> AddProductGroup(AddProductGroupDto newProductGroup)
        {
            try
            {
                var productGroup = await _dbContext.ProductGroups.FirstOrDefaultAsync(x => x.Name == newProductGroup.ProductGroupName);
                if (!(productGroup is null))
                {
                    return ResponseResult.Failure<GetProductGroupDto>("ProductGroup duplicate!");
                }

                var productGroup_new = new ProductGroup
                {
                    Name = newProductGroup.ProductGroupName
                };

                _dbContext.ProductGroups.Add(productGroup_new);
                await _dbContext.SaveChangesAsync();

                var productGroup_new_retuen = _mapper.Map<GetProductGroupDto>(productGroup_new);
                _log.LogInformation($"Add ProductGroup Success");
                return ResponseResult.Success(productGroup_new_retuen, "Success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetProductGroupDto>(ex.Message);
            }
        }

        public async Task<ServiceResponse<List<Product>>> GetProductFilter(ProductFilterDto ProductFilter)
        {
            var products_queryable = _dbContext.Products
                .Include(x => x.ProductGroup).AsQueryable();

            //Filter
            if (!string.IsNullOrWhiteSpace(ProductFilter.ProductName))
            {
                products_queryable = products_queryable.Where(x => x.Name.Contains(ProductFilter.ProductName));
            }

            // if (!string.IsNullOrWhiteSpace(EmployeeFilter.EmployeeDepartment))
            // {
            //     queryable = queryable.Where(x => x.Department.Contains(EmployeeFilter.EmployeeDepartment));
            // }

            //Ordering
            if (!string.IsNullOrWhiteSpace(ProductFilter.OrderingField))
            {
                try
                {
                    products_queryable = products_queryable.OrderBy($"{ProductFilter.OrderingField} {(ProductFilter.AscendingOrder ? "ascending" : "descending")}");
                }
                catch
                {
                    return ResponseResultWithPagination.Failure<List<Product>>($"Could not order by field: {ProductFilter.OrderingField}");
                }
            }

            var paginationResult = await _httpContext.HttpContext
                .InsertPaginationParametersInResponse(products_queryable, ProductFilter.RecordsPerPage, ProductFilter.Page);

            var ProductFilter_return = await products_queryable.Paginate(ProductFilter).ToListAsync();

            return ResponseResultWithPagination.Success(ProductFilter_return, paginationResult);
        }

        public async Task<ServiceResponse<List<GetProductGroupDto>>> GetAllProductGroups()
        {
            try
            {
                var productGroups = await _dbContext.ProductGroups.ToListAsync();

                //mapper Dto and return
                var productGroup_return = _mapper.Map<List<GetProductGroupDto>>(productGroups);

                _log.LogInformation("Get ProductGroups Success");
                return ResponseResult.Success(productGroup_return, "Success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<List<GetProductGroupDto>>(ex.Message);
            }
        }

        public async Task<ServiceResponse<List<GetProductDto>>> GetAllProducts()
        {
            try
            {
                var products = await _dbContext.Products
               .Include(x => x.ProductGroup).ToListAsync();

                //mapper Dto and return
                var product_return = _mapper.Map<List<GetProductDto>>(products);

                _log.LogInformation("GetProducts Success");
                return ResponseResult.Success(product_return, "Success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<List<GetProductDto>>(ex.Message);
            }
        }

        public async Task<ServiceResponse<GetProductDto>> EditProduct(EditProductDto editProduct)
        {
            try
            {
                //check department
                var produck = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == editProduct.ProductId);
                if (produck is null)
                {
                    return ResponseResult.Failure<GetProductDto>($"Position id {editProduct.ProductId} not found");
                }

                //assign value
                produck.Name = editProduct.ProductName;
                produck.Price = editProduct.ProductPrice;
                produck.ProductGroupId = editProduct.ProductGroupId;

                //update database
                _dbContext.Products.Update(produck);
                await _dbContext.SaveChangesAsync();

                //mapper Dto and return
                var dto = _mapper.Map<GetProductDto>(produck);
                _log.LogInformation($"Edit produck Success");
                return ResponseResult.Success(dto, "Success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetProductDto>(ex.Message);
            }
        }

        public async Task<ServiceResponse<GetProductDto>> GetProductById(int ProductId)
        {
            try
            {
                var product = await _dbContext.Products
                    .Include(x => x.ProductGroup)
                    .FirstOrDefaultAsync(x => x.Id == ProductId);
                //check employee
                if (product is null)
                {
                    _log.LogError($"employee id {ProductId} not found");
                    return ResponseResult.Failure<GetProductDto>($"employee id {ProductId} not found");
                }

                //mapper Dto and return
                var dto = _mapper.Map<GetProductDto>(product);

                _log.LogInformation("Get Product Success");
                return ResponseResult.Success(dto, "Success");
            }
            catch (Exception ex)
            {

                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetProductDto>(ex.Message);
            }
        }
    }
}