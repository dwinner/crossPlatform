using System.Collections.ObjectModel;
using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using WorkingWithWebApi.Messages;
using WorkingWithWebApi.Model;
using WorkingWithWebApi.Services;

namespace WorkingWithWebApi.ViewModel;

public class BookViewModel : ObservableObject
{
   private const string ApplicationUrl = "http://localhost:5080";
   private readonly IMessenger _messenger;
   private ObservableCollection<Book> _books;
   private Book _newBook;
   private Book _selectedBook;

   public BookViewModel()
   {
      Books = [];
      LoadBooksCommand = new Command(async () => { await LoadBooksAsync(); });
      AddBookCommand = new Command(async () =>
      {
         if (NewBook != null)
         {
            await AddBookAsync();
         }
      });
      DeleteBookCommand = new Command(async () =>
      {
         if (SelectedBook != null)
         {
            // *** REMEMBER TO ASK CONFIRMATION TO THE USER BEFORE DELETING! ***
            // *** INFORM THE USER THIS ACTION IS IRREVERSIBLE ***
            await DeleteBookAsync();
         }
      });
      _messenger = WeakReferenceMessenger.Default;
   }

   public ObservableCollection<Book> Books
   {
      get => _books;
      set => SetProperty(ref _books, value);
   }

   public Book NewBook
   {
      get => _newBook;
      set => SetProperty(ref _newBook, value);
   }

   public Book SelectedBook
   {
      get => _selectedBook;
      set => SetProperty(ref _selectedBook, value);
   }

   public Command LoadBooksCommand { get; }

   public Command AddBookCommand { get; }

   public Command DeleteBookCommand { get; }

   private async Task LoadBooksAsync()
   {
      const string url = $"{ApplicationUrl}/api/books";
      var result = await WebApiService.GetDataAsync(url);

      switch (result.StatusCode)
      {
         case HttpStatusCode.OK:
            var resultString = await result.Content.ReadAsStringAsync();
            var deserialized = JsonConvert.DeserializeObject<List<Book>>(resultString);
            Books = new ObservableCollection<Book>(deserialized);
            return;

         default:
            _messenger.Send(new DataStatusMessage(DataStatus.BookError));
            return;
      }
   }

   private async Task AddBookAsync()
   {
      if (NewBook == null)
      {
         return;
      }

      const string url = $"{ApplicationUrl}/api/books";
      var result = await WebApiService.WriteDataAsync(NewBook, url);

      switch (result.StatusCode)
      {
         case HttpStatusCode.OK:
         case HttpStatusCode.Created:
            var resultString = await result.Content.ReadAsStringAsync();
            var deserialized = JsonConvert.DeserializeObject<Book>(resultString);
            Books.Add(deserialized);
            _messenger.Send(new DataStatusMessage(DataStatus.BookSaved));
            return;

         default:
            _messenger.Send(new DataStatusMessage(DataStatus.BookError));
            return;
      }
   }

   private async Task DeleteBookAsync()
   {
      const string url = $"{ApplicationUrl}/api/books";
      var result = await WebApiService.DeleteDataAsync(url, SelectedBook.Id);
      switch (result.StatusCode)
      {
         case HttpStatusCode.OK:
            var resultString = await result.Content.ReadAsStringAsync();

            // Do anything you need with the deleted object...
            var deserializedBook = JsonConvert.DeserializeObject<Book>(resultString);
            Books.Remove(SelectedBook);
            _messenger.Send(new DataStatusMessage(DataStatus.BookDeleted));
            return;

         default:
            _messenger.Send(new DataStatusMessage(DataStatus.BookError));
            return;
      }
   }
}