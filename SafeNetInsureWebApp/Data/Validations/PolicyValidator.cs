using FluentValidation;
using SafeNetInsureWebApp.Models;

namespace SafeNetInsureWebApp.Data.Validations;

public class PolicyValidator : AbstractValidator<Policy>
{
    public PolicyValidator()
    {
        RuleFor(policy => policy.EndDate)
            .NotEmpty()
            .WithMessage("Имя не может быть пустым.")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("Дата конца страхования должна быть больше начала действия этого же страхования!");
        RuleFor(policy => policy.ConditionPolicyIdConditionPolicies)
            .NotEmpty()
            .WithMessage("Выберите условия страхования!");
        RuleFor(policy => policy.ObjectInsuranceIdObjectInsurances)
            .NotEmpty()
            .WithMessage("Установите объекты страхования!");

        RuleFor(p => p.InsuranceAmout)
            .GreaterThanOrEqualTo(1000)
            .WithMessage(
                "Сумма выплат не может быть отрицательной, пожалуйста введите большую сумму страхования или уберите несколько условий страхования для экономии на страховке!");
    }
}