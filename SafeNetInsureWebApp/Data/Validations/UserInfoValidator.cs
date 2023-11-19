using FluentValidation;
using SafeNetInsureWebApp.Models;

namespace SafeNetInsureWebApp.Data.Validations;

public class UserInfoValidator : AbstractValidator<UserInfo>
{
    public UserInfoValidator()
    {
        RuleFor(userInfo => userInfo.FirstName)
            .NotEmpty()
            .WithMessage("Имя не может быть пустым.");

        RuleFor(userInfo => userInfo.LastName)
            .NotEmpty()
            .WithMessage("Фамилия не может быть пустой.");

        RuleFor(userInfo => userInfo.BirthDate)
            .Must(BeAValidDate)
            .WithMessage("Некорректная дата рождения.")
            .Must(BeAtLeast18YearsOld)
            .WithMessage("Пользователь должен быть старше 18 лет.")
            .Must(BeYoungerThan100Years)
            .WithMessage("Пользователь должен быть младше 100 лет.");

        RuleFor(userInfo => userInfo.Gender)
            .IsInEnum()
            .WithMessage("Некорректный пол.");

        RuleFor(userInfo => userInfo.Adress)
            .NotEmpty()
            .WithMessage("Адрес не может быть пустым.");

        RuleFor(userInfo => userInfo.PhoneNumber)
            .NotEmpty()
            .WithMessage("Номер телефона не может быть пустым.");
    }

    private bool BeAValidDate(DateTime date)
    {
        return date <= DateTime.Now;
    }

    private bool BeAtLeast18YearsOld(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;

        if (birthDate > today.AddYears(-age))
            age--;

        return age >= 18;
    }

    private bool BeYoungerThan100Years(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;

        if (birthDate > today.AddYears(-age))
            age--;

        return age < 100;
    }
}