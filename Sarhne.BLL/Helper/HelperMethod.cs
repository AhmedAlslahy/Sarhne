
using Sarhne.DAL.Enums;
using static Sarhne.BLL.Abstraction.Errors;

namespace Sarhne.BLL.Helper
{
   public static class HelperMethod
    {
        public static string GetPreview(string message, int maxLength = 50)
        {
            if (string.IsNullOrEmpty(message))
                return message;

            if (message.Length <= maxLength)
                return message;

            var trimmed = message.Substring(0, maxLength);
            var lastSpace = trimmed.LastIndexOf(' ');

            if (lastSpace > 0)
                trimmed = trimmed.Substring(0, lastSpace);

            return trimmed + "...";
        }


        public static class ValidationHelper
        {
            public static Error? Validate(FluentValidation.Results.ValidationResult result)
            {
                if (result.IsValid)
                    return null;

                return new Error(
                    "ValidationError",
                    string.Join(", ", result.Errors.Select(x => x.ErrorMessage)),
                    ErrorType.BadRequest);
            }
        }
    }
}
