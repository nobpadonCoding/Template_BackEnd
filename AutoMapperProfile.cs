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
			CreateMap<Orders, GetOrderDto>()
				.ForMember(x => x.Total, x => x.MapFrom(x => x.OrderNo.Total))
				.ForMember(x => x.TotalAmount, x => x.MapFrom(x => x.OrderNo.TotalAmount))
				.ForMember(x => x.Discount, x => x.MapFrom(x => x.OrderNo.Discount));
				// .ForMember(x => x.OrderDetail, x => x.MapFrom(x => x.Product));
			CreateMap<Product, GetOrderDto>();
			CreateMap<Product, GetOrderProductDetailDto>();
			CreateMap<OrderNo, GetOrderFilterDto>();
		}
	}
}