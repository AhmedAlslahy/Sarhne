

using Sarhne.DAL.Enums;

namespace Sarhne.BLL.Abstraction
{
    public class Errors
    {
        public record Error(string Code, string Description, ErrorType? StatusCode)
        {
            public static readonly Error None = new Error(string.Empty, string.Empty, ErrorType.None);
        }
    }
}
