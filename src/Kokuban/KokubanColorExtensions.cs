using System;

namespace Kokuban
{
    public static class KokubanColorExtensions
    {
        public static (int Foreground, int Background) ToAnsiColor(this KokubanColor color)
        {
            return ((int)color, (int)color + 10);
        }

        public static KokubanColor ToKokubanColor(this ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Black => KokubanColor.Black,
                ConsoleColor.DarkBlue => KokubanColor.Blue,
                ConsoleColor.DarkGreen => KokubanColor.Green,
                ConsoleColor.DarkCyan => KokubanColor.Cyan,
                ConsoleColor.DarkRed => KokubanColor.Red,
                ConsoleColor.DarkMagenta => KokubanColor.Magenta,
                ConsoleColor.DarkYellow => KokubanColor.Yellow,
                ConsoleColor.Gray => KokubanColor.White,
                ConsoleColor.DarkGray => KokubanColor.Gray,
                ConsoleColor.Blue => KokubanColor.BrightBlue,
                ConsoleColor.Green => KokubanColor.BrightGreen,
                ConsoleColor.Cyan => KokubanColor.BrightCyan,
                ConsoleColor.Red => KokubanColor.BrightRed,
                ConsoleColor.Magenta => KokubanColor.BrightMagenta,
                ConsoleColor.Yellow => KokubanColor.BrightYellow,
                ConsoleColor.White => KokubanColor.BrightWhite,
                _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
            };
        }
    }
}