using System.ComponentModel.DataAnnotations.Schema;
using SafeNetInsureWebApp.Data.Component;

namespace SafeNetInsureWebApp.Models;

public partial class RoleHasUser : IModelDataHas
{
    [NotMapped]
    public int IdCurrent
    {
        get => UserIdUser;
        set => UserIdUser = value;
    }

    [NotMapped]
    public int IdSubmit
    {
        get => RoleIdRole;
        set => RoleIdRole = value;
    }
}