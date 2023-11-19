using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SafeNetInsureWebApp.Models;

[Table("ObjectInsurance")]
public partial class ObjectInsurance
{
    [Key] [Column("idObjectInsurance")] public int IdObjectInsurance { get; set; }

    /// <summary>
    ///     Наименование объекта страхования.
    /// </summary>
    [Column("title")]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    /// <summary>
    ///     Описание объекта страхования.
    /// </summary>
    [Column("description", TypeName = "text")]
    public string Description { get; set; } = null!;

    /// <summary>
    ///     Данные объекта страхования.
    /// </summary>
    [Column("dataObjectInsurance", TypeName = "json")]
    public string DataObjectInsurance { get; set; } = JsonConvert.SerializeObject("Описание объекта страхования JSON!");

    [ForeignKey("ObjectInsuranceIdObjectInsurance")]
    [InverseProperty("ObjectInsuranceIdObjectInsurances")]
    public virtual ICollection<Policy> PolicyIdPolicies { get; set; } = new List<Policy>();
}