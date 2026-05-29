using Sarhne.DAL.Enums;
using static Sarhne.BLL.Abstraction.Errors;


namespace Sarhne.BLL.Errors
{
    public class MessageErrors
    {
        public static Error NotFound
          = new Error("Message.NotFound", "Message not found", ErrorType.NotFound);

        public static Error InvalidData
           = new Error("Message.InvalidData", "Invalid Message data", ErrorType.BadRequest);
    }
}
