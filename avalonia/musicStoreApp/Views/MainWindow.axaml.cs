using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using MusicStoreApp.ViewModels;
using ReactiveUI;

namespace MusicStoreApp.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
   public MainWindow()
   {
      InitializeComponent();

      this.WhenActivated(action => action(ViewModel!.ShowDialig.RegisterHandler(DoShowDialogAsync)));
   }

   private async Task DoShowDialogAsync(InteractionContext<MusicStoreViewModel, AlbumViewModel?> interaction)
   {
      var dialog = new MusicStoreWindow
      {
         DataContext = interaction.Input
      };
      var result = await dialog.ShowDialog<AlbumViewModel?>(this);
      interaction.SetOutput(result);
   }
}