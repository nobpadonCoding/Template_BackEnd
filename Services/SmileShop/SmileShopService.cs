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
				var product = await _dbContext.Products
					.Include(x => x.CreatedBy)
					.FirstOrDefaultAsync(x => x.Name == newProduct.ProductName);
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
					CreatedById = Guid.Parse(GetUserId()),
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
				var productGroup = await _dbContext.ProductGroups
					.Include(x => x.CreatedBy)
					.FirstOrDefaultAsync(x => x.Name == newProductGroup.ProductGroupName);
				if (!(productGroup is null))
				{
					return ResponseResult.Failure<GetProductGroupDto>("ProductGroup duplicate!");
				}

				var productGroup_new = new ProductGroup
				{
					Name = newProductGroup.ProductGroupName,
					CreatedById = Guid.Parse(GetUserId()),
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
				.Include(x => x.ProductGroup)
				.Include(x => x.CreatedBy).AsQueryable();

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
				var productGroups = await _dbContext.ProductGroups
					.Include(x => x.CreatedBy)
					.Include(x => x.Products)
					.ToListAsync();

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
					.Include(x => x.ProductGroup)
					.Include(x => x.CreatedBy)
					.ToListAsync();

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
					.Include(x => x.CreatedBy)
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

		public async Task<ServiceResponse<GetOrderDto>> GetOrder(int orderNumber)
		{
			try
			{

				var order = await _dbContext.OrderNo
				.Include(x => x.Orders).ThenInclude(x => x.Product)
				.Include(x => x.CreatedBy)
				.Where(x => x.Id == orderNumber).SingleOrDefaultAsync();

				//check Order
				if (order is null)
				{
					_log.LogError($"Order number {orderNumber} not found");
					return ResponseResult.Failure<GetOrderDto>($"Order number {orderNumber} not found");
				}

				//mapper Dto and return
				var dto = _mapper.Map<GetOrderDto>(order);

				_log.LogInformation("Get Order Success");
				return ResponseResult.Success(dto, "Success");
			}
			catch (Exception ex)
			{

				_log.LogError(ex.Message);
				return ResponseResult.Failure<GetOrderDto>(ex.Message);
			}
		}

		public async Task<ServiceResponse<GetOrderNoDto>> AddOrder(AddOrderDto newOrder)
		{
			try
			{
				var sumProductQuantity = 0;
				//loob check product
				foreach (var item in newOrder.OrderDetail)
				{
					var product = _dbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
					//sum total
					sumProductQuantity = sumProductQuantity + item.ProductQuantity;

					//check product มีหรือเปล่า
					if (!(product is null))
					{
						//check QTY product ต้อง > 0
						if (product.Result.StockCount <= 0)
						{
							_log.LogError($"{product.Result.Name} StockCount < 0");
							return ResponseResult.Failure<GetOrderNoDto>($"{product.Result.Name} StockCount < 0");
						}
						else
						{
							//check StockCount ต้องมากกว่า Quantity
							if (product.Result.StockCount >= item.ProductQuantity)
							{
								var xxx = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
								//ตัด stock
								var StockCount = product.Result.StockCount - item.ProductQuantity;
								xxx.Name = product.Result.Name;
								xxx.Price = product.Result.Price;
								xxx.StockCount = StockCount;
								xxx.ProductGroupId = xxx.ProductGroupId;
								_dbContext.Products.Update(xxx);
							}
							else
							{
								_log.LogError($"{product.Result.Name} StockCount < Quantity");
								return ResponseResult.Failure<GetOrderNoDto>($"{product.Result.Name} StockCount < {item.ProductQuantity}");
							}
						}
					}
					else
					{
						_log.LogError($"{item.ProductId} Product Not found");
						return ResponseResult.Failure<GetOrderNoDto>($"{item.ProductId} Product Not found");
					}
				}

				//create Order id
				var runNo = new OrderNo
				{
					CreatedById = Guid.Parse(GetUserId()),
					CreatedDate = Now(),
					Discount = newOrder.Discount,
					Total = newOrder.Total,
					TotalAmount = newOrder.TotalAmount,
					ProductQuantity = sumProductQuantity
				};

				_dbContext.OrderNo.Add(runNo);
				await _dbContext.SaveChangesAsync();

				//check order duplicate
				// var order_ch = _dbContext.Orders.FirstOrDefaultAsync(x => x.OrderNoId == runNo.Id);
				// if (order_ch is null)
				// {
				// 	_log.LogError($"Order id {runNo.Id} duplicate");
				// 	return ResponseResult.Failure<GetOrderDto>($"Order id {runNo.Id} duplicate");
				// }


				foreach (var item in newOrder.OrderDetail)
				{
					var orders = new Orders
					{
						OrderNoId = runNo.Id,
						ProductId = item.ProductId,
						ProductPrice = item.ProductPrice,

						CreatedById = Guid.Parse(GetUserId()),
						Quantity = item.ProductQuantity,
						CreatedDate = Now()
					};

					_dbContext.Orders.AddRange(orders);

				}

				await _dbContext.SaveChangesAsync();

				var get_order_return = await _dbContext.Orders.Where(x => x.OrderNoId == runNo.Id).FirstOrDefaultAsync();


				var order_return = _mapper.Map<GetOrderNoDto>(get_order_return);
				_log.LogInformation($"Add Order Success");
				return ResponseResult.Success<GetOrderNoDto>(order_return, "Success");
			}
			catch (Exception ex)
			{

				_log.LogError(ex.Message);
				return ResponseResult.Failure<GetOrderNoDto>(ex.Message);
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
				var productgroup = await _dbContext.ProductGroups
					.Include(x => x.Products)
					.Include(x => x.CreatedBy)
					.FirstOrDefaultAsync(x => x.Id == ProductGroupId);

				//check Product Group
				if (productgroup is null)
				{
					_log.LogError($"ProductGroup id {ProductGroupId} not found");
					return ResponseResult.Failure<GetProductGroupDto>($"ProductGroup id {ProductGroupId} not found");
				}

				//mapper Dto and return
				var dto = _mapper.Map<GetProductGroupDto>(productgroup);
				var createdByName = _dbContext.Users.FirstOrDefaultAsync(x => x.Id == productgroup.CreatedById);


				_log.LogInformation("Get ProductGroup Success");
				return ResponseResult.Success(dto, "Success");
			}
			catch (Exception ex)
			{

				_log.LogError(ex.Message);
				return ResponseResult.Failure<GetProductGroupDto>(ex.Message);
			}
		}

		public Task<ServiceResponse<GetStockDto>> GetStock()
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResponse<GetStockDto>> AddStock(AddstockDto newStock)
		{
			try
			{
				var product_onhand = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == newStock.ProductId);

				// cut stock
				if (newStock.StoreTypeId == "1")
				{
					product_onhand.StockCount = product_onhand.StockCount + newStock.Quantity;
				}
				else
				{
					if (product_onhand.StockCount < newStock.Quantity)//check on hand
					{
						_log.LogError($"Product onHand < {newStock.Quantity} not found");
						return ResponseResult.Failure<GetStockDto>($"Product onHand < {newStock.Quantity}");
					}
					product_onhand.StockCount = product_onhand.StockCount - newStock.Quantity;
				}

				//update database
				_dbContext.Products.Update(product_onhand);
				_log.LogInformation($"cut stock Success");

				var stock = new Store
				{
					ProductId = newStock.ProductId,
					Qty = newStock.Quantity,
					ProductStockCount = newStock.ProductStockCount,
					StockAfter = newStock.StockAfter,
					CreatedById = Guid.Parse(GetUserId()),
					CreatedDate = Now(),
					StoreType = newStock.StoreTypeId,
					Remark = newStock.Remark
				};

				_dbContext.Stores.Add(stock);
				await _dbContext.SaveChangesAsync();

				var stock_retuen = _mapper.Map<GetStockDto>(stock);

				_log.LogInformation($"Add Stores Success");
				return ResponseResult.Success(stock_retuen, "Success");
			}
			catch (Exception ex)
			{

				_log.LogError(ex.Message);
				return ResponseResult.Failure<GetStockDto>(ex.Message);
			}
		}

		public async Task<ServiceResponse<List<GetStockDto>>> GetStoreFilter(StoreFilterDto StoreFilter)
		{
			var store_queryable = _dbContext.Stores
				.Include(x => x.CreatedBy)
				.Include(x => x.Product).ThenInclude(x => x.ProductGroup)
				.AsQueryable();

			//Filter
			// if (!string.IsNullOrWhiteSpace(StoreFilter.ProductName))
			// {
			// 	store_queryable = store_queryable.Where(x => x.Product.Name.Contains(StoreFilter.ProductName));
			// }

			if (!string.IsNullOrWhiteSpace(StoreFilter.StoreType))
			{
				store_queryable = store_queryable.Where(x => x.StoreType.Contains(StoreFilter.StoreType));
			}

			// if (!string.IsNullOrWhiteSpace(EmployeeFilter.EmployeeDepartment))
			// {
			//     queryable = queryable.Where(x => x.Department.Contains(EmployeeFilter.EmployeeDepartment));
			// }

			//Ordering
			if (!string.IsNullOrWhiteSpace(StoreFilter.OrderingField))
			{
				try
				{
					_log.LogInformation($"Order by Success");
					store_queryable = store_queryable.OrderBy($"{StoreFilter.OrderingField} {(StoreFilter.AscendingOrder ? "ascending" : "descending")}");
				}
				catch
				{
					return ResponseResultWithPagination.Failure<List<GetStockDto>>($"Could not order by field: {StoreFilter.OrderingField}");
				}
			}

			var paginationResult = await _httpContext.HttpContext
				.InsertPaginationParametersInResponse(store_queryable, StoreFilter.RecordsPerPage, StoreFilter.Page);



			var StoreFilter_Paginate = await store_queryable.Paginate(StoreFilter).ToListAsync();
			_log.LogInformation($"Store Filter Success");

			var StoreFilter_return = _mapper.Map<List<GetStockDto>>(StoreFilter_Paginate);

			return ResponseResultWithPagination.Success(StoreFilter_return, paginationResult);
		}

		public async Task<ServiceResponse<List<GetOrderFilterDto>>> GetOrderFilter(OrderFilterDto OrderFilter)
		{
			var order_queryable = _dbContext.OrderNo
				.Include(x => x.CreatedBy).AsQueryable();

			if (!string.IsNullOrWhiteSpace(OrderFilter.OrderNumber))
			{
				order_queryable = order_queryable.Where(x => x.Id.ToString().Contains(OrderFilter.OrderNumber));
			}


			if (OrderFilter.EndDate.HasValue && OrderFilter.StartDate.HasValue)
			{
				order_queryable = order_queryable.Where(x => x.CreatedDate.Date >= OrderFilter.StartDate.Value.Date && x.CreatedDate.Date < OrderFilter.EndDate.Value.Date.AddDays(+1));
			}

			//Ordering
			if (!string.IsNullOrWhiteSpace(OrderFilter.OrderingField))
			{
				try
				{
					order_queryable = order_queryable.OrderBy($"{OrderFilter.OrderingField} {(OrderFilter.AscendingOrder ? "ascending" : "descending")}");
					_log.LogInformation($"Order by Success");
				}
				catch
				{
					return ResponseResultWithPagination.Failure<List<GetOrderFilterDto>>($"Could not order by field: {OrderFilter.OrderingField}");
				}
			}

			var paginationResult = await _httpContext.HttpContext
				.InsertPaginationParametersInResponse(order_queryable, OrderFilter.RecordsPerPage, OrderFilter.Page);



			var OrderFilter_Paginate = await order_queryable.Paginate(OrderFilter).ToListAsync();
			_log.LogInformation($"Order Filter Success");

			var OrderFilter_return = _mapper.Map<List<GetOrderFilterDto>>(OrderFilter_Paginate);

			return ResponseResultWithPagination.Success(OrderFilter_return, paginationResult);

		}
	}
}