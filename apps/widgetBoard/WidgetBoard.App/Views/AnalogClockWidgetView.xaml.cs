using System.ComponentModel;
using WidgetBoard.App.ViewModels;

namespace WidgetBoard.App.Views;

public partial class AnalogClockWidgetView : IWidgetView
{
   public AnalogClockWidgetView()
   {
      InitializeComponent();
   }

   public IWidgetViewModel WidgetViewModel
   {
      get => (IWidgetViewModel)BindingContext;
      set
      {
         BindingContext = value;
         if (BindingContext is IDrawable drawable)
         {
            Drawable = drawable;
         }

         if (BindingContext is INotifyPropertyChanged propertyChanged)
         {
            propertyChanged.PropertyChanged += ClockWidgetViewModelOnPropertyChanged;
         }
      }
   }

   private void ClockWidgetViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
   {
      if (e.PropertyName == nameof(AnalogClockWidgetViewModel.Time))
      {
         Invalidate();
      }
   }
}