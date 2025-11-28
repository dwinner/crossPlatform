using Avalonia.Controls;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Ch5_Controls
{
    public partial class WorkingWithDateAndTimeWindow : Window
    {
        public WorkingWithDateAndTimeWindow()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

        }

        private void DatePicker1_SelectedDateChanged(object sender, 
            DatePickerSelectedValueChangedEventArgs e)
        {
            DateTimeOffset? newDate = e.NewDate;
            DateTimeOffset? oldDate = e.OldDate;
        }

        private void Calendar1_DisplayDateChanged(object sender, 
            CalendarDateChangedEventArgs e) 
        {
            DateTime? newDate = e.AddedDate;
            DateTime? oldDate = e.RemovedDate;
        }

        private void TimePicker1_SelectedTimeChanged(object sender, 
            TimePickerSelectedValueChangedEventArgs e)
        {
            TimeSpan? newTime = e.NewTime;
            TimeSpan? oldTime = e.OldTime;
        }
    }
}
