using System;
using Kokuban.Internal;

namespace Kokuban.AnsiEscape
{
    public readonly partial struct AnsiStyleBuilder : IAnsiStyleBuilder
    {
        private readonly KokubanOptions? _options;
        private readonly KokubanColorValue? _fgColor;
        private readonly KokubanColorValue? _bgColor;
        private readonly bool _underline;
        private readonly bool _overline;
        private readonly bool _italic;
        private readonly bool _bold;
        private readonly bool _dim;
        private readonly bool _inverse;

        KokubanOptions? IAnsiStyleBuilder.Options => _options;
        KokubanColorValue? IAnsiStyleBuilder.Foreground => _fgColor;
        KokubanColorValue? IAnsiStyleBuilder.Background => _bgColor;
        bool IAnsiStyleBuilder.Underline => _underline;
        bool IAnsiStyleBuilder.Overline => _overline;
        bool IAnsiStyleBuilder.Italic => _italic;
        bool IAnsiStyleBuilder.Bold => _bold;
        bool IAnsiStyleBuilder.Dim => _dim;
        bool IAnsiStyleBuilder.Inverse => _inverse;

        public AnsiStyleBuilder Underline => new AnsiStyleBuilder(_options, _fgColor, _bgColor, true, _overline, _italic, _bold, _dim, _inverse);
        public AnsiStyleBuilder Overline => new AnsiStyleBuilder(_options, _fgColor, _bgColor, _underline, true, _italic, _bold, _dim, _inverse);
        public AnsiStyleBuilder Italic => new AnsiStyleBuilder(_options, _fgColor, _bgColor, _underline, _overline, true, _bold, _dim, _inverse);
        public AnsiStyleBuilder Bold => new AnsiStyleBuilder(_options, _fgColor, _bgColor, _underline, _overline, _italic, true, _dim, _inverse);
        public AnsiStyleBuilder Dim => new AnsiStyleBuilder(_options, _fgColor, _bgColor, _underline, _overline, _italic, _bold, true, _inverse);
        public AnsiStyleBuilder Faint => new AnsiStyleBuilder(_options, _fgColor, _bgColor, _underline, _overline, _italic, _bold, true, _inverse);
        public AnsiStyleBuilder Inverse => new AnsiStyleBuilder(_options, _fgColor, _bgColor, _underline, _overline, _italic, _bold, _dim, true);

        // for LINQPad
        private object ToDump() => $"{nameof(AnsiStyleBuilder)}: Foreground={_fgColor}; Background={_bgColor}; Underline={_underline}; Overline={_overline}; Italic={_italic}; Bold={_bold}; Dim={_dim}; Inverse={_inverse}";

        internal AnsiStyleBuilder(KokubanOptions? options, KokubanColorValue? fgColor, KokubanColorValue? bgColor, bool underline, bool overline, bool italic, bool bold, bool dim, bool inverse)
        {
            _options = options;
            _fgColor = fgColor;
            _bgColor = bgColor;
            _underline = underline;
            _overline = overline;
            _italic = italic;
            _bold = bold;
            _dim = dim;
            _inverse = inverse;
        }

        internal AnsiStyleBuilder WithKokubanOptions(KokubanOptions options)
            => new AnsiStyleBuilder(options, _fgColor, _bgColor, _underline, _overline, _italic, _bold, _dim, _inverse);
        public AnsiStyleBuilder Foreground(KokubanColor color)
            => new AnsiStyleBuilder(_options, KokubanColorValue.FromBasic((byte)color), _bgColor, _underline, _overline, _italic, _bold, _dim, _inverse);
        public AnsiStyleBuilder Background(KokubanColor color)
            => new AnsiStyleBuilder(_options, _fgColor, KokubanColorValue.FromBasic((byte)color), _underline, _overline, _italic, _bold, _dim, _inverse);
        public AnsiStyleBuilder Foreground(byte color)
            => new AnsiStyleBuilder(_options, KokubanColorValue.FromBasic(color), _bgColor, _underline, _overline, _italic, _bold, _dim, _inverse);
        public AnsiStyleBuilder Background(byte color)
            => new AnsiStyleBuilder(_options, _fgColor, KokubanColorValue.FromBasic(color), _underline, _overline, _italic, _bold, _dim, _inverse);
        public AnsiStyleBuilder Rgb(byte r, byte g, byte b)
            => new AnsiStyleBuilder(_options, KokubanColorValue.FromRgb(r, g, b), _bgColor, _underline, _overline, _italic, _bold, _dim, _inverse);
        public AnsiStyleBuilder BgRgb(byte r, byte g, byte b)
            => new AnsiStyleBuilder(_options, _fgColor, KokubanColorValue.FromRgb(r, g, b), _underline, _overline, _italic, _bold, _dim, _inverse);

        public string this[string value] => Render(value);

        public static string operator +(AnsiStyleBuilder a, string b)
            => AnsiStyledStringRenderer.Default.Render(a, b);

        public string Render(string value)
            => AnsiStyledStringRenderer.Default.Render(this, value);
    }
}