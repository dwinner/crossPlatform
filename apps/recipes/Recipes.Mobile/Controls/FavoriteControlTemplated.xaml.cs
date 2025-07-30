using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Recipes.Mobile.Controls;

public partial class FavoriteControlTemplated
{
   public static readonly BindableProperty IsFavoriteProperty = BindableProperty.Create(
      nameof(IsFavorite),
      typeof(bool),
      typeof(FavoriteControlTemplated),
      defaultBindingMode: BindingMode.TwoWay,
      propertyChanged: OnIsFavoriteChanged);

   public static readonly BindableProperty ToggledCommandProperty = BindableProperty.Create(
      nameof(ToggledCommand),
      typeof(ICommand),
      typeof(FavoriteControlTemplated),
      propertyChanged: ToggledCommandChanged);

   private VisualElement? _scalableContent;

   public FavoriteControlTemplated()
   {
      InitializeComponent();
      if (ControlTemplate != null)
      {
         return;
      }

      var template = Resources["DefaultTemplate"];
      ControlTemplate = template as ControlTemplate;
   }

   public bool IsFavorite
   {
      get => (bool)GetValue(IsFavoriteProperty);
      set => SetValue(IsFavoriteProperty, value);
   }

   public ICommand? ToggledCommand
   {
      get => (ICommand)GetValue(ToggledCommandProperty);
      set => SetValue(ToggledCommandProperty, value);
   }

   public bool IsInteractive { get; set; }

   protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
   {
      base.OnPropertyChanged(propertyName);
      if (propertyName == nameof(IsEnabled))
      {
         UpdateIsInteractive();
      }
   }

   protected override void OnApplyTemplate()
   {
      base.OnApplyTemplate();
      _scalableContent = GetTemplateChild("scalableContent") as VisualElement;
   }

   private static void OnIsFavoriteChanged(BindableObject bindable, object oldValue, object newValue) =>
      (bindable as FavoriteControlTemplated)?.AnimateChange();

   private static void ToggledCommandChanged(BindableObject bindable, object oldValue, object newValue)
   {
      if (bindable is not FavoriteControlTemplated control)
      {
         return;
      }

      if (oldValue is ICommand oldCommand)
      {
         oldCommand.CanExecuteChanged -= control.CanExecuteChanged;
      }

      if (newValue is ICommand newCommand)
      {
         newCommand.CanExecuteChanged += control.CanExecuteChanged;
      }

      control.UpdateIsInteractive();
   }

   private void UpdateIsInteractive()
   {
      IsInteractive = IsEnabled
                      && (ToggledCommand?.CanExecute(IsFavorite) ?? false);
      OnPropertyChanged(nameof(IsInteractive));
   }

   private void CanExecuteChanged(object? sender, EventArgs e) => UpdateIsInteractive();

   private async Task AnimateChange()
   {
      if (_scalableContent is null)
      {
         return;
      }

      await _scalableContent.ScaleTo(1.5, 100);
      await _scalableContent.ScaleTo(1, 100);
   }

   private void TapGestureRecognizer_OnTapped(object? sender, TappedEventArgs e)
   {
      if (!IsInteractive)
      {
         return;
      }

      IsFavorite = !IsFavorite;
      ToggledCommand?.Execute(IsFavorite);
   }
}