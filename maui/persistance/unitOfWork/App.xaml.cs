using c4_LocalDatabaseConnection.DataAccess;
using SQLitePCL;

namespace c4_LocalDatabaseConnection;

public partial class App
{
   public App()
   {
      using var context = new CrmContext();
      Batteries_V2.Init();
      context.Database.EnsureCreated();

      InitializeComponent();
   }

   protected override Window CreateWindow(IActivationState activationState) => new(new AppShell());
}