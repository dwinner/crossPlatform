using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockTake.App.Common.Interfaces;
using StockTake.Shared.Inventory;

namespace StockTake.App.Inventory.Queries;

public class GetInventoryQueryHandler : IRequestHandler<GetInventoryQuery, InventoryVm>
{
   private readonly IApplicationDbContext _context;
   private readonly IIdentityService _identityService;
   private readonly IMapper _mapper;

   public GetInventoryQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
   {
      _context = context;
      _identityService = identityService;
      _mapper = mapper;
   }

   public async Task<InventoryVm> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
   {
      var inventory = new List<InventoryItemDto>();

      var products = await _context.Products
         .Include(p => p.Manufacturer)
         .Include(p => p.StockCounts)
         .ToListAsync(cancellationToken);

      foreach (var product in products)
      {
         var item = new InventoryItemDto
         {
            CountedAt = product.StockCounts.Any()
               ? product.StockCounts.OrderByDescending(s => s.CountedAt).FirstOrDefault().CountedAt
               : DateTime.MinValue,

            Count = product.StockCounts.Any()
               ? product.StockCounts.OrderByDescending(s => s.CountedAt).FirstOrDefault().Count
               : 0,

            CountedById = product.StockCounts.Any()
               ? product.StockCounts.OrderByDescending(s => s.CountedAt).FirstOrDefault().CountedById
               : "No stock counted",

            CountedByName = product.StockCounts.Any()
               ? await _identityService.GetUserNameAsync(product.StockCounts.OrderByDescending(s => s.CountedAt)
                  .FirstOrDefault().CountedById)
               : "No stock counted",

            ManufacturerName = product.Manufacturer.Name,

            ProductId = product.Id,

            ProductName = product.Name
         };

         inventory.Add(item);
      }

      return new InventoryVm { Inventory = inventory, InventoryAt = DateTime.Now };
   }
}