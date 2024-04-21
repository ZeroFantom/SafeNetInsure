using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using SafeNetInsureWebApp.Data.Component;

namespace SafeNetInsureWebApp.Models;

public partial class User : IModelData
{
    [InverseProperty("UserIdUserNavigation")]
    public virtual ICollection<Policy> Policies { get; set; } = new ObservableCollection<Policy>();

    [JsonIgnore]
    [InverseProperty("UserIdUserNavigation")]
    public virtual RoleHasUser RoleHasUser { get; set; } = new();

    [InverseProperty("UserIdUserNavigation")]
    public virtual UserInfo UserInfo { get; set; } = new();

    [NotMapped]
    public int Id
    {
        get => IdUser;
        set => IdUser = value;
    }
}