using System.ComponentModel.DataAnnotations.Schema;
using SafeNetInsureWebApp.Data.Component;

namespace SafeNetInsureWebApp.Models;

public partial class ObjectInsurance : IModelData
{
    [NotMapped]
    public int Id
    {
        get => IdObjectInsurance;
        set => IdObjectInsurance = value;
    }
}