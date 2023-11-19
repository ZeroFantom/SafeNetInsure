using System.ComponentModel.DataAnnotations.Schema;
using SafeNetInsureWebApp.Data.Component;

namespace SafeNetInsureWebApp.Models;

public partial class Role : IModelData
{
    [NotMapped]
    public int Id
    {
        get => IdRole;
        set => IdRole = value;
    }
}