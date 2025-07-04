using BooksApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController(BooksContext context) : ControllerBase
{
   // GET: api/Books
   [HttpGet]
   public async Task<ActionResult<IEnumerable<Book>>> GetBooks() => await context.Books.ToListAsync();

   // GET: api/Books/5
   [HttpGet("{id:int}")]
   public async Task<ActionResult<Book>> GetBooks(int id)
   {
      var books = await context.Books.FindAsync(id);
      if (books == null)
      {
         return NotFound();
      }

      return books;
   }

   // PUT: api/Books/5
   // To protect from over-posting attacks, enable the specific properties you want to bind to, for
   // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
   [HttpPut("{id:int}")]
   public async Task<IActionResult> PutBook(int id, Book book)
   {
      if (id != book.Id)
      {
         return BadRequest();
      }

      context.Entry(book).State = EntityState.Modified;

      try
      {
         await context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
         if (!BooksExists(id))
         {
            return NotFound();
         }

         throw;
      }

      return NoContent();
   }

   // POST: api/Books
   // To protect from overposting attacks, enable the specific properties you want to bind to, for
   // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
   [HttpPost]
   public async Task<ActionResult<Book>> PostBook(Book book)
   {
      context.Books.Add(book);
      await context.SaveChangesAsync();

      return CreatedAtAction("GetBooks", new { id = book.Id }, book);
   }

   // DELETE: api/Books/5
   [HttpDelete("{id:int}")]
   public async Task<ActionResult<Book>> DeleteBook(int id)
   {
      var book = await context.Books.FindAsync(id);
      if (book == null)
      {
         return NotFound();
      }

      context.Books.Remove(book);
      await context.SaveChangesAsync();

      return book;
   }

   private bool BooksExists(int id)
   {
      return context.Books.Any(book => book.Id == id);
   }
}