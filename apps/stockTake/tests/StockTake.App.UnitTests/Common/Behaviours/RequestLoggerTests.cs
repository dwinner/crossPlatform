using Microsoft.Extensions.Logging;
using Moq;
using StockTake.App.Common.Behaviours;
using StockTake.App.Common.Interfaces;
using StockTake.App.Inventory.Commands;
using StockTake.Shared.StockCounts;

namespace StockTake.App.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
   private Mock<ICurrentUserService> _currentUserService = null!;
   private Mock<IIdentityService> _identityService = null!;
   private Mock<ILogger<AddStockCountCommand>> _logger = null!;

   [SetUp]
   public void Setup()
   {
      _logger = new Mock<ILogger<AddStockCountCommand>>();
      _currentUserService = new Mock<ICurrentUserService>();
      _identityService = new Mock<IIdentityService>();
   }

   [Test]
   public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
   {
      _currentUserService.Setup(x => x.UserId).Returns(Guid.NewGuid().ToString());

      LoggingBehaviour<AddStockCountCommand> requestLogger =
         new(_logger.Object, _currentUserService.Object, _identityService.Object);

      await requestLogger.Process(
         new AddStockCountCommand { Count = new StockCountDto { ProductId = 1, ProductCount = 1 } },
         CancellationToken.None);

      _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
   }

   [Test]
   public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
   {
      LoggingBehaviour<AddStockCountCommand> requestLogger =
         new(_logger.Object, _currentUserService.Object, _identityService.Object);

      await requestLogger.Process(
         new AddStockCountCommand { Count = new StockCountDto { ProductId = 1, ProductCount = 1 } },
         CancellationToken.None);

      _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
   }
}