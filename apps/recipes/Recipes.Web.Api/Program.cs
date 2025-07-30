// APPLY: It's not enough to have data available online only,
// we have to specify local data storage also

using Microsoft.AspNetCore.Mvc;
using Recipes.Web.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

// Set up WEB API endpoints
var recipeService = new RecipeService();
var ratingsService = new RatingsService();

// APPLY: use language to retrieve recipes
app.MapGet(
      "/recipes",
      (int pageSize, int pageIndex, [FromHeader(Name = "Accept-Language")] string language) =>
         recipeService.LoadRecipes(pageSize, pageIndex))
   .WithName("GetRecipes")
   .WithOpenApi();

app.MapGet(
      "/recipe/{id}",
      (string id) => recipeService.LoadRecipe(id))
   .WithName("GetRecipe")
   .WithOpenApi();

app.MapGet(
      "/recipe/{id}/ratings",
      (string id) => ratingsService.LoadRatings(id))
   .WithName("GetRecipeRatings")
   .WithOpenApi();

app.MapGet(
      "/recipe/{id}/ratingssummary",
      (string id) => ratingsService.LoadRatingsSummary(id))
   .WithName("GetRecipeRatingsSummary")
   .WithOpenApi();

app.MapGet(
      "/users/{userId}/favorites",
      FavoritesDataStore.GetFavorites)
   .WithName("GetUserFavorites")
   .WithOpenApi();

app.MapPost(
      "/users/{userId}/favorites",
      FavoritesDataStore.StoreFavorite)
   .WithName("AddFavorite")
   .WithOpenApi();

app.MapDelete(
      "/users/{userId}/favorites/{recipeId}",
      FavoritesDataStore.DeleteFavorite)
   .WithName("DeleteFavorite")
   .WithOpenApi();

// Run web app
app.Run();