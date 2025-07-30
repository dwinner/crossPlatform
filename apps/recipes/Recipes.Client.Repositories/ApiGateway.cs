using Recipes.Client.Core;
using Refit;

namespace Recipes.Client.Repositories;

public abstract class ApiGateway
{
   protected Task<Result<TType>> InvokeAndMap<TType>(Task<ApiResponse<TType>> call)
      where TType : notnull
      => InvokeAndMap(call, type => type);

   protected async Task<Result<TResult>> InvokeAndMap<TResult, TDtoResult>(
      Task<ApiResponse<TDtoResult>> call,
      Func<TDtoResult, TResult> mapper)
      where TResult : notnull
   {
      try
      {
         var response = await call;
         if (response.IsSuccessStatusCode)
         {
            return Result<TResult>
               .Success(mapper(response.Content));
         }

         return Result<TResult>
            .Fail("FAILED_REQUEST", response.Error.StatusCode.ToString());
      }
      catch (ApiException apiEx)
      {
         return Result<TResult>
            .Fail("ApiException", apiEx.StatusCode.ToString(), apiEx);
      }
      catch (Exception ex)
      {
         return Result<TResult>.Fail(ex);
      }
   }
}