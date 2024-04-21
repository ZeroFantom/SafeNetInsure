using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SafeNetInsureWebApp.Models;

[Table("UserInfo")]
[Index("UserIdUser", Name = "fk_UserInfo_User1_idx", IsUnique = true)]
public partial class UserInfo
{
    [Key] [Column("idUserInfo")] public int IdUserInfo { get; set; }

    /// <summary>
    ///     Имя пользователя.
    /// </summary>
    [Column("firstName")]
    [StringLength(45)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    ///     Фамилия пользователя.
    /// </summary>
    [Column("lastName")]
    [StringLength(45)]
    public string LastName { get; set; } = null!;

    /// <summary>
    ///     Адресс пользователя.
    /// </summary>
    [Column("adress", TypeName = "text")]
    public string Adress { get; set; } = null!;

    /// <summary>
    ///     Номер телефона пользователя.
    /// </summary>
    [Column("phoneNumber")]
    [StringLength(30)]
    public string PhoneNumber { get; set; } = null!;

    [Column("User_idUser")] public int UserIdUser { get; set; }
}