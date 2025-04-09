namespace Recipes.Shared;

public record RecipeOverviewItemsDto(int TotalItems, int PageSize, int PageIndex, RecipeOverviewItemDto[] Recipes);