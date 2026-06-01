using Sarhne.DAL.Enums;
using static Sarhne.BLL.Abstraction.Errors;

namespace Sarhne.BLL.Errors
{
    public class AuthErrors
    {
        public static Error NotFound
         = new Error("Auth.NotFound", "Auth not found", ErrorType.NotFound);

        public static Error InvalidData
           = new Error("Auth.InvalidData", "Invalid Auth data", ErrorType.BadRequest);
    }
}
