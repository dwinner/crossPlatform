using MvvmDemo.Core;

namespace MvvmDemo.Impl;

public class QuoteService : IQuoteService
{
   private readonly HttpClient _httpClient = new();

   public async Task<string> GetQuote()
   {
      var responseMessage = await _httpClient.GetAsync("https://my-quotes-api.com/quote-of-the-day");
      if (responseMessage.IsSuccessStatusCode)
      {
         return await responseMessage.Content.ReadAsStringAsync();
      }

      throw new Exception("Failed to retrieve quote.");
   }
}