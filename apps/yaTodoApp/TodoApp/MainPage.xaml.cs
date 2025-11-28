using System.Collections.ObjectModel;

namespace TodoApp;

public partial class MainPage
{
   private readonly Database _database;

   public MainPage()
   {
      InitializeComponent();
      _database = new Database();
   }

   public ObservableCollection<TodoItem> Todos { get; set; } = new();

   protected override async void OnAppearing()
   {
      base.OnAppearing();

      var todos = await _database.GetTodos();
      foreach (var todo in todos)
      {
         Todos.Add(todo);
      }
   }

   private async void Button_Clicked(object sender, EventArgs e)
   {
      var todo = new TodoItem
      {
         Due = dueDatepicker.Date,
         Title = todoTitleEntry.Text
      };

      var inserted = await _database.AddTodo(todo);
      if (inserted != 0)
      {
         Todos.Add(todo);
         todoTitleEntry.Text = string.Empty;
         dueDatepicker.Date = DateTime.Now;
      }
   }

   private async void SwipeItem_Invoked(object sender, EventArgs e)
   {
      var item = sender as SwipeItem;
      var mainPage = Application.Current?.Windows[0].Page;
      await mainPage?.DisplayAlert(item.Text, $"You invoked the {item.Text} action.", "OK");
   }
}