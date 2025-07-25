namespace c7_DerivedHandler;

public static class CustomBuilderExtensions
{
   public static MauiAppBuilder UseCustomEntry(this MauiAppBuilder builder)
   {
      builder.ConfigureMauiHandlers(handlers => { handlers.AddHandler<CustomEntry, CustomEntryHandler>(); });

      return builder;
   }
}