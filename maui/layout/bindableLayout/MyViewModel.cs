using System.Collections.ObjectModel;

namespace c1_BindableLayout;

public class MyViewModel
{
   public ObservableCollection<ActionInfo> DynamicActions { get; set; } =
   [
      new() { Caption = "Action1" },
      new() { Caption = "Action2" },
      new() { Caption = "Action3" }
   ];
}

public class ActionInfo
{
   public string? Caption { get; set; }
}