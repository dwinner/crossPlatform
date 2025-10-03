using WidgetBoard.App.ViewModels;

namespace WidgetBoard.App.Pages;

public partial class BoardListPage
{
   private readonly BoardListPageViewModel _viewModel;

   public BoardListPage(BoardListPageViewModel viewModel)
   {
      _viewModel = viewModel;
      InitializeComponent();
      BindingContext = _viewModel;
   }

   protected override void OnNavigatedTo(NavigatedToEventArgs args)
   {
      base.OnNavigatedTo(args);
      _viewModel.LoadBoards();
   }
}