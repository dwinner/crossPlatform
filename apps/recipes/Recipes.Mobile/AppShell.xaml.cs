using Recipes.Client.Core.Navigation;
using Recipes.Mobile.Navigation;
using static Microsoft.Maui.Controls.ShellNavigationSource;

namespace Recipes.Mobile;

public partial class AppShell
{
   private readonly INavigationInterceptor _interceptor;

   public AppShell(INavigationInterceptor interceptor)
   {
      _interceptor = interceptor;
      InitializeComponent();
   }

   protected override async void OnNavigating(ShellNavigatingEventArgs args)
   {
      base.OnNavigating(args);

      var token = args.GetDeferral();
      if (token == null)
      {
         return;
      }

      var bindingCtx = CurrentPage?.BindingContext;
      if (bindingCtx == null)
      {
         return;
      }

      var canNavigate = await _interceptor.CanNavigate(bindingCtx, GetNavigationType(args.Source));
      if (canNavigate)
      {
         token.Complete();
      }
      else
      {
         args.Cancel();
      }
   }

   protected override async void OnNavigated(ShellNavigatedEventArgs args)
   {
      base.OnNavigated(args);
      var navigationType = GetNavigationType(args.Source);
      var bindingCtx = CurrentPage?.BindingContext;
      if (bindingCtx != null)
      {
         await _interceptor.OnNavigatedTo(bindingCtx, navigationType);
      }
   }

   private static NavigationType GetNavigationType(ShellNavigationSource source) =>
      source switch
      {
         Push or Insert
            => NavigationType.Forward,
         Pop or PopToRoot or Remove
            => NavigationType.Back,
         ShellItemChanged or ShellSectionChanged or ShellContentChanged
            => NavigationType.SectionChange,
         _ => NavigationType.Unknown
      };
}