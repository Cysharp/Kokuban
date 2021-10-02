using Kokuban.AnsiEscape;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kokuban
{
    public partial class Chalk
    {
        /// <summary>
        /// Creates a new instance of AnsiStyle with options.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static AnsiStyle Create(KokubanOptions options)
            => new AnsiStyle().WithKokubanOptions(options);

        public static AnsiStyle Foreground(KokubanColor color)
            => new AnsiStyle().Foreground(color);
        public static AnsiStyle Background(KokubanColor color)
            => new AnsiStyle().Background(color);
        public static AnsiStyle Foreground(byte color)
            => new AnsiStyle().Foreground(color);
        public static AnsiStyle Background(byte color)
            => new AnsiStyle().Background(color);
        public static AnsiStyle Foreground(byte r, byte g, byte b)
            => new AnsiStyle().Foreground(r, g, b);
        public static AnsiStyle Background(byte r, byte g, byte b)
            => new AnsiStyle().Background(r, g, b);
        public static AnsiStyle Rgb(byte r, byte g, byte b)
            => new AnsiStyle().Foreground(r, g, b);
        public static AnsiStyle BgRgb(byte r, byte g, byte b)
            => new AnsiStyle().Background(r, g, b);
    }
}
