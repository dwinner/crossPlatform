namespace c6_OpenAITextAssistant;

public partial class App
{
   public App()
   {
      InitializeComponent();
   }

   protected override Window CreateWindow(IActivationState activationState) => new(new AppShell());
}