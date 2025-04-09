using Moq;
using MvvmDemo.Core;

namespace MvvmDemo.UnitTests;

public class MainPageViewModelTests
{
   private readonly Mock<IQuoteService> _quoteServiceMock;

   public MainPageViewModelTests()
   {
      _quoteServiceMock = new Mock<IQuoteService>();
      _quoteServiceMock.Setup(m => m.GetQuote()).ReturnsAsync(string.Empty);
   }

   [Fact]
   public void ButtonShouldBeVisible()
   {
      var sut = new MainPageViewModel(_quoteServiceMock.Object);
      Assert.True(sut.IsButtonVisible);
   }

   [Fact]
   public void LabelShouldNotBeVisible()
   {
      var sut = new MainPageViewModel(_quoteServiceMock.Object);
      Assert.False(sut.IsLabelVisible);
   }

   [Fact]
   public void GetQuoteCommand_ShoudCallGetQuote()
   {
      var sut = new MainPageViewModel(_quoteServiceMock.Object);
      sut.GetQuoteCommand.Execute(null);
      _quoteServiceMock.Verify(m => m.GetQuote(), Times.Once());
   }

   [Fact]
   public void GetQuoteCommand_ShoudSetButtonInvisible()
   {
      var sut = new MainPageViewModel(_quoteServiceMock.Object);
      sut.GetQuoteCommand.Execute(null);
      Assert.False(sut.IsButtonVisible);
   }

   [Fact]
   public void GetQuoteCommand_GotQuote_ShowQuote()
   {
      var quote = "My quote of the day";
      _quoteServiceMock.Setup(m => m.GetQuote()).ReturnsAsync(quote);
      var sut = new MainPageViewModel(_quoteServiceMock.Object);
      sut.GetQuoteCommand.Execute(null);
      Assert.True(sut.IsLabelVisible);
      Assert.Equal(quote, sut.QuoteOfTheDay);
   }

   [Fact]
   public void GetQuoteCommand_ServiceThrows_ShouldShowButton()
   {
      _quoteServiceMock.Setup(m => m.GetQuote()).ThrowsAsync(new Exception());
      var sut = new MainPageViewModel(_quoteServiceMock.Object);
      sut.GetQuoteCommand.Execute(null);
      Assert.True(sut.IsButtonVisible);
   }
}