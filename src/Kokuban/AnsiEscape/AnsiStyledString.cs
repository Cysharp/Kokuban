using System;
using System.Collections.Generic;
using System.Text;
using Kokuban.Internal;

namespace Kokuban.AnsiEscape
{
    public class AnsiStyledString
    {
        internal AnsiStyleBuilder? Style { get; set; }
        internal object? First { get; set; }
        internal object? Second { get; set; }

        public AnsiStyledString(AnsiStyleBuilder? style, object first)
        {
            Style = style;
            First = first;
        }

        public AnsiStyledString(AnsiStyleBuilder? style, object first, object second)
        {
            Style = style;
            First = first;
            Second = second;
        }

        public static AnsiStyledString operator +(string s, AnsiStyledString styled)
        {
            return new AnsiStyledString(null, s, styled);
        }
        public static AnsiStyledString operator +(AnsiStyledString styled, string s)
        {
            if (styled.Second is AnsiStyleBuilder secondStyle)
            {
                return new AnsiStyledString(null, styled.First!, new AnsiStyledString(secondStyle, s));
            }
            return new AnsiStyledString(null, styled, s);
        }
        public static AnsiStyledString operator +(AnsiStyledString styled1, AnsiStyledString styled2)
        {
            return new AnsiStyledString(null, styled1, styled2);
        }

        public static implicit operator string(AnsiStyledString s)
            => s.ToString();

        // for LINQPad
        private object ToDump()
            => ToString();

        public override string ToString()
            => AnsiStyledStringRenderer.Default.Render(this);
    }
}
