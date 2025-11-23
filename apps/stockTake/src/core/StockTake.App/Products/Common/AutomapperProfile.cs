using AutoMapper;
using StockTake.Domain.Entities;
using StockTake.Shared.Products;

namespace StockTake.App.Products.Common;

public class AutomapperProfile : Profile
{
   public AutomapperProfile()
   {
      CreateMap<Product, ProductDto>();
   }
}