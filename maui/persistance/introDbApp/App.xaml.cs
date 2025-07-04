namespace People;

public partial class App
{
   public App(PersonRepository repository)
   {
      InitializeComponent();
      MainPage = new AppShell();
      PersonRepo = repository;
   }

   public static PersonRepository PersonRepo { get; private set; }
}