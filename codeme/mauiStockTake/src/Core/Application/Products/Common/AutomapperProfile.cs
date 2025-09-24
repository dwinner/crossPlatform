using AutoMapper;
using MauiStockTake.Domain.Entities;
using MauiStockTake.Shared.Products;

namespace MauiStockTake.Application.Products.Common;
public class AutomapperProfile : Profile
{
	public AutomapperProfile()
	{
		CreateMap<Product, ProductDto>();
	}
}
