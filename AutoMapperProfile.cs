using AutoMapper;
using NetCoreAPI_Template_v3_with_auth.DTOs;
using NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop;
using NetCoreAPI_Template_v3_with_auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAPI_Template_v3_with_auth
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<User, UserDto>();
			CreateMap<Role, RoleDto>().ForMember(x => x.RoleName, x => x.MapFrom(x => x.Name));
			CreateMap<RoleDtoAdd, Role>()
				.ForMember(x => x.Name, x => x.MapFrom(x => x.RoleName));
			CreateMap<UserRole, UserRoleDto>();
			CreateMap<Product, GetProductDto>();
			CreateMap<Product, GetStockDto>();
			CreateMap<Product, FilterProductNameDto>();
			CreateMap<ProductGroup, GetProductGroupDto>();
			CreateMap<ProductGroup, FilterProductGroupNameDto>();
			CreateMap<Store, GetStockDto>();
			CreateMap<Orders, GetOrderDto>();
			CreateMap<Orders, OrdersDto>();
			CreateMap<OrdersDto, GetOrderDto>();
			CreateMap<GetProductDetail, GetOrderDto>();
			CreateMap<OrderNo, GetOrderDto>().ForMember(x => x.Orders, x => x.MapFrom(x => x.Orders));
			CreateMap<Product, GetProductDetail>();
			CreateMap<Product, GetOrderDto>();

			CreateMap<OrderNo, GetOrderFilterDto>();

		}
	}
}