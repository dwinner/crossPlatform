using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StockTake.App.Common.Behaviours;

namespace StockTake.App;

public static class DependencyInjection
{
   public static IServiceCollection AddApplication(this IServiceCollection services)
   {
      var executingAssembly = Assembly.GetExecutingAssembly();
      services.AddAutoMapper(cfg => cfg.AddMaps(executingAssembly));
      services.AddValidatorsFromAssembly(executingAssembly);
      services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(executingAssembly));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

      return services;
   }
}