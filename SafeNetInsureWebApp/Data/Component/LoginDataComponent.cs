using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SafeNetInsureWebApp.Data.Services;
using SafeNetInsureWebApp.Models;

namespace SafeNetInsureWebApp.Data.Component;

public class LoginDataComponent : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; }

    [Inject] public ServiceAdministrationData ServiceAdministrationData { get; set; }
    [Inject] public ProtectedSessionStorage ProtectedSessionStore { get; set; }

    [Inject] public IHttpContextAccessor ContextAccessor { get; set; }


    public Blazorise.Validations Validator { get; set; }

    public User Client { get; set; } = new();

    public async Task Login()
    {
        if (await Validator.ValidateAll())
            if (await ServiceAdministrationData.Login(Client) is User client)
            {
                await ProtectedSessionStore.SetAsync("UserId", client.IdUser.ToString());
                await OnInitializedAsync();
            }
    }

    protected override async Task OnInitializedAsync()
    {
        var hasClaim = await ProtectedSessionStore.GetAsync<string>("UserId");
        if (hasClaim.Value != null)
        {
            NavigationManager.NavigateTo($"/?userId={BCrypt.Net.BCrypt.HashPassword(hasClaim.Value)}", true);
            await ProtectedSessionStore.DeleteAsync("UserId");
        }

        await base.OnInitializedAsync();
    }
}