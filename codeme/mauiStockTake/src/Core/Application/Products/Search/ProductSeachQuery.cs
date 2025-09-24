using AutoMapper;
using AutoMapper.QueryableExtensions;
using MauiStockTake.Application.Common.Interfaces;
using MauiStockTake.Shared.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MauiStockTake.Application.Products.Search;
public class ProductSeachQuery : IRequest<List<ProductDto>>
{
    public string SearchTerm { get; set; }
}

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
            .Where(p => p.Name.ToLower().Contains(request.SearchTerm.ToLower()))
            .AsNoTracking()
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return results;
    }
}
