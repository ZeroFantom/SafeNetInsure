using System.ComponentModel.DataAnnotations.Schema;
using SafeNetInsureWebApp.Data.Component;

namespace SafeNetInsureWebApp.Models;

public partial class ConditionPolicy : IModelData
{
    [NotMapped]
    public int Id
    {
        get => IdConditionPolicy;
        set => IdConditionPolicy = value;
    }
}