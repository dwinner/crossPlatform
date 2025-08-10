using System.Windows.Input;

namespace SticksAndStones.Controls;

public partial class ActivityButton
{
   public static readonly BindableProperty CommandProperty = BindableProperty.Create(
      nameof(Command),
      typeof(ICommand),
      typeof(ActivityButton),
      defaultBindingMode: BindingMode.TwoWay
   );

   public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
      nameof(CommandParameter),
      typeof(object),
      typeof(ActivityButton),
      defaultBindingMode: BindingMode.TwoWay
   );

   public static readonly BindableProperty TextProperty = BindableProperty.Create(
      nameof(Text),
      typeof(string),
      typeof(ActivityButton),
      string.Empty,
      BindingMode.TwoWay
   );

   public static readonly BindableProperty IsRunningProperty = BindableProperty.Create(
      nameof(IsRunning),
      typeof(bool),
      typeof(ActivityButton),
      false
   );

   public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
      nameof(FontFamily),
      typeof(string),
      typeof(ActivityButton),
      string.Empty,
      BindingMode.TwoWay
   );

   public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
      nameof(FontSize),
      typeof(double),
      typeof(ActivityButton),
      Device.GetNamedSize(NamedSize.Small, typeof(Label)),
      BindingMode.TwoWay
   );

   public ActivityButton()
   {
      InitializeComponent();
   }

   public ICommand Command
   {
      get => (ICommand)GetValue(CommandProperty);
      set => SetValue(CommandProperty, value);
   }

   public object CommandParameter
   {
      get => GetValue(CommandParameterProperty);
      set => SetValue(CommandParameterProperty, value);
   }

   public string Text
   {
      get => (string)GetValue(TextProperty);
      set => SetValue(TextProperty, value);
   }

   public bool IsRunning
   {
      get => (bool)GetValue(IsRunningProperty);
      set => SetValue(IsRunningProperty, value);
   }

   public string FontFamily
   {
      get => (string)GetValue(Label.FontFamilyProperty);
      set => SetValue(Label.FontFamilyProperty, value);
   }

   public double FontSize
   {
      set => SetValue(FontSizeProperty, value);
      get => (double)GetValue(FontSizeProperty);
   }
}