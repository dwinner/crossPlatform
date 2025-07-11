using c4_LocalDatabaseConnection.DataAccess;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c4_LocalDatabaseConnection.ViewModels;

public partial class CustomerEditViewModel : CustomerDetailViewModel
{
   [ObservableProperty] private bool _isNewItem;

   [RelayCommand]
   private async Task SaveAsync()
   {
      using var uof = new CrmUnitOfWork();
      if (IsNewItem)
      {
         await uof.Items.AddAsync(Item);
      }
      else
      {
         await uof.Items.UpdateAsync(Item);
      }

      await uof.SaveAsync();
      await ParentRefreshAction(Item);
      await Shell.Current.GoToAsync("..");
   }

   public override void ApplyQueryAttributes(IDictionary<string, object> query)
   {
      if (query.TryGetValue(nameof(IsNewItem), out var isNew))
      {
         IsNewItem = (bool)isNew;
      }

      base.ApplyQueryAttributes(query);
   }
}