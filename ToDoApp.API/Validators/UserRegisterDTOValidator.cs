using FluentValidation;
using ToDoApp.API.Models.DTOs;

namespace ToDoApp.API.Validators
{
    // Validator för UserRegisterDto som definierar valideringsregler för registrering
    public class UserRegisterDTOValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDTOValidator() //konstruktior där valideringsregler sätts upp
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
        }
    }
}
