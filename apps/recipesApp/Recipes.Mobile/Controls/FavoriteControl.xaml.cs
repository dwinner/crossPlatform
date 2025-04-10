﻿using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Recipes.Mobile.Controls;

public partial class FavoriteControl
{
   public static readonly BindableProperty IsFavoriteProperty = BindableProperty.Create(
      nameof(IsFavorite),
      typeof(bool),
      typeof(FavoriteControl),
      defaultBindingMode: BindingMode.TwoWay,
      propertyChanged: OnIsFavoriteChanged);

   public static readonly BindableProperty ToggledCommandProperty = BindableProperty.Create(
      nameof(ToggledCommand),
      typeof(ICommand),
      typeof(FavoriteControl),
      propertyChanged: ToggledCommandChanged);

   public FavoriteControl()
   {
      InitializeComponent();
   }

   public bool IsInteractive { get; private set; }

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

   protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
   {
      base.OnPropertyChanged(propertyName);
      if (propertyName == nameof(IsEnabled))
      {
         UpdateIsInteractive();
      }
   }

   private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
   {
      if (IsInteractive)
      {
         IsFavorite = !IsFavorite;
         ToggledCommand?.Execute(IsFavorite);
      }
   }

   private async Task AnimateChange()
   {
      await icon.ScaleTo(1.5, 100);
      await icon.ScaleTo(1, 100);
   }

   private void CanExecuteChanged(object? sender, EventArgs e) => UpdateIsInteractive();

   private void UpdateIsInteractive()
   {
      IsInteractive = IsEnabled
                      && (ToggledCommand?.CanExecute(IsFavorite) ?? false);
      OnPropertyChanged(nameof(IsInteractive));
   }

   private static void OnIsFavoriteChanged(BindableObject bindable, object oldValue, object newValue) =>
      (bindable as FavoriteControl)?.AnimateChange();

   private static void ToggledCommandChanged(BindableObject bindable, object oldValue, object newValue)
   {
      if (bindable is not FavoriteControl control)
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
}