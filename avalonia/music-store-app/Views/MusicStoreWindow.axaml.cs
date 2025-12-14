using System;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using MusicStoreApp.ViewModels;
using ReactiveUI;

namespace MusicStoreApp.Views;

public partial class MusicStoreWindow : ReactiveWindow<MusicStoreViewModel>
{
   public MusicStoreWindow()
   {
      InitializeComponent();
      if (Design.IsDesignMode)
      {
         return;
      }

      this.WhenActivated(action => action(ViewModel!.BuyMusicCommand.Subscribe(Close)));
   }
}