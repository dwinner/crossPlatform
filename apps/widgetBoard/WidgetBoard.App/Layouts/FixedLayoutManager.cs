using System.Windows.Input;
using WidgetBoard.App.Controls;

namespace WidgetBoard.App.Layouts;

public class FixedLayoutManager : BindableObject, ILayoutManager
{
   public static readonly BindableProperty NumberOfColumnsProperty = BindableProperty.Create(
      nameof(NumberOfColumns),
      typeof(int),
      typeof(FixedLayoutManager),
      defaultBindingMode: BindingMode.OneWay,
      propertyChanged: OnNumberOfColumnsChanged
   );

   public static readonly BindableProperty NumberOfRowsProperty = BindableProperty.Create(
      nameof(NumberOfRows),
      typeof(int),
      typeof(FixedLayoutManager),
      defaultBindingMode: BindingMode.OneWay,
      propertyChanged: OnNumberOfRowsChanged
   );

   public static readonly BindableProperty PlaceholderTappedCommandProperty = BindableProperty.Create(
      nameof(PlaceholderTappedCommand),
      typeof(ICommand),
      typeof(FixedLayoutManager)
   );

   private BoardLayout? _board;
   private bool _isInitialized;

   public int NumberOfColumns
   {
      get => (int)GetValue(NumberOfColumnsProperty);
      set => SetValue(NumberOfColumnsProperty, value);
   }

   public int NumberOfRows
   {
      get => (int)GetValue(NumberOfRowsProperty);
      set => SetValue(NumberOfRowsProperty, value);
   }

   public ICommand PlaceholderTappedCommand
   {
      get => (ICommand)GetValue(PlaceholderTappedCommandProperty);
      set => SetValue(PlaceholderTappedCommandProperty, value);
   }

   public BoardLayout? Board
   {
      get => _board;
      set
      {
         _board = value;
         InitializeGrid();
      }
   }

   public void SetPosition(BindableObject bindableObject, int position)
   {
      if (NumberOfColumns == 0 || Board is null)
      {
         return;
      }

      var column = position % NumberOfColumns;
      var row = position / NumberOfColumns;

      Grid.SetColumn(bindableObject, column);
      Grid.SetRow(bindableObject, row);

      var placeholder = Board.Placeholders.FirstOrDefault(p => p.Position == position);
      if (placeholder is not null)
      {
         Board.RemovePlaceholder(placeholder);
      }
   }

   private static void OnNumberOfColumnsChanged(BindableObject bindable, object oldValue, object newValue)
   {
      var manager = (FixedLayoutManager)bindable;
      manager.InitializeGrid();
   }

   private static void OnNumberOfRowsChanged(BindableObject bindable, object oldValue, object newValue)
   {
      var manager = (FixedLayoutManager)bindable;
      manager.InitializeGrid();
   }

   private void OnTapGestureRecognizerTapped(object? sender, EventArgs e)
   {
      if (sender is Placeholder placeholder
          && PlaceholderTappedCommand.CanExecute(placeholder.Position))
      {
         PlaceholderTappedCommand.Execute(placeholder.Position);
      }
   }

   private void InitializeGrid()
   {
      if (Board is null || NumberOfColumns == 0 || NumberOfRows == 0 || _isInitialized)
      {
         return;
      }

      _isInitialized = true;
      for (var i = 0; i < NumberOfColumns; i++)
      {
         Board.AddColumn(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
      }

      for (var i = 0; i < NumberOfRows; i++)
      {
         Board.AddRow(new RowDefinition(new GridLength(1, GridUnitType.Star)));
      }

      for (var column = 0; column < NumberOfColumns; column++)
      {
         for (var row = 0; row < NumberOfRows; row++)
         {
            var placeholder = new Placeholder
            {
               Position = row * NumberOfColumns + column
            };
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += OnTapGestureRecognizerTapped;
            placeholder.GestureRecognizers.Add(tapGestureRecognizer);
            Board.AddPlaceholder(placeholder);
            Grid.SetColumn(placeholder, column);
            Grid.SetRow(placeholder, row);
         }
      }
   }
}