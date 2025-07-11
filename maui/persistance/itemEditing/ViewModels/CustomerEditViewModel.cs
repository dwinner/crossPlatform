using c4_LocalDatabaseConnection.DataAccess;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

namespace c4_LocalDatabaseConnection.ViewModels;

public partial class CustomerEditViewModel : CustomerDetailViewModel
{
   [ObservableProperty] private bool _isNewItem;

   //[ObservableProperty]
   //Customer item;

   //public Func<Customer, Task> ParentSaveAction { get; set; }

   [RelayCommand]
   private async Task SaveAsync()
   {
      var context = new CrmContext();
      if (IsNewItem)
      {
         context.Customers.Add(Item);
      }
      else
      {
         context.Customers.Attach(Item);
         context.Entry(Item).State = EntityState.Modified;
      }

      await context.SaveChangesAsync();
      await ParentRefreshAction(Item);
      await Shell.Current.GoToAsync("..");
   }

   public override void ApplyQueryAttributes(IDictionary<string, object> query)
   {
      if (query.TryGetValue("IsNewItem", out var isNew))
      {
         IsNewItem = (bool)isNew;
      }

      base.ApplyQueryAttributes(query);
   }
}