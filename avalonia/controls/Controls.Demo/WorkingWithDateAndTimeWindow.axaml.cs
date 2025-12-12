using System.Globalization;
using System.Threading;
using Avalonia.Controls;

namespace Controls.Demo;

public partial class WorkingWithDateAndTimeWindow : Window
{
   public WorkingWithDateAndTimeWindow()
   {
      InitializeComponent();
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
   }

   private void DatePicker1_SelectedDateChanged(object sender, DatePickerSelectedValueChangedEventArgs e)
   {
      var newDate = e.NewDate;
      var oldDate = e.OldDate;
   }

   private void Calendar1_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
   {
      var newDate = e.AddedDate;
      var oldDate = e.RemovedDate;
   }

   private void TimePicker1_SelectedTimeChanged(object sender, TimePickerSelectedValueChangedEventArgs e)
   {
      var newTime = e.NewTime;
      var oldTime = e.OldTime;
   }
}