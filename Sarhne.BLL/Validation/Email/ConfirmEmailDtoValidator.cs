using FluentValidation;
using Sarhne.BLL.DTOs.Email;

namespace Sarhne.BLL.Validation.Email
{
    public class ConfirmEmailDtoValidator : AbstractValidator<ConfirmEmailDto>
    {
        public ConfirmEmailDtoValidator()
        {       
            RuleFor(x => x.OTP)
            .NotEmpty()
            .WithMessage("OTP is required.")
            .Matches(@"^\d{6}$");
        }
    }
}
