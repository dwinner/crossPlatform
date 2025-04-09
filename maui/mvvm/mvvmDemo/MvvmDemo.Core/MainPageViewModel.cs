using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MvvmDemo.Core;

public class MainPageViewModel : ViewModelBase
{
   private readonly IQuoteService _quoteService;
   private bool _isButtonVisible;
   private bool _isLabelVisible;
   private string _quoteOfTheDay;

   public MainPageViewModel(IQuoteService quoteService)
   {
      _quoteService = quoteService;
      _quoteOfTheDay = string.Empty;
      _isButtonVisible = true;
      _isLabelVisible = true;
   }

   public string QuoteOfTheDay
   {
      get => _quoteOfTheDay;
      set => SetField(ref _quoteOfTheDay, value);
   }

   public bool IsButtonVisible
   {
      get => _isButtonVisible;
      set => SetField(ref _isButtonVisible, value);
   }

   public bool IsLabelVisible
   {
      get => _isLabelVisible;
      set => SetField(ref _isLabelVisible, value);
   }

   public ICommand GetQuoteCommand => new Command(async _ => await GetQuote());

   private async Task GetQuote()
   {
      IsButtonVisible = false;

      try
      {
         var quote = await _quoteService.GetQuote();
         QuoteOfTheDay = quote;
         IsLabelVisible = true;
      }
      catch (Exception)
      {
         // ignored
      }
      finally
      {
         IsButtonVisible = true;
      }
   }
}