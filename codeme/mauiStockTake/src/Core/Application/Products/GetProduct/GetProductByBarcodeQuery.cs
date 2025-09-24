using AutoMapper;
using AutoMapper.QueryableExtensions;
using MauiStockTake.Application.Common.Exceptions;
using MauiStockTake.Application.Common.Interfaces;
using MauiStockTake.Shared.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MauiStockTake.Application.Products.GetProduct;

public class GetProductByBarcodeQuery : IRequest<ProductDto>
{
    public string BarCode { get; set; }
}

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
