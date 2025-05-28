using Android.App.Job;
using Android.Content;
using Java.Lang;
using MeTracker.Services;
using Application = Android.App.Application;

namespace MeTracker.Droid;

internal sealed class DroidLocationTracking : ILocationTrackingService
{
   public void StartTracking()
   {
      var javaClass = Class.FromType(typeof(LocationJobService));
      var componentName = new ComponentName(Application.Context, javaClass);
      var jobBuilder = new JobInfo.Builder(1, componentName);

      jobBuilder.SetOverrideDeadline(1000);
      jobBuilder.SetPersisted(true);
      jobBuilder.SetRequiresDeviceIdle(false);
      jobBuilder.SetRequiresBatteryNotLow(true);
      var jobInfo = jobBuilder.Build();
      var jobScheduler = (JobScheduler?)Application.Context.GetSystemService(Context.JobSchedulerService);
      if (jobInfo is not null)
      {
         jobScheduler?.Schedule(jobInfo);
      }
   }
}