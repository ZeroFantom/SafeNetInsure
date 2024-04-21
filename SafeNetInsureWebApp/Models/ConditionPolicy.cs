using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeNetInsureWebApp.Models;

[Table("ConditionPolicy")]
public partial class ConditionPolicy
{
    [Key] [Column("idConditionPolicy")] public int IdConditionPolicy { get; set; }

    /// <summary>
    ///     Условие страхования.
    /// </summary>
    [Column("title")]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    /// <summary>
    ///     Текст условия страхования (подробно).
    /// </summary>
    [Column("condition", TypeName = "text")]
    public string Condition { get; set; } = null!;

    [ForeignKey("ConditionPolicyIdConditionPolicy")]
    [InverseProperty("ConditionPolicyIdConditionPolicies")]
    public virtual ICollection<Policy> PolicyIdPolicies { get; set; } = new List<Policy>();
}