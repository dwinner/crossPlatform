using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CustomControls;

public partial class FileBrowserUserControl : UserControl
{
   public static readonly StyledProperty<string> FileNameProperty =
      AvaloniaProperty.Register<FileBrowserUserControl, string>(nameof(FileName));

   private static readonly RoutedEvent _FileNameChangedEvent =
      RoutedEvent.Register<FileBrowserUserControl, RoutedEventArgs>(
         nameof(FileNameChanged),
         RoutingStrategies.Bubble);

   public FileBrowserUserControl()
   {
      InitializeComponent();
   }

   public string FileName
   {
      get => Convert.ToString(GetValue(FileNameProperty));
      set
      {
         SetValue(FileNameProperty, value);
         RaiseEvent(new RoutedEventArgs(_FileNameChangedEvent));
      }
   }

   public event EventHandler<RoutedEventArgs> FileNameChanged
   {
      add => AddHandler(_FileNameChangedEvent, value);
      remove => RemoveHandler(_FileNameChangedEvent, value);
   }

   private async void BrowseButton_Click(object sender, RoutedEventArgs e)
   {
      var openDialog = new OpenFileDialog();
      openDialog.Filters.Add(new FileDialogFilter
      {
         Extensions = ["*.*"],
         Name = "All files"
      });

      var parentWindow = (Window)((StackPanel)Parent).Parent;
      var result = await openDialog.ShowAsync(parentWindow).ConfigureAwait(true);
      if (result != null)
      {
         FileName = (result is [var first, ..] ? first : null) ?? string.Empty;
      }
   }
}