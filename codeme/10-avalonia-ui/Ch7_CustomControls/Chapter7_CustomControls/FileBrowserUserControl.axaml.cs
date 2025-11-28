using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace Chapter7_CustomControls
{
    public partial class FileBrowserUserControl : UserControl
    {
        public FileBrowserUserControl()
        {
            InitializeComponent();
        }

        private static StyledProperty<string> FileNameProperty =
            AvaloniaProperty.Register<FileBrowserUserControl, string>(nameof(FileName));

        public string FileName
        {
            get { return Convert.ToString(GetValue(FileNameProperty)); }
            set 
            { 
                SetValue(FileNameProperty, value);
                RaiseEvent(new RoutedEventArgs(fileNameChangedEvent));
            }
        }


        private static readonly RoutedEvent fileNameChangedEvent =
                RoutedEvent.Register<FileBrowserUserControl, 
                RoutedEventArgs>(nameof(FileNameChanged), 
                RoutingStrategies.Bubble);

        public event EventHandler<RoutedEventArgs> FileNameChanged
        {
            add { AddHandler(fileNameChangedEvent, value); }
            remove { RemoveHandler(fileNameChangedEvent, value); }
        }


        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filters.Add(new FileDialogFilter { Extensions = new 
                System.Collections.Generic.List<string> { "*.*" }, Name="All files" });
            Window parentWindow = (Window)((StackPanel)Parent).Parent;
            var result = await openDialog.ShowAsync(parentWindow);
            if(result != null)
                FileName = result.FirstOrDefault();
        }
    }
}
