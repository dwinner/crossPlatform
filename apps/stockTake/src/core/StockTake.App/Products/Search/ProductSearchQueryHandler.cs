using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockTake.App.Common.Interfaces;
using StockTake.Shared.Products;

namespace StockTake.App.Products.Search;

public class ProductSearchQueryHandler : IRequestHandler<ProductSeachQuery, List<ProductDto>>
{
   private readonly IApplicationDbContext _context;
   private readonly IMapper _mapper;

   public ProductSearchQueryHandler(IApplicationDbContext context, IMapper mapper)
   {
      _context = context;
      _mapper = mapper;
   }

   public async Task<List<ProductDto>> Handle(ProductSeachQuery request, CancellationToken cancellationToken)
   {
      var results = await _context.Products
         .Where(p => p.Name.Contains(request.SearchTerm, StringComparison.CurrentCultureIgnoreCase))
         .AsNoTracking()
         .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
         .ToListAsync(cancellationToken);

      return results;
   }
}