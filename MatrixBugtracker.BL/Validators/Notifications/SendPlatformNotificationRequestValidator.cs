using FluentValidation;
using MatrixBugtracker.BL.DTOs.Notifications;

namespace MatrixBugtracker.BL.Validators.Notifications
{
    public class SendPlatformNotificationRequestValidator : AbstractValidator<SendPlatformNotificationRequestDTO>
    {
        public SendPlatformNotificationRequestValidator()
        {
            RuleFor(p => p.Text).NotEmpty().Length(5, 1024);
        }
    }
}
