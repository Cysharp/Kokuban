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

    public readonly partial struct AnsiStyleBuilder : IAnsiStyleBuilder
    {
        private readonly KokubanOptions? _options;
        private readonly KokubanColorValue? _fgColor;
        private readonly KokubanColorValue? _bgColor;
        private readonly SgrParameterFlag _flag;

        KokubanOptions? IAnsiStyleBuilder.Options => _options;
        KokubanColorValue? IAnsiStyleBuilder.Foreground => _fgColor;
        KokubanColorValue? IAnsiStyleBuilder.Background => _bgColor;
        bool IAnsiStyleBuilder.Underline => _flag.HasFlag(SgrParameterFlag.Underilne);
        bool IAnsiStyleBuilder.Overline => _flag.HasFlag(SgrParameterFlag.Overline);
        bool IAnsiStyleBuilder.Italic => _flag.HasFlag(SgrParameterFlag.Italic);
        bool IAnsiStyleBuilder.Bold => _flag.HasFlag(SgrParameterFlag.Bold);
        bool IAnsiStyleBuilder.Dim => _flag.HasFlag(SgrParameterFlag.Dim);
        bool IAnsiStyleBuilder.Inverse => _flag.HasFlag(SgrParameterFlag.Inverse);

        public AnsiStyleBuilder Underline => WithSgrParameterFlag(SgrParameterFlag.Underilne);
        public AnsiStyleBuilder Overline => WithSgrParameterFlag(SgrParameterFlag.Overline);
        public AnsiStyleBuilder Italic => WithSgrParameterFlag(SgrParameterFlag.Italic);
        public AnsiStyleBuilder Bold => WithSgrParameterFlag(SgrParameterFlag.Bold);
        public AnsiStyleBuilder Dim => WithSgrParameterFlag(SgrParameterFlag.Dim);
        public AnsiStyleBuilder Faint => WithSgrParameterFlag(SgrParameterFlag.Dim);
        public AnsiStyleBuilder Inverse => WithSgrParameterFlag(SgrParameterFlag.Inverse);

        // for LINQPad
        private object ToDump() => $"{nameof(AnsiStyleBuilder)}: Flag={_flag}";

        internal AnsiStyleBuilder(KokubanOptions? options, KokubanColorValue? fgColor, KokubanColorValue? bgColor, SgrParameterFlag flag)
        {
            _options = options;
            _fgColor = fgColor;
            _bgColor = bgColor;
            _flag = flag;
        }

        internal AnsiStyleBuilder WithSgrParameterFlag(SgrParameterFlag flag)
            => new AnsiStyleBuilder(_options, _fgColor, _bgColor, flag | _flag);
        internal AnsiStyleBuilder WithKokubanOptions(KokubanOptions options)
            => new AnsiStyleBuilder(options, _fgColor, _bgColor, _flag);
        internal AnsiStyleBuilder Foreground(KokubanColorValue color)
            => new AnsiStyleBuilder(_options, color, _bgColor, _flag);
        internal AnsiStyleBuilder Background(KokubanColorValue color)
            => new AnsiStyleBuilder(_options, _fgColor, color, _flag);

        public AnsiStyleBuilder Foreground(KokubanColor color)
            => Foreground(KokubanColorValue.FromBasic((byte)color));
        public AnsiStyleBuilder Background(KokubanColor color)
            => Background(KokubanColorValue.FromBasic((byte)color));
        public AnsiStyleBuilder Foreground(byte color)
            => Foreground(KokubanColorValue.FromBasic(color));
        public AnsiStyleBuilder Background(byte color)
            => Background(KokubanColorValue.FromBasic(color));
        public AnsiStyleBuilder Rgb(byte r, byte g, byte b)
            => Foreground(KokubanColorValue.FromRgb(r, g, b));
        public AnsiStyleBuilder BgRgb(byte r, byte g, byte b)
            => Background(KokubanColorValue.FromRgb(r, g, b));
        public AnsiStyleBuilder Ansi256(byte index)
            => Foreground(KokubanColorValue.FromIndex(index));
        public AnsiStyleBuilder BgAnsi256(byte index)
            => Background(KokubanColorValue.FromIndex(index));

        public AnsiStyledString this[string value] => new AnsiStyledString(this, value);
        public AnsiStyledString this[AnsiStyledString value] => new AnsiStyledString(this, value);

        public static AnsiStyledString operator +(AnsiStyleBuilder a, string b)
            => new AnsiStyledString(a, b);
        public static AnsiStyledString operator +(AnsiStyleBuilder a, AnsiStyledString b)
            => new AnsiStyledString(a, b);

        public static AnsiStyledString operator +(string a, AnsiStyleBuilder b)
            => new AnsiStyledString(null, a, b);
        public static AnsiStyledString operator +(AnsiStyledString a, AnsiStyleBuilder b)
            => new AnsiStyledString(null, a, b);

        public AnsiStyledString Render(string value)
            => new AnsiStyledString(this, value);
        public AnsiStyledString Render(AnsiStyledString value)
            => new AnsiStyledString(this, value);
    }
}
