using AutoBogus;
using Moq;
using Recipes.Client.Core.Features.Favorites;
using Recipes.Client.Core.Features.Ratings;
using Recipes.Client.Core.Features.Recipes;
using Recipes.Client.Core.Navigation;
using Recipes.Client.Core.Services;
using Recipes.Client.Core.ViewModels;
// APPLY: Not all tests are written for important things
namespace Recipes.Client.Core.UnitTests;

public class RecipeDetailViewModelTests
{
   private readonly Mock<IDialogService> _dialogServiceMock;
   private readonly Mock<INavigationService> _navigationServiceMock;
   private readonly Mock<IRecipeService> _recipeServiceMock;
   private readonly RecipeDetailViewModel _sut;

   public RecipeDetailViewModelTests()
   {
      _recipeServiceMock = new Mock<IRecipeService>();
      Mock<IFavoritesService> favoritesServiceMock = new();
      Mock<IRatingsService> ratingsServiceMock = new();
      _navigationServiceMock = new Mock<INavigationService>();
      _dialogServiceMock = new Mock<IDialogService>();

      ratingsServiceMock
         .Setup(ratingsSvc => ratingsSvc.LoadRatingsSummary(It.IsAny<string>()))
         .ReturnsAsync(Result<RatingsSummary>.Success(AutoFaker.Generate<RatingsSummary>()));

      _sut = new RecipeDetailViewModel(
         _recipeServiceMock.Object,
         favoritesServiceMock.Object,
         ratingsServiceMock.Object,
         _navigationServiceMock.Object,
         _dialogServiceMock.Object);
   }

   [Fact]
   public async Task OnNavigatedTo_Should_Load_Recipe()
   {
      // Arrange
      var recipeId = AutoFaker.Generate<string>();
      var parameters = new Dictionary<string, object>
      {
         { "id", recipeId }
      };

      _recipeServiceMock
         .Setup(recipeSvc => recipeSvc.LoadRecipe(It.IsAny<string>()))
         .ReturnsAsync(Result<RecipeDetail>.Success(AutoFaker.Generate<RecipeDetail>()));

      // Act
      await _sut.OnNavigatedTo(parameters);

      // Assert
      _recipeServiceMock.Verify(recipeSvc => recipeSvc.LoadRecipe(recipeId), Times.Once);
   }

   [Fact]
   public async Task OnNavigatedTo_Should_Map_RecipeDetail()
   {
      // Arrange
      var recipeDetail = AutoFaker.Generate<RecipeDetail>();
      var parameters = new Dictionary<string, object>
      {
         { "id", AutoFaker.Generate<string>() }
      };

      _recipeServiceMock
         .Setup(recipeSvc => recipeSvc.LoadRecipe(It.IsAny<string>()))
         .ReturnsAsync(Result<RecipeDetail>.Success(recipeDetail));

      // Act
      await _sut.OnNavigatedTo(parameters);

      // Assert
      Assert.Equal(recipeDetail.Name, _sut.Title);
      Assert.Equal(recipeDetail.Author, _sut.Author);
   }

   [Fact]
   public async Task FailedLoad_Should_ShowDialog()
   {
      // Arrange
      var parameters = new Dictionary<string, object>
      {
         { "id", AutoFaker.Generate<string>() }
      };

      _recipeServiceMock
         .Setup(recipeSvc => recipeSvc.LoadRecipe(It.IsAny<string>()))
         .ReturnsAsync(Result<RecipeDetail>.Fail(AutoFaker.Generate<string>()));

      _dialogServiceMock
         .Setup(dialogSvc =>
            dialogSvc.AskYesNo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
         .ReturnsAsync(false);

      // Act
      await _sut.OnNavigatedTo(parameters);

      // Assert
      _dialogServiceMock.Verify(
         dialogSvc =>
            dialogSvc.AskYesNo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
         Times.Once);
      _navigationServiceMock.Verify(navSvc => navSvc.GoBack(), Times.Once);
   }
}