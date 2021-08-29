using Kokuban.AnsiEscape;
using Kokuban.Internal;

namespace Kokuban
{
    public static class KokubanExtensions
    {
        public static string ToStyledString(this string value, AnsiStyleBuilder styles)
        {
            return (styles + value);
        }
    }
}
