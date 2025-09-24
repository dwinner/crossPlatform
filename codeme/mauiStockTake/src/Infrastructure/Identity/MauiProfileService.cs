using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

namespace MauiStockTake.Infrastructure.Identity;

public class MauiProfileService : IProfileService
{
    public Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var claim = new Claim("Username", context.Subject.Identity?.Name?? "bah-bow");

        context.IssuedClaims.Add(claim);

        return Task.CompletedTask;
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
        return Task.CompletedTask;
    }
}
