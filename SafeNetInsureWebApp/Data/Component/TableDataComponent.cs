using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using SafeNetInsureWebApp.Data.Services;
using SafeNetInsureWebApp.Models;
using static SafeNetInsureWebApp.Data.Services.ServiceAdministrationData;
using static SafeNetInsureWebApp.Data.Services.ServiceDataPolicy;

namespace SafeNetInsureWebApp.Data.Component;

[Authorize]
public class TableDataComponent : ComponentBase
{
    [Inject] public ServiceDataUser ServiceDataUser { get; set; }
    [Inject] public ServiceDataPolicy ServiceDataPolicy { get; set; }
    [Inject] public ServiceAdministrationData ServiceAdministrationData { get; set; }

    [Inject] public NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var segment = string.Join("", NavigationManager.ToAbsoluteUri(NavigationManager.Uri).Segments.TakeLast(2));
        switch (segment)
        {
            case "user_service/administration":
                await ServiceAdministrationData.GetUsers();
                await ServiceAdministrationData.GetUserInfos();
                break;
            case "user_service/policy":
                await ServiceDataPolicy.GetPolicies();
                await ServiceDataPolicy.GetCPolicies();
                break;
            case "user-role_service/administration":
                await ServiceAdministrationData.GetRoles();
                await ServiceAdministrationData.GetRoleHasUser();
                break;
            case "policy_service/administration":
                await ServiceDataPolicy.GetPolicies();
                await ServiceDataPolicy.GetCPolicies();
                break;
        }

        await base.OnInitializedAsync();
    }

    protected async Task PostUserPolicy()
    {
        if (ServiceDataUser.Policy.ObjectInsuranceIdObjectInsurances.Count == 0 ||
            ServiceDataUser.Policy.ConditionPolicyIdConditionPolicies.Count == 0)
        {
            AlertShow(TimeSpan.FromSeconds(10), "Ошибка валидации",
                "Нельзя отправлять форму без указанных условий страхования и объектов страхования!", Color.Warning);
        }
        else
        {
            ServiceDataUser.Policy.UserIdUser = ServiceDataUser.User.IdUser;
            await InsertModel(ServiceDataUser.Policy);
            await OnInitializedAsync();
        }
    }

    protected async Task DeleteModel<T>(T model)
    {
        await ServiceDataUser.DeleteModel(model);
    }

    protected async Task UpdateModel<T>(SavedRowItem<T, Dictionary<string, object>> obj)
    {
        await ServiceDataUser.UpdateModel();
    }

    protected async Task InsertModel<T>(SavedRowItem<T, Dictionary<string, object>> obj) where T : class, IModelData
    {
        var objNewDb = await ServiceDataUser.InsertModel(obj.NewItem);
        switch (objNewDb)
        {
            case ConditionPolicy newDb:
                ChangedCollection(ConditionPolicies, newDb);
                break;
            case ObjectInsurance newDb:
                ChangedCollection(ServiceDataUser.Policy.ObjectInsuranceIdObjectInsurances, newDb);
                break;
            case Policy newDb:
                ChangedCollection(Policies, newDb);
                break;
            case Role newDb:
                ChangedCollection(Roles, newDb);
                break;
            case User newDb:
                ChangedCollection(Users, newDb);
                break;
            case UserInfo newDb:
                ChangedCollection(UserInfos, newDb);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected async Task InsertModel<T>(T obj) where T : class, IModelData
    {
        var objNewDb = await ServiceDataUser.InsertModel(obj);
        switch (objNewDb)
        {
            case ConditionPolicy newDb:
                ChangedCollection(ConditionPolicies, newDb);
                break;
            case ObjectInsurance newDb:
                ChangedCollection(ServiceDataUser.Policy.ObjectInsuranceIdObjectInsurances, newDb);
                break;
            case Policy newDb:
                ChangedCollection(Policies, newDb);
                break;
            case Role newDb:
                ChangedCollection(Roles, newDb);
                break;
            case User newDb:
                ChangedCollection(Users, newDb);
                break;
            case UserInfo newDb:
                ChangedCollection(UserInfos, newDb);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static void ChangedCollection<T>(IList<T> collection, T newItem) where T : IModelData
    {
        if (collection.Any(x => x.Id == 0))
        {
            var index = collection.IndexOf(collection.First(x => x.Id == 0));
            collection[index] = newItem;
        }
    }

    #region Обратная связь

    protected Alert AlertUser { get; set; }

    protected Color AlertColor { get; set; } = Color.Success;
    protected string AlertTitle { get; set; }
    protected string AlertDescription { get; set; }

    public void AlertShow(TimeSpan duration, string title, string description, Color color)
    {
        AlertTitle = title;
        AlertDescription = description;
        AlertColor = color;

        AlertUser.Show();

        Task.Delay(duration).ContinueWith(_ =>
        {
            AlertUser.Hide();
            StateHasChanged();
        });
    }

    #endregion
}

public interface IModelData
{
    public int Id { get; set; }
}

public interface IModelDataHas
{
    public int IdCurrent { get; set; }
    public int IdSubmit { get; set; }
}