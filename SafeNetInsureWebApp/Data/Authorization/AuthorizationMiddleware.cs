using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using SafeNetInsureWebApp.Context;
using static SafeNetInsureWebApp.Data.Authorization.ExpansionClaimsPrincipal;

namespace SafeNetInsureWebApp.Data.Authorization;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public AuthorizationMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await using (var scope = _serviceProvider.CreateAsyncScope())
        {
            if (context.Request.Query.TryGetValue("logout", out var logoutTrue))
            {
                await context.SignOutAsync();
            }
            else
            {
                if (context.Request.Query.TryGetValue("userId", out var userId))
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<InsureDbContext>();

                    var user = (await dbContext.Users
                            .Include(x => x.UserInfo)
                            .Include(x => x.RoleHasUser)
                            .ThenInclude(x => x.RoleIdRoleNavigation)
                            .AsTracking().ToListAsync())
                        .FirstOrDefault(x =>
                            BCrypt.Net.BCrypt.Verify(x.IdUser.ToString(), userId.First()));

                    if (user != null)
                    {
                        var identity = CreateUserClaimIdentityIssuer(user);
                        await context.SignInAsync(new ClaimsPrincipal(identity!));
                    }
                }
            }
        }

        await _next(context.SetIssuer("SafeNet"));
    }
}