using c6_OfflineDataSyncClient.Model;
using SQLitePCL;

namespace c6_OfflineDataSyncClient;

public partial class App
{
   public App()
   {
      using var context = new LocalAppDbContext();
      Batteries_V2.Init();
      context.Database.EnsureCreated();

      InitializeComponent();
   }

   protected override Window CreateWindow(IActivationState? activationState) => new(new AppShell());
}