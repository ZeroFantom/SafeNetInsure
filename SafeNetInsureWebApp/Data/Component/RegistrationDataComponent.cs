using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SafeNetInsureWebApp.Data.Services;
using SafeNetInsureWebApp.Models;

namespace SafeNetInsureWebApp.Data.Component;

public class RegistrationDataComponent : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; }

    [Inject] public ServiceAdministrationData ServiceAdministrationData { get; set; }

    [Inject] public ServiceDataUser ServiceDataUser { get; set; }

    [Inject] public ProtectedSessionStorage ProtectedSessionStore { get; set; }

    [Inject] public IHttpContextAccessor ContextAccessor { get; set; }

    public Blazorise.Validations Validator { get; set; }
    public User Client { get; set; } = new();

    public async Task Registration()
    {
        if (await Validator.ValidateAll())
        {
            await ServiceAdministrationData.Registration(Client);
            await ProtectedSessionStore.SetAsync("UserId", Client.IdUser.ToString());
            await OnInitializedAsync();
        }
    }

    protected void Callback(DateTime? obj)
    {
        Client.UserInfo.BirthDate = obj ?? DateTime.Now;
    }

    protected override async Task OnInitializedAsync()
    {
        await ServiceAdministrationData.GetRoles();
        if (Client.IdUser != 0)
        {
            var hasClaim = await ProtectedSessionStore.GetAsync<string>("UserId");
            if (hasClaim.Value != null)
                NavigationManager.NavigateTo($"/?userId={BCrypt.Net.BCrypt.HashPassword(hasClaim.Value)}", true);
        }

        await base.OnInitializedAsync();
    }
}