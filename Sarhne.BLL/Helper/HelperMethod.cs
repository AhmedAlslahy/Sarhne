using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
