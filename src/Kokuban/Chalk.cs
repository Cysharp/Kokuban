using Kokuban.AnsiEscape;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kokuban
{
    public partial class Chalk
    {
        /// <summary>
        /// Creates a new instance of Chalk with options.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static AnsiStyleBuilder Create(KokubanOptions options)
            => new AnsiStyleBuilder().WithKokubanOptions(options);

        public static AnsiStyleBuilder Foreground(KokubanColor color)
            => new AnsiStyleBuilder().Foreground(color);
        public static AnsiStyleBuilder Background(KokubanColor color)
            => new AnsiStyleBuilder().Background(color);
        public static AnsiStyleBuilder Foreground(byte color)
            => new AnsiStyleBuilder().Foreground(color);
        public static AnsiStyleBuilder Background(byte color)
            => new AnsiStyleBuilder().Background(color);
        public static AnsiStyleBuilder Rgb(byte r, byte g, byte b)
            => new AnsiStyleBuilder().Rgb(r, g, b);
        public static AnsiStyleBuilder BgRgb(byte r, byte g, byte b)
            => new AnsiStyleBuilder().BgRgb(r, g, b);
    }
}
