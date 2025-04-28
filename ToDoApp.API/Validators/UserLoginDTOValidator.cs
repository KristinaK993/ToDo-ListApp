using FluentValidation;
using ToDoApp.API.Models.DTOs;

namespace ToDoApp.API.Validators
{
    //validator för UserLoginDTO som ärver från AVUserLoginDto
    public class UserLoginDTOValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDTOValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required"); //regler:det får inte vara tomt
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required"); //regler:det måste skrivas in
        }
    }
}
