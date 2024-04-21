using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SafeNetInsureWebApp.Models;

[PrimaryKey("UserIdUser", "RoleIdRole")]
[Table("Role_has_User")]
[Index("RoleIdRole", Name = "fk_Role_has_User_Role1_idx")]
[Index("UserIdUser", Name = "fk_Role_has_User_User1_idx", IsUnique = true)]
public partial class RoleHasUser
{
    [Key] [Column("User_idUser")] public int UserIdUser { get; set; }

    [Key] [Column("Role_idRole")] public int RoleIdRole { get; set; }

    [ForeignKey("RoleIdRole")]
    [InverseProperty("RoleHasUsers")]
    public virtual Role RoleIdRoleNavigation { get; set; } = null!;

    [ForeignKey("UserIdUser")]
    [InverseProperty("RoleHasUser")]
    public virtual User UserIdUserNavigation { get; set; } = null!;
}