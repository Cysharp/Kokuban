using System;
using Kokuban.Internal;

namespace Kokuban.AnsiEscape
{
    [Flags]
    internal enum SgrParameterFlag : byte
    {
        Underilne = 1 << 0,
        Overline  = 1 << 1,
        Italic = 1 << 2,
        Bold = 1 << 3,
        Dim = 1 << 4,
        Inverse = 1 << 5,
    }

    public readonly partial struct AnsiStyle : IAnsiStyle
    {
        private readonly KokubanOptions? _options;
        private readonly KokubanColorValue? _fgColor;
        private readonly KokubanColorValue? _bgColor;
        private readonly SgrParameterFlag _flag;

        KokubanOptions? IAnsiStyle.Options => _options;
        KokubanColorValue? IAnsiStyle.Foreground => _fgColor;
        KokubanColorValue? IAnsiStyle.Background => _bgColor;
        bool IAnsiStyle.Underline => _flag.HasFlag(SgrParameterFlag.Underilne);
        bool IAnsiStyle.Overline => _flag.HasFlag(SgrParameterFlag.Overline);
        bool IAnsiStyle.Italic => _flag.HasFlag(SgrParameterFlag.Italic);
        bool IAnsiStyle.Bold => _flag.HasFlag(SgrParameterFlag.Bold);
        bool IAnsiStyle.Dim => _flag.HasFlag(SgrParameterFlag.Dim);
        bool IAnsiStyle.Inverse => _flag.HasFlag(SgrParameterFlag.Inverse);

        public AnsiStyle Underline => WithSgrParameterFlag(SgrParameterFlag.Underilne);
        public AnsiStyle Overline => WithSgrParameterFlag(SgrParameterFlag.Overline);
        public AnsiStyle Italic => WithSgrParameterFlag(SgrParameterFlag.Italic);
        public AnsiStyle Bold => WithSgrParameterFlag(SgrParameterFlag.Bold);
        public AnsiStyle Dim => WithSgrParameterFlag(SgrParameterFlag.Dim);
        public AnsiStyle Faint => WithSgrParameterFlag(SgrParameterFlag.Dim);
        public AnsiStyle Inverse => WithSgrParameterFlag(SgrParameterFlag.Inverse);

        // for LINQPad
        private object ToDump() => $"{nameof(AnsiStyle)}: Flag={_flag}";

        internal AnsiStyle(KokubanOptions? options, KokubanColorValue? fgColor, KokubanColorValue? bgColor, SgrParameterFlag flag)
        {
            _options = options;
            _fgColor = fgColor;
            _bgColor = bgColor;
            _flag = flag;
        }

        internal AnsiStyle WithSgrParameterFlag(SgrParameterFlag flag)
            => new AnsiStyle(_options, _fgColor, _bgColor, flag | _flag);
        internal AnsiStyle WithKokubanOptions(KokubanOptions options)
            => new AnsiStyle(options, _fgColor, _bgColor, _flag);
        
        public AnsiStyle Foreground(KokubanColorValue color)
            => new AnsiStyle(_options, color, _bgColor, _flag);
        public AnsiStyle Background(KokubanColorValue color)
            => new AnsiStyle(_options, _fgColor, color, _flag);
        public AnsiStyle Foreground(KokubanColor color)
            => Foreground(KokubanColorValue.FromBasic((byte)color));
        public AnsiStyle Background(KokubanColor color)
            => Background(KokubanColorValue.FromBasic((byte)color));
        public AnsiStyle Foreground(byte color)
            => Foreground(KokubanColorValue.FromBasic(color));
        public AnsiStyle Background(byte color)
            => Background(KokubanColorValue.FromBasic(color));
        public AnsiStyle Foreground(byte r, byte g, byte b)
            => Foreground(KokubanColorValue.FromRgb(r, g, b));
        public AnsiStyle Background(byte r, byte g, byte b)
            => Background(KokubanColorValue.FromRgb(r, g, b));
        public AnsiStyle Rgb(byte r, byte g, byte b)
            => Foreground(KokubanColorValue.FromRgb(r, g, b));
        public AnsiStyle BgRgb(byte r, byte g, byte b)
            => Background(KokubanColorValue.FromRgb(r, g, b));
        public AnsiStyle Ansi256(byte index)
            => Foreground(KokubanColorValue.FromIndex(index));
        public AnsiStyle BgAnsi256(byte index)
            => Background(KokubanColorValue.FromIndex(index));

        public AnsiStyledString this[string value] => new AnsiStyledString(this, value);
        public AnsiStyledString this[AnsiStyledString value] => new AnsiStyledString(this, value);

        public static AnsiStyledString operator +(AnsiStyle a, string b)
            => new AnsiStyledString(a, b);
        public static AnsiStyledString operator +(AnsiStyle a, AnsiStyledString b)
            => new AnsiStyledString(a, b);

        public static AnsiStyledString operator +(string a, AnsiStyle b)
            => new AnsiStyledString(null, a, b);
        public static AnsiStyledString operator +(AnsiStyledString a, AnsiStyle b)
            => new AnsiStyledString(null, a, b);

        public AnsiStyledString Render(string value)
            => new AnsiStyledString(this, value);
        public AnsiStyledString Render(AnsiStyledString value)
            => new AnsiStyledString(this, value);
    }
}
