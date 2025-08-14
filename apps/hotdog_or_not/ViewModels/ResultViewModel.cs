using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using HotdogOrNot.Models;

namespace HotdogOrNot.ViewModels;

public partial class ResultViewModel : ObservableObject, IQueryAttributable
{
   [ObservableProperty] private string _description;

   [ObservableProperty] private byte[] _photoBytes;

   [ObservableProperty] private string _title;

   public void ApplyQueryAttributes(IDictionary<string, object> query)
   {
      var result = query["result"] as Result;
      Debug.Assert(result != null);

      Initialize(result);
   }

   public void Initialize(Result result)
   {
      PhotoBytes = result.PhotoBytes;

      switch (result.IsHotdog)
      {
         case true when result.Confidence > 0.9:
            Title = "Hot dog";
            Description = "This is for sure a hot dog";
            break;
         case true:
            Title = "Maybe";
            Description = "This is maybe a hot dog";
            break;
         default:
            Title = "Not a hot dog";
            Description = "This is not a hot dog";
            break;
      }
   }
}