namespace Recipes.Client.Core.ViewModels;

public class NoteViewModel : InstructionBaseViewModel
{
   public NoteViewModel(string note)
      => Note = note;

   public string Note { get; }
}