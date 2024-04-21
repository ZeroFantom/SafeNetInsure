using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using SafeNetInsureWebApp.Data.Component;

namespace SafeNetInsureWebApp.Models;

public partial class UserInfo : IModelData
{
    public enum Sex
    {
        Male,
        Female,
        Other
    }

    /// <summary>
    ///     Дата рождения пользователя.
    /// </summary>
    [Column("birthDate", TypeName = "datetime")]
    public DateTime BirthDate { get; set; } = DateTime.Now.AddYears(-18);

    /// <summary>
    ///     Пол пользователя.
    /// </summary>
    [Column("gender", TypeName = "enum('Male','Female','Other')")]
    public Sex Gender { get; set; }

    [JsonIgnore]
    [ForeignKey("UserIdUser")]
    [InverseProperty("UserInfo")]
    public virtual User UserIdUserNavigation { get; set; } = null!;

    [NotMapped]
    public int Id
    {
        get => IdUserInfo;
        set => IdUserInfo = value;
    }
}