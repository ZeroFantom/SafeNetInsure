using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SafeNetInsureWebApp.Models;

[Table("Policy")]
[Index("UserIdUser", Name = "fk_Policy_User1_idx")]
public partial class Policy
{
    [Key] [Column("idPolicy")] public int IdPolicy { get; set; }

    /// <summary>
    ///     Сумма страхования.
    /// </summary>
    [Column("insuranceAmout")]
    public decimal InsuranceAmout { get; set; }

    [Column("User_idUser")] public int? UserIdUser { get; set; }

    [ForeignKey("UserIdUser")]
    [InverseProperty("Policies")]
    public virtual User? UserIdUserNavigation { get; set; }
}