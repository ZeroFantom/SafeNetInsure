using FluentValidation;
using SafeNetInsureWebApp.Models;

namespace SafeNetInsureWebApp.Data.Validations;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("Поле не может быть пустым.")
            .EmailAddress()
            .WithMessage("Логин должен содержать действительную почту.");

        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage("Поле не может быть пустым.")
            .Length(6, 60)
            .WithMessage("Пароль должен быть от 6 до 60 символов.")
            .Must(password => password.Any(char.IsLower))
            .WithMessage("Пароль должен содержать хотя бы одну маленькую букву.")
            .Must(password => password.Any(char.IsUpper))
            .WithMessage("Пароль должен содержать хотя бы одну большую букву.")
            .Must(password => password.Any(char.IsDigit))
            .WithMessage("Пароль должен содержать хотя бы одну цифру.")
            .Must(password => password.Any(ch => !char.IsLetterOrDigit(ch)))
            .WithMessage("Пароль должен содержать хотя бы один спецсимвол.");
    }
}