using Microsoft.AspNetCore.Identity;

namespace AuthSvc;

public class User : IdentityUser
{
   public DateOnly BirthDate { get; set; }
}