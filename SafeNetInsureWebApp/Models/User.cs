using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SafeNetInsureWebApp.Models;

[Table("User")]
[Index("Email", Name = "email_UNIQUE", IsUnique = true)]
public partial class User
{
    [Key] [Column("idUser")] public int IdUser { get; set; }

    /// <summary>
    ///     Почта пользователя.
    /// </summary>
    [Column("email")]
    [StringLength(318)]
    public string Email { get; set; } = null!;

    /// <summary>
    ///     Пароль пользователя.
    /// </summary>
    [Column("password")]
    [StringLength(60)]
    public string Password { get; set; } = null!;
}