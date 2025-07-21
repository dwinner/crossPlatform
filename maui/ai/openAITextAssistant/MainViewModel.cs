using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenAI.Chat;

namespace c6_OpenAITextAssistant;

public partial class MainViewModel : ObservableObject
{
   private readonly ChatClient _aiClient = new("gpt-3.5-turbo", "[Your API Key]");

   [ObservableProperty] private string _letterText;

   [RelayCommand]
   private async Task FixErrorsAsync()
   {
      try
      {
         var updates = _aiClient.CompleteChatStreamingAsync(
            new SystemChatMessage("You are an assistant correcting text"),
            new UserChatMessage($"Fix grammar errors in the following text: {LetterText}")
         );

         LetterText = null!;
         await foreach (var update in updates)
         {
            foreach (var updatePart in update.ContentUpdate)
            {
               LetterText += updatePart.Text;
            }
         }
      }
      catch (Exception ex)
      {
         await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
      }
   }
}