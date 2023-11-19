using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using SafeNetInsureWebApp.Data.Component;

namespace SafeNetInsureWebApp.Models;

public partial class Policy : IModelData
{
    [NotMapped]
    public int Id
    {
        get => IdPolicy;
        set => IdPolicy = value;
    }

    private decimal _insureAmoutUser = 1000;

    public enum InsType
    {
        Auto,
        Life,
        Health,
        Property,
        Liability,
        Business,
        Travel,
        Other
    }

    public enum StatusInsure
    {
        Complete,
        Active
    }

    /// <summary>
    ///     Уникальный идентификатор полиса.
    /// </summary>
    [Column("policyNumber")]
    [StringLength(320)]
    public Guid PolicyNumber { get; set; } = Guid.NewGuid();

    /// <summary>
    ///     Статус страховки (выплата была произведена в полном объёме и страховка не продлена, выплата не была произведена в
    ///     полном объёме, страховка действует, страховка продлена).
    /// </summary>
    [Column("status", TypeName = "enum('Complete','Active')")]
    public StatusInsure Status { get; set; } = StatusInsure.Active;

    /// <summary>
    ///     Начало действия полиса.
    /// </summary>
    [Column("startDate", TypeName = "datetime")]
    public DateTime StartDate { get; set; } = DateTime.Now;

    private DateTime _endDateTime = DateTime.Now.AddDays(1);

    /// <summary>
    ///     Конец действия полиса.
    /// </summary>
    [Column("endDate", TypeName = "datetime")]
    public DateTime EndDate
    {
        get => _endDateTime;
        set
        {
            _endDateTime = value;
            Status = _endDateTime <= DateTime.Now ? StatusInsure.Complete : StatusInsure.Active;
            InsureAmoutUser = _insureAmoutUser;
        }
    }

    /// <summary>
    ///     Тип страхования.
    /// </summary>
    [Column("insuranceType",
        TypeName = "enum('Auto','Life','Health','Property','Liability','Business','Travel','Other')")]
    public InsType InsuranceType { get; set; } = InsType.Other;

    [NotMapped]
    [JsonIgnore]
    public decimal InsureAmoutUser
    {
        get => _insureAmoutUser;
        set
        {
            _insureAmoutUser = value;
            InsuranceAmout = Convert.ToDecimal(Convert.ToDouble(value) * 1.23) * (EndDate.Year + 1 - StartDate.Year) -
                             ConditionPolicyIdConditionPolicies.Count * 100;
        }
    }

    private List<ConditionPolicy> _conditionPolicyIdConditionPolicies = new();
    private List<ObjectInsurance> _objectInsuranceIdObjectInsurances = new();

    [ForeignKey("PolicyIdPolicy")]
    [InverseProperty("PolicyIdPolicies")]
    public List<ConditionPolicy> ConditionPolicyIdConditionPolicies
    {
        get => _conditionPolicyIdConditionPolicies;
        set
        {
            _conditionPolicyIdConditionPolicies = value;
            InsureAmoutUser = _insureAmoutUser;
        }
    }

    [ForeignKey("PolicyIdPolicy")]
    [InverseProperty("PolicyIdPolicies")]
    public List<ObjectInsurance> ObjectInsuranceIdObjectInsurances
    {
        get => _objectInsuranceIdObjectInsurances;
        set
        {
            _objectInsuranceIdObjectInsurances = value;
            InsureAmoutUser = _insureAmoutUser;
        }
    }
}