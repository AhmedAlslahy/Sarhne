using FluentValidation;
using Sarhne.BLL.DTOs.Notification;

namespace Sarhne.BLL.Validation.Notification
{
    public class CreateNotificationDtoValidator : AbstractValidator<CreateNotificationDto>
    {
        public CreateNotificationDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(50)
                .WithMessage("Title must not exceed 50 characters.");

            RuleFor(x => x.Body)
                .MaximumLength(150)
                .When(x => !string.IsNullOrWhiteSpace(x.Body))
                .WithMessage("Body must not exceed 150 characters.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");
        }
    }
}
