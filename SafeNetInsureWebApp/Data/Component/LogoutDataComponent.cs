using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using SafeNetInsureWebApp.Data.Authorization;

namespace SafeNetInsureWebApp.Data.Component;

public class LogoutDataComponent : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; }

    [Inject] public IHttpContextAccessor ContextAccessor { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var hasClaim = ContextAccessor.HttpContext?.User.GetClaimIssuer(ClaimTypes.Sid)?.Value;
        if (hasClaim != null)
            NavigationManager.NavigateTo($"/?userId={BCrypt.Net.BCrypt.HashPassword(hasClaim)}&logout=true", true);
        await base.OnInitializedAsync();
    }
}