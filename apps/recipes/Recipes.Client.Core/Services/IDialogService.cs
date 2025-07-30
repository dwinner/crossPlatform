namespace Recipes.Client.Core.Services;

// FIXME: Hardcoded i10n strings passed as optional parameters

public interface IDialogService
{
   Task Notify(string title, string message, string buttonText = "OK");

   Task<bool> AskYesNo(string title, string message, string trueButtonText = "Yes", string falseButtonText = "No");

   Task<string?> Ask(string title, string message, string acceptButtonText = "OK", string cancelButtonText = "Cancel");
}