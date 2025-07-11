using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiServer.Model;

namespace WebApiServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(CrmContext context) : ControllerBase
{
   // GET: api/Customers
   [HttpGet]
   public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers() => await context.Customers.ToListAsync();

   // GET: api/Customers/5
   [HttpGet("{id}")]
   public async Task<ActionResult<Customer>> GetCustomer(int id)
   {
      var customer = await context.Customers.FindAsync(id);
      if (customer == null)
      {
         return NotFound();
      }

      return customer;
   }

   // PUT: api/Customers/5
   // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
   [HttpPut("{id}")]
   public async Task<IActionResult> PutCustomer(int id, Customer customer)
   {
      if (id != customer.Id)
      {
         return BadRequest();
      }

      context.Entry(customer).State = EntityState.Modified;

      try
      {
         await context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
         if (!CustomerExists(id))
         {
            return NotFound();
         }

         throw;
      }

      return NoContent();
   }

   // POST: api/Customers
   // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
   [HttpPost]
   public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
   {
      context.Customers.Add(customer);
      await context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
   }

   // DELETE: api/Customers/5
   [HttpDelete("{id}")]
   public async Task<IActionResult> DeleteCustomer(int id)
   {
      var customer = await context.Customers.FindAsync(id);
      if (customer == null)
      {
         return NotFound();
      }

      context.Customers.Remove(customer);
      await context.SaveChangesAsync();

      return NoContent();
   }

   private bool CustomerExists(int id)
   {
      return context.Customers.Any(customer => customer.Id == id);
   }
}