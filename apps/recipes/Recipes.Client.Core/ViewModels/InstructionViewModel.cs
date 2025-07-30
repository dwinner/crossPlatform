namespace Recipes.Client.Core.ViewModels;

public class InstructionViewModel : InstructionBaseViewModel
{
   public InstructionViewModel(int index, string description)
   {
      Index = index;
      Description = description;
   }

   public int Index { get; }

   public string Description { get; }
}