using FluentValidation;
using ToDoApp.API.Models.DTOs;

namespace ToDoApp.API.Validators
{
    public class UserLoginDTOValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDTOValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
