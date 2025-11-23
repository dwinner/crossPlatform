using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockTake.App.Common.Exceptions;
using StockTake.App.Common.Interfaces;
using StockTake.Shared.Products;

namespace StockTake.App.Products.GetProduct;

public class GetProductByBarcodeQueryHandler : IRequestHandler<GetProductByBarcodeQuery, ProductDto>
{
   private readonly IApplicationDbContext _context;
   private readonly IMapper _mapper;

   public GetProductByBarcodeQueryHandler(IApplicationDbContext context, IMapper mapper)
   {
      _context = context;
      _mapper = mapper;
   }

   public async Task<ProductDto> Handle(GetProductByBarcodeQuery request, CancellationToken cancellationToken)
   {
      var product = await _context.Products
         .Where(p => p.BarCode == request.BarCode)
         .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
         .AsNoTracking()
         .FirstOrDefaultAsync(cancellationToken);

      if (product is null)
      {
         throw new NotFoundException($"Product with barcode {request.BarCode} not found");
      }

      return product;
   }
}