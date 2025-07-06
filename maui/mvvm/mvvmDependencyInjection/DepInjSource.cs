namespace c2_DecoupleViewAndViewModel;

public class DepInjSource : IMarkupExtension
{
   public Type Type { get; set; }

   public object ProvideValue(IServiceProvider serviceProvider)
   {
      var mainPage = Application.Current.Windows[0].Page;
      return mainPage.Handler.MauiContext.Services.GetService(Type);
   }
}