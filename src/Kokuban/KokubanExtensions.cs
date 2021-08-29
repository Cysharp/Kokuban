using Kokuban.AnsiEscape;
using Kokuban.Internal;

namespace Kokuban
{
    public static class KokubanExtensions
    {
        /// <summary>
        /// Create a styled string from the string and the specified styles.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="styles"></param>
        /// <returns></returns>
        public static AnsiStyledString ToStyledString(this string value, AnsiStyle styles)
        {
            return (styles + value);
        }
    }
}
