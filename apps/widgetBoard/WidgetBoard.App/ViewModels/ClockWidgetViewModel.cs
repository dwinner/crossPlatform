using CommunityToolkit.Mvvm.ComponentModel;

namespace WidgetBoard.App.ViewModels;

public partial class ClockWidgetViewModel : ViewModelBase, IWidgetViewModel
{
   private readonly Scheduler _scheduler = new();

   [ObservableProperty] private DateOnly _date;

   [ObservableProperty] private TimeOnly _time;

   public ClockWidgetViewModel()
   {
      SetTime(DateTime.Now);
   }

   public int Position { get; set; }

   public string Type => "Clock";

   private void SetTime(DateTime dateTime)
   {
      Date = DateOnly.FromDateTime(dateTime);
      Time = TimeOnly.FromDateTime(dateTime);
      _scheduler.ScheduleAction(TimeSpan.FromSeconds(1), () => SetTime(DateTime.Now));
   }
}