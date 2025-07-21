using System.Security.Claims;
using c5_AuthenticationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
   swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
   {
      Title = "WebAPI API",
      Version = "v1"
   });

   swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
   {
      Type = SecuritySchemeType.Http,
      Name = "Bearer",
      Scheme = "bearer",
      BearerFormat = "Bearer",
      In = ParameterLocation.Header
   });

   swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
   {
      {
         new OpenApiSecurityScheme
         {
            Reference = new OpenApiReference
            {
               Type = ReferenceType.SecurityScheme,
               Id = "Bearer"
            }
         },
         Array.Empty<string>()
      }
   });
});


builder.Services
   .AddAuthentication()
   .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services
   .AddIdentityCore<User>()
   .AddRoles<IdentityRole>()
   .AddEntityFrameworkStores<ApplicationDbContext>()
   .AddApiEndpoints()
   .AddDefaultTokenProviders();

builder.Services
   .AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlite("Data Source=mydatabase.db"));

builder.Services.AddAuthorization(options =>
{
   options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
   options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapIdentityApi<User>();

app.MapDelete("/users/delete/{email}", async (
   string email,
   UserManager<User> userManager,
   HttpContext _) =>
{
   var user = await userManager.FindByEmailAsync(email);
   if (user == null)
   {
      return Results.NotFound();
   }

   var result = await userManager.DeleteAsync(user);
   if (result.Succeeded)
   {
      return Results.Ok();
   }

   return Results.Problem("Failed to delete the user.");
}).RequireAuthorization("AdminPolicy");

app.MapGet("/users/candelete", async (
   HttpContext httpContext,
   IAuthorizationService authSvc) =>
{
   var authResult = await authSvc.AuthorizeAsync(httpContext.User, "AdminPolicy");
   return Results.Ok(authResult);
});

app.MapGet("/users", async (
   UserManager<User> userManager) =>
{
   var users = await userManager.Users
      .Select(user => new
      {
         user.Email,
         user.BirthDate
      })
      .ToListAsync();

   return Results.Ok(users);
}).RequireAuthorization();

app.MapGet("/me", async (ClaimsPrincipal principal, UserManager<User> userManager) =>
{
   var userEmail = principal.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
   var currentUser = await userManager.FindByEmailAsync(userEmail);
   return Results.Ok(new
   {
      currentUser.Email,
      currentUser.BirthDate
   });
}).RequireAuthorization();

using (var scope = app.Services.CreateScope())
{
   var services = scope.ServiceProvider;
   var dbCtx = services.GetRequiredService<ApplicationDbContext>();
   dbCtx.Database.EnsureCreated();
   var userManager = services.GetRequiredService<UserManager<User>>();
   var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
   await userManager.CreateUserWithRoleAsync(
      roleManager,
      "admin@cookbook.com",
      "123Password123!",
      "Admin");
   for (var i = 0; i < 10; i++)
   {
      await userManager.CreateUserWithRoleAsync(
         roleManager,
         $"user{i}@cookbook.com",
         "123Password123!",
         "User");
   }
}

app.Run();