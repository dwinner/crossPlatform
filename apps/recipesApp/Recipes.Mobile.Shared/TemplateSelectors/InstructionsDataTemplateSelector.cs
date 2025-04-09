using Recipes.Client.Core.ViewModels;

namespace Recipes.Mobile.Shared.TemplateSelectors;

public class InstructionsDataTemplateSelector : DataTemplateSelector
{
   public DataTemplate? InstructionTemplate { get; set; }

   public DataTemplate? NoteTemplate { get; set; }

   protected override DataTemplate? OnSelectTemplate(object item, BindableObject container) =>
      item switch
      {
         InstructionViewModel => InstructionTemplate,
         NoteViewModel => NoteTemplate,
         _ => null
      };
}