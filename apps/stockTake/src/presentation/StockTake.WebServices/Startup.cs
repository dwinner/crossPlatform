using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;
using StockTake.App;
using StockTake.App.Common.Interfaces;
using StockTake.Infrastructure;
using StockTake.Infrastructure.Persistence;
using StockTake.WebServices.Filters;
using StockTake.WebServices.Services;

namespace StockTake.WebServices;

public class Startup
{
   public Startup(IConfiguration configuration) => Configuration = configuration;

   public IConfiguration Configuration { get; }

   // This method gets called by the runtime. Use this method to add services to the container.
   public void ConfigureServices(IServiceCollection services)
   {
      services.AddApplication();
      services.AddInfrastructure(Configuration);

      services.AddDatabaseDeveloperPageExceptionFilter();

      services.AddSingleton<ICurrentUserService, CurrentUserService>();

      services.AddHttpContextAccessor();

      services.AddHealthChecks()
         .AddDbContextCheck<ApplicationDbContext>();

      services.AddControllersWithViews(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>())
         .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

      services.AddRazorPages();

      // Customise default API behaviour
      services.Configure<ApiBehaviorOptions>(options =>
         options.SuppressModelStateInvalidFilter = true);

      services.AddOpenApiDocument(configure =>
      {
         configure.Title = "MauiStockTake API";
         configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
         {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Type into the textbox: Bearer {your JWT token}."
         });

         configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
      });
   }

   // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
   public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
   {
      if (env.IsDevelopment())
      {
         app.UseDeveloperExceptionPage();
         app.UseMigrationsEndPoint();
      }
      else
      {
         app.UseExceptionHandler("/Error");
         // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
         app.UseHsts();
      }

      app.UseHealthChecks("/health");
      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseSwaggerUi(settings =>
      {
         settings.Path = "/api";
         settings.DocumentPath = "/api/specification.json";
      });

      app.UseRouting();
      app.UseAuthentication();
      app.UseIdentityServer();
      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
         endpoints.MapControllerRoute(
            "default",
            "{controller}/{action=Index}/{id?}");
         endpoints.MapRazorPages();
      });
   }
}