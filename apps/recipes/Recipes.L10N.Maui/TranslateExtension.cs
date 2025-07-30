namespace Recipes.L10N.Maui;

[ContentProperty(nameof(Key))]
public class TranslateExtension : IMarkupExtension<Binding>
{
   public string Key { get; set; }

   public Binding ProvideValue(IServiceProvider serviceProvider) =>
      new()
      {
         Mode = BindingMode.OneWay,
         Path = $"[{Key}]",
         Source = LocalizedResourcesProvider.Instance
      };

   object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}