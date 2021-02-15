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
	public class SmileShopService : ServiceBase, ISmileShopService
	{
		private readonly AppDBContext _dbContext;
		private readonly IMapper _mapper;
		private readonly ILogger<SmileShopService> _log;
		private readonly IHttpContextAccessor _httpContext;

		public SmileShopService(AppDBContext dbContext, IMapper mapper,
		ILogger<SmileShopService> log, IHttpContextAccessor httpContext) : base(dbContext, mapper, httpContext)
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
					return ResponseResult.Failure<GetProductDto>("Product Name duplicate!");
				}

				var productGroup = await _dbContext.ProductGroups.FirstOrDefaultAsync(x => x.Id == newProduct.ProductGroupId);
				if (productGroup is null)
				{
					return ResponseResult.Failure<GetProductDto>($"ProductGroup id {newProduct.ProductGroupId} not Found!");
				}

				var product_new = new Product
				{
					Name = newProduct.ProductName,
					Price = newProduct.Price,
					StockCount = newProduct.StockCount,
					CreatedDate = Now(),
					CreatedBy = GetUsername(),
					UserId = Guid.Parse(GetUserId()),
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
					Name = newProductGroup.ProductGroupName,
					CreatedBy = GetUsername(),
					UserIdCreated = Guid.Parse(GetUserId()),
					CreatedDate = Now()
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
					_log.LogInformation($"Order by Success");
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
			_log.LogInformation($"Product Filter Success");

			return ResponseResultWithPagination.Success(ProductFilter_return, paginationResult);
		}

		public async Task<ServiceResponse<List<GetProductGroupDto>>> GetAllProductGroups()
		{
			try
			{
				var productGroups = await _dbContext.ProductGroups.Include(x=>x.Products).ToListAsync();

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

		public async Task<ServiceResponse<GetProductDto>> EditProduct(int editProductId, EditProductDto editProduct)
		{
			try
			{
				//check Product
				var product = await _dbContext.Products
				.Include(x => x.ProductGroup)
				.FirstOrDefaultAsync(x => x.Id == editProductId);
				if (product is null)
				{
					return ResponseResult.Failure<GetProductDto>($"Position id {editProductId} not found");
				}

				var productGroup = await _dbContext.ProductGroups.FirstOrDefaultAsync(x => x.Id == editProduct.ProductGroupId);
				if (productGroup is null)
				{
					return ResponseResult.Failure<GetProductDto>($"ProductGroup id {editProduct.ProductGroupId} not Found!");
				}

				//assign value
				product.Name = editProduct.ProductName;
				product.Price = editProduct.ProductPrice;
				product.ProductGroupId = editProduct.ProductGroupId;
				product.Status = editProduct.ProductStatus;

				//update database
				_dbContext.Products.Update(product);
				await _dbContext.SaveChangesAsync();

				//mapper Dto and return
				var dto = _mapper.Map<GetProductDto>(product);
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
					_log.LogError($"Product id {ProductId} not found");
					return ResponseResult.Failure<GetProductDto>($"Product id {ProductId} not found");
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

		public Task<ServiceResponse<List<GetOrderDto>>> GetOrder()
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResponse<GetOrderDto>> AddOrder(List<AddOrderDto> newOrder)
		{
			try
			{
				foreach (var item in newOrder)
				{
					var product = _dbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
					if (!(product is null))
					{
						if (product.Result.StockCount <= 0)
						{
							_log.LogError($"{product.Result.Name} StockCount < 0");
							return ResponseResult.Failure<GetOrderDto>($"{product.Result.Name} StockCount < 0");
						}
						else
						{
							var xxx = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
							var StockCount = product.Result.StockCount - 1;
							xxx.Name = product.Result.Name;
							xxx.Price = product.Result.Price;
							xxx.StockCount = StockCount;
							xxx.ProductGroupId = xxx.ProductGroupId;
							// StockCount_return.Append(StockCount);
							_dbContext.Products.Update(xxx);
						}
					}
				}
				// await _dbContext.SaveChangesAsync();

				var runNo = new OrderNo
				{
					CreatedDate = Now()
				};

				_dbContext.OrderNos.Add(runNo);
				await _dbContext.SaveChangesAsync();

				var order_ch = _dbContext.Orders.FirstOrDefaultAsync(x => x.OrderNoId == runNo.Id);
				if (order_ch is null)
				{
					_log.LogError($"Order id {runNo.Id} duplicate");
					return ResponseResult.Failure<GetOrderDto>($"Order id {runNo.Id} duplicate");
				}


				foreach (var item in newOrder)
				{
					var order_new = new Orders
					{
						OrderNoId = runNo.Id,
						ProductId = item.ProductId,
						ProductPrice = item.ProductPrice,
						Discount = item.ProductDiscount,
						CreatedBy = GetUserId(),
						CreatedDate = Now(),
						ItemCount = newOrder.Count(),
					};

					_dbContext.Orders.Add(order_new);
					await _dbContext.SaveChangesAsync();
				}

				var get_order_retuen = await _dbContext.Orders.Where(x => x.OrderNoId == runNo.Id).FirstOrDefaultAsync();
				// var idorder = _dbContext.OrderNos.Where(x=>x.Id==runNo.Id).ToListAsync();

				var order_retuen = _mapper.Map<GetOrderDto>(get_order_retuen);
				_log.LogInformation($"Add Order Success");
				return ResponseResult.Success<GetOrderDto>(order_retuen, "Success");
			}
			catch (Exception ex)
			{

				_log.LogError(ex.Message);
				return ResponseResult.Failure<GetOrderDto>(ex.Message);
			}
		}

		public async Task<ServiceResponse<List<ProductGroup>>> GetProducGrouptFilter(ProductGroupFilterDto ProductGroupFilter)
		{
			var productgroup_queryable = _dbContext.ProductGroups.AsQueryable();

			//Filter
			if (!string.IsNullOrWhiteSpace(ProductGroupFilter.ProductGroupName))
			{
				productgroup_queryable = productgroup_queryable.Where(x => x.Name.Contains(ProductGroupFilter.ProductGroupName));
			}

			// if (!string.IsNullOrWhiteSpace(EmployeeFilter.EmployeeDepartment))
			// {
			//     queryable = queryable.Where(x => x.Department.Contains(EmployeeFilter.EmployeeDepartment));
			// }

			//Ordering
			if (!string.IsNullOrWhiteSpace(ProductGroupFilter.OrderingField))
			{
				try
				{
					_log.LogInformation($"Order by Success");
					productgroup_queryable = productgroup_queryable.OrderBy($"{ProductGroupFilter.OrderingField} {(ProductGroupFilter.AscendingOrder ? "ascending" : "descending")}");
				}
				catch
				{
					return ResponseResultWithPagination.Failure<List<ProductGroup>>($"Could not order by field: {ProductGroupFilter.OrderingField}");
				}
			}

			var paginationResult = await _httpContext.HttpContext
				.InsertPaginationParametersInResponse(productgroup_queryable, ProductGroupFilter.RecordsPerPage, ProductGroupFilter.Page);

			var ProductGroupFilter_return = await productgroup_queryable.Paginate(ProductGroupFilter).ToListAsync();
			_log.LogInformation($"ProducGrouptFilter Success");

			return ResponseResultWithPagination.Success(ProductGroupFilter_return, paginationResult);
		}

		public async Task<ServiceResponse<GetProductDto>> DeleteProduct(int deleteProductId)
		{
			try
			{
				var product = await _dbContext.Products
					.Include(x => x.ProductGroup)
					.FirstOrDefaultAsync(x => x.Id == deleteProductId);
				//check Product
				if (product is null)
				{
					return ResponseResult.Failure<GetProductDto>($"Product id {deleteProductId} not found");
				}

				//mapper Dto and return
				var product_return = _mapper.Map<GetProductDto>(product);

				//remove database
				_dbContext.Products.RemoveRange(product);
				await _dbContext.SaveChangesAsync();

				_log.LogInformation("Delete product done.");
				return ResponseResult.Success(product_return, "success");
			}
			catch (Exception ex)
			{

				_log.LogError(ex.Message);
				return ResponseResult.Failure<GetProductDto>(ex.Message);
			}
		}

		public async Task<ServiceResponse<GetProductGroupDto>> DeleteProductGroup(int ProductGroupId)
		{
			try
			{
				//caheck department
				var productgroup = await _dbContext.ProductGroups.FirstOrDefaultAsync(x => x.Id == ProductGroupId);
				if (productgroup is null)
				{
					return ResponseResult.Failure<GetProductGroupDto>($"ProductGroup id {ProductGroupId} not found");
				}

				//check department is use?
				var productgroup_isuse = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductGroupId == ProductGroupId);
				if (!(productgroup_isuse is null))
				{
					var productgroup_Active = _mapper.Map<GetProductGroupDto>(productgroup);
					return ResponseResult.Failure<GetProductGroupDto>($"ProductGroup name {productgroup_Active.Name} product is Use");
				}

				//mapper Dto and return
				var department_return = _mapper.Map<GetProductGroupDto>(productgroup);

				//remove database
				_dbContext.ProductGroups.RemoveRange(productgroup);
				await _dbContext.SaveChangesAsync();

				_log.LogInformation($"Delete ProductGroup id {ProductGroupId} done.");
				return ResponseResult.Success(department_return, "success");
			}
			catch (Exception ex)
			{

				_log.LogError(ex.Message);
				return ResponseResult.Failure<GetProductGroupDto>(ex.Message);
			}
		}

		public async Task<ServiceResponse<GetProductGroupDto>> EditProductGroup(int editProductGroupId, EditProductGroupDto editProductGroup)
		{
			try
			{
				//check productgroup
				var productgroup = await _dbContext.ProductGroups
				.FirstOrDefaultAsync(x => x.Id == editProductGroupId);
				if (productgroup is null)
				{
					return ResponseResult.Failure<GetProductGroupDto>($"ProductGroup id {editProductGroupId} not found");
				}

				//assign value
				productgroup.Name = editProductGroup.ProductGroupName;
				productgroup.Status = editProductGroup.ProductGroupStatus;

				//update database
				_dbContext.ProductGroups.Update(productgroup);
				await _dbContext.SaveChangesAsync();

				//mapper Dto and return
				var dto = _mapper.Map<GetProductGroupDto>(productgroup);
				_log.LogInformation($"Edit productgroup Success");
				return ResponseResult.Success(dto, "Success");
			}
			catch (Exception ex)
			{

				_log.LogError(ex.Message);
				return ResponseResult.Failure<GetProductGroupDto>(ex.Message);
			}
		}

		public async Task<ServiceResponse<GetProductGroupDto>> GetProductGroupById(int ProductGroupId)
		{
			try
			{
				var productgroup = await _dbContext.ProductGroups.Include(x => x.Products)
					.FirstOrDefaultAsync(x => x.Id == ProductGroupId);

				//check Product Group
				if (productgroup is null)
				{
					_log.LogError($"ProductGroup id {ProductGroupId} not found");
					return ResponseResult.Failure<GetProductGroupDto>($"ProductGroup id {ProductGroupId} not found");
				}

				//mapper Dto and return
				var dto = _mapper.Map<GetProductGroupDto>(productgroup);

				_log.LogInformation("Get ProductGroup Success");
				return ResponseResult.Success(dto, "Success");
			}
			catch (Exception ex)
			{

				_log.LogError(ex.Message);
				return ResponseResult.Failure<GetProductGroupDto>(ex.Message);
			}
		}
	}
}