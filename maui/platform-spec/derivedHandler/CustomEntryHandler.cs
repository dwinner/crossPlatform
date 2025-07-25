using Microsoft.Maui.Handlers;

namespace c7_DerivedHandler;

public partial class CustomEntryHandler() : EntryHandler(PropertyMapper, CommandMapper)
{
   public static readonly IPropertyMapper<CustomEntry, CustomEntryHandler> PropertyMapper =
      new PropertyMapper<CustomEntry, CustomEntryHandler>(Mapper)
      {
         [nameof(CustomEntry.SelectionColor)] = MapSelectionColor
      };

   static partial void MapSelectionColor(CustomEntryHandler handler, CustomEntry entry);
}