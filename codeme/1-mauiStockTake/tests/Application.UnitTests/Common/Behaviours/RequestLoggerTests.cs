using MauiStockTake.Application.Common.Behaviours;
using MauiStockTake.Application.Common.Interfaces;
using MauiStockTake.Application.Inventory.Commands;
using MauiStockTake.Shared.StockCounts;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace MauiStockTake.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<ILogger<AddStockCountCommand>> _logger = null!;
    private Mock<ICurrentUserService> _currentUserService = null!;
    private Mock<IIdentityService> _identityService = null!;

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

        var requestLogger = new LoggingBehaviour<AddStockCountCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

        await requestLogger.Process(new AddStockCountCommand { Count = new StockCountDto { ProductId = 1, ProductCount = 1 } }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
    {
        var requestLogger = new LoggingBehaviour<AddStockCountCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

        await requestLogger.Process(new AddStockCountCommand { Count = new StockCountDto { ProductId = 1, ProductCount = 1 } }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
    }
}
