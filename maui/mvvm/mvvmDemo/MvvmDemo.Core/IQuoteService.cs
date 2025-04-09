namespace MvvmDemo.Core;

public interface IQuoteService
{
   Task<string> GetQuote();
}