using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Recipes.Client.Core.Validation;

public class ValidationErrorExposer : INotifyPropertyChanged, IDisposable
{
   private readonly ObservableValidator _validator;

   public ValidationErrorExposer(ObservableValidator validator)
   {
      _validator = validator;
      _validator.ErrorsChanged += OnErrorsChanged;
   }

   public List<ValidationResult> this[string property] => _validator.GetErrors(property).ToList();

   public void Dispose()
   {
      _validator.ErrorsChanged -= OnErrorsChanged;
   }

   public event PropertyChangedEventHandler? PropertyChanged;

   private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
   {
      PropertyChanged?.Invoke(
         this, new PropertyChangedEventArgs($"Item[{e.PropertyName}]"));
   }
}