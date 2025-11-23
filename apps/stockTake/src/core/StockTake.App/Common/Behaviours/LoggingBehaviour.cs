using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using StockTake.App.Common.Interfaces;

namespace StockTake.App.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
   private readonly ICurrentUserService _currentUserService;
   private readonly IIdentityService _identityService;
   private readonly ILogger _logger;

   public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService,
      IIdentityService identityService)
   {
      _logger = logger;
      _currentUserService = currentUserService;
      _identityService = identityService;
   }

   public async Task Process(TRequest request, CancellationToken cancellationToken)
   {
      var requestName = typeof(TRequest).Name;
      var userId = _currentUserService.UserId ?? string.Empty;
      var userName = string.Empty;

      if (!string.IsNullOrEmpty(userId))
      {
         userName = await _identityService.GetUserNameAsync(userId);
      }

      _logger.LogInformation("MauiStockTake Request: {Name} {@UserId} {@UserName} {@Request}",
         requestName, userId, userName, request);
   }
}