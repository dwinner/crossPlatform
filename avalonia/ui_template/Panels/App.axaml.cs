using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Panels.ViewModels;
using Panels.Views;

namespace Panels;

public class App : Application
{
   public override void Initialize()
   {
      AvaloniaXamlLoader.Load(this);
   }

   public override void OnFrameworkInitializationCompleted()
   {
      switch (ApplicationLifetime)
      {
         case IClassicDesktopStyleApplicationLifetime desktop:
            desktop.MainWindow = new MainWindow
            {
               DataContext = new MainViewModel()
            };
            break;

         case ISingleViewApplicationLifetime singleViewPlatform:
            singleViewPlatform.MainView = new MainView
            {
               DataContext = new MainViewModel()
            };
            break;
      }

      base.OnFrameworkInitializationCompleted();
   }
}