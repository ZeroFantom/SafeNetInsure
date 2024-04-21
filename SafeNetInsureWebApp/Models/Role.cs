using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeNetInsureWebApp.Models;

[Table("Role")]
public partial class Role
{
    [Key] [Column("idRole")] public int IdRole { get; set; }

    [Column("title")] [StringLength(30)] public string Title { get; set; } = null!;

    [Column("description", TypeName = "text")]
    public string Description { get; set; } = null!;

    [InverseProperty("RoleIdRoleNavigation")]
    public virtual ICollection<RoleHasUser> RoleHasUsers { get; set; } = new List<RoleHasUser>();
}