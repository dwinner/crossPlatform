using Microsoft.Maui.Handlers;
using WidgetBoard.App.ViewModels;
#if IOS || MACCATALYST
using ObjCRuntime;
#endif

namespace WidgetBoard.App.Pages;

public partial class BoardDetailsPage
{
   public BoardDetailsPage(BoardDetailsPageViewModel viewModel)
   {
      InitializeComponent();
      BindingContext = viewModel;

      EntryHandler.Mapper.AppendToMapping("SelectAllText", (handler, _) =>
      {
#if ANDROID
         handler.PlatformView.SetSelectAllOnFocus(true);
#elif IOS || MACCATALYST
         handler.PlatformView.EditingDidBegin += (s, e) =>
         {
            handler.PlatformView.PerformSelector(new Selector("selectAll"), null, 0.0f);
         };
#elif WINDOWS
         handler.PlatformView.GotFocus += (_, _) => { handler.PlatformView.SelectAll(); };
#endif
      });
   }
}