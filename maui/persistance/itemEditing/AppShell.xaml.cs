using c4_LocalDatabaseConnection.DataAccess;
using c4_LocalDatabaseConnection.Views;
using SQLitePCL;

namespace c4_LocalDatabaseConnection;

public partial class AppShell
{
   public AppShell()
   {
      Batteries_V2.Init();
      using var context = new CrmContext();
      context.Database.EnsureCreated();

      InitializeComponent();
      Routing.RegisterRoute(nameof(CustomerEditPage), typeof(CustomerEditPage));
      Routing.RegisterRoute(nameof(CustomerDetailPage), typeof(CustomerDetailPage));
   }
}