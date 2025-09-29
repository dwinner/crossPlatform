using System.Collections;
using WidgetBoard.App.Controls;
using WidgetBoard.App.Views;

namespace WidgetBoard.App.Layouts;

public partial class BoardLayout
{
   public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
      nameof(ItemsSource),
      typeof(IEnumerable),
      typeof(BoardLayout)
   );

   public static readonly BindableProperty ItemTemplateSelectorProperty = BindableProperty.Create(
      nameof(ItemTemplateSelector),
      typeof(DataTemplateSelector),
      typeof(BoardLayout)
   );

   private ILayoutManager? _underlyingLayout;

   public BoardLayout()
   {
      InitializeComponent();
   }

   public IReadOnlyList<Placeholder> Placeholders => placeholderGrid.Children.OfType<Placeholder>().ToList();

   public DataTemplateSelector ItemTemplateSelector
   {
      get => (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
      set => SetValue(ItemTemplateSelectorProperty, value);
   }

   public ILayoutManager? UnderlyingLayout
   {
      get => _underlyingLayout;
      set
      {
         _underlyingLayout = value;
         if (_underlyingLayout is not null)
         {
            _underlyingLayout.Board = this;
         }
      }
   }

   public IEnumerable ItemsSource
   {
      get => (IEnumerable)GetValue(ItemsSourceProperty);
      set => SetValue(ItemsSourceProperty, value);
   }

   public void AddPlaceholder(Placeholder placeholder) => placeholderGrid.Children.Add(placeholder);

   public void RemovePlaceholder(Placeholder placeholder) => placeholderGrid.Children.Remove(placeholder);

   public void AddColumn(ColumnDefinition columnDefinition)
   {
      placeholderGrid.ColumnDefinitions.Add(columnDefinition);
      widgetGrid.ColumnDefinitions.Add(columnDefinition);
   }

   public void AddRow(RowDefinition rowDefinition)
   {
      placeholderGrid.RowDefinitions.Add(rowDefinition);
      widgetGrid.RowDefinitions.Add(rowDefinition);
   }

   private void OnWidgetsChildAdded(object? sender, ElementEventArgs e)
   {
      var element = e.Element;
      if (element is IWidgetView widgetView)
      {
         UnderlyingLayout?.SetPosition(element, widgetView.Position);
      }
   }

   protected override void OnBindingContextChanged()
   {
      base.OnBindingContextChanged();
      if (_underlyingLayout is not null)
      {
         _underlyingLayout.BindingContext = BindingContext;
      }
   }
}