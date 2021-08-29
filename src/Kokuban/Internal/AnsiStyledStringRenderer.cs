using System;
using System.Text;
using Kokuban.AnsiEscape;

namespace Kokuban.Internal
{
    internal struct RenderState
    {
        public KokubanColorValue? Foreground;
        public KokubanColorValue? Background;
        public bool? Underline;
        public bool? Overline;
        public bool? Italic;
        public bool? Bold;
        public bool? Dim;
        public bool? Inverse;
        public KokubanOptions Options;
    }

    internal class AnsiStyledStringRenderer
    {
        public static AnsiStyledStringRenderer Default { get; } = new AnsiStyledStringRenderer();

        public string Render(AnsiStyledString s)
        {
            var sb = new StringBuilder();
            RenderCore(sb, s, default);
            return sb.ToString();
        }

        private void RenderCore(StringBuilder sb, AnsiStyledString s, in RenderState prevState)
        {
            var hasPrefix = false;
            var renderState = prevState; // Copy previous state to new state.
            if (s.Style is {} style)
            {
                hasPrefix = TryRenderPrefix(sb, style, ref renderState);
            }

            switch (s.First)
            {
                case string str: sb.Append(str); break;
                case AnsiStyledString styled: RenderCore(sb, styled, renderState); break;
                default: throw new NotSupportedException();
            }
            switch (s.Second)
            {
                case string str: sb.Append(str); break;
                case AnsiStyledString styled: RenderCore(sb, styled, renderState); break;
                case AnsiStyleBuilder: break;
                case null: break; // Ignore
                default: throw new NotSupportedException();
            }

            if (hasPrefix)
            {
                RenderSuffix(sb, prevState, renderState);
            }
        }

        private bool TryRenderPrefix<T>(StringBuilder sb, T style, ref RenderState renderState)
            where T : struct, IAnsiStyleBuilder
        {
            var options = renderState.Options = style.Options ?? KokubanOptions.Default;
            if (options.Mode == KokubanColorMode.None)
            {
                return false;
            }

            if (style.Foreground is null && style.Background is null && !style.Underline && !style.Overline && !style.Italic && !style.Bold && !style.Dim && !style.Inverse)
            {
                return false;
            }

            if (options.AutoEnableEscapeSequenceOnWindows)
            {
                WindowsConsole.TryAutoEnableEscapeSequenceOnce();
            }

            var hasCode = false;
            sb.Append(AnsiEscapeCode.EscapeSequenceCsi);

            if (TryAppendColor(sb, false, style.Background, hasCode, options))
            {
                hasCode = true;
                renderState.Background = style.Background;
            }
            if (TryAppendColor(sb, true, style.Foreground, hasCode, options))
            {
                hasCode = true;
                renderState.Foreground = style.Foreground;
            }
            if (TryAppend(sb, style.Bold, AnsiEscapeCode.Bold.Begin, hasCode))
            {
                hasCode = true;
                renderState.Bold = style.Bold;
            }
            if (TryAppend(sb, style.Dim, AnsiEscapeCode.Dim.Begin, hasCode))
            {
                hasCode = true;
                renderState.Dim = style.Dim;
            }
            if (TryAppend(sb, style.Italic, AnsiEscapeCode.Italic.Begin, hasCode))
            {
                hasCode = true;
                renderState.Italic = style.Italic;
            }
            if (TryAppend(sb, style.Underline, AnsiEscapeCode.Underline.Begin, hasCode))
            {
                hasCode = true;
                renderState.Underline = style.Underline;
            }
            if (TryAppend(sb, style.Inverse, AnsiEscapeCode.Inverse.Begin, hasCode))
            {
                hasCode = true;
                renderState.Inverse = style.Inverse;
            }
            if (TryAppend(sb, style.Overline, AnsiEscapeCode.Overline.Begin, hasCode))
            {
                hasCode = true;
                renderState.Overline = style.Overline;
            }

            sb.Append('m');

            return true;
        }

        private void RenderSuffix(StringBuilder sb, in RenderState prevState, in RenderState currentState)
        {
            sb.Append(AnsiEscapeCode.EscapeSequenceCsi);
            
            var hasCode = false;
            hasCode |= TryAppendResetColor(sb, false, prevState.Background, currentState.Background, hasCode, currentState.Options);
            hasCode |= TryAppendResetColor(sb, true, prevState.Foreground, currentState.Foreground, hasCode, currentState.Options);
            hasCode |= TryAppendReset(sb, prevState.Bold, currentState.Bold, AnsiEscapeCode.Bold, hasCode);
            hasCode |= TryAppendReset(sb, prevState.Dim, currentState.Dim, AnsiEscapeCode.Dim, hasCode);
            hasCode |= TryAppendReset(sb, prevState.Italic, currentState.Italic, AnsiEscapeCode.Italic, hasCode);
            hasCode |= TryAppendReset(sb, prevState.Underline, currentState.Underline, AnsiEscapeCode.Underline, hasCode);
            hasCode |= TryAppendReset(sb, prevState.Inverse, currentState.Inverse, AnsiEscapeCode.Inverse, hasCode);
            hasCode |= TryAppendReset(sb, prevState.Overline, currentState.Overline, AnsiEscapeCode.Overline, hasCode);
            
            sb.Append('m');
        }

        private static bool TryAppendColor(StringBuilder sb, bool isForeground, KokubanColorValue? value, bool hasCode, KokubanOptions options)
        {
            if (options.Mode != KokubanColorMode.None && value is not null)
            {
                if (hasCode)
                {
                    sb.Append(';');
                }

                var color = value.Value;

                // 24-bit
                if (color.Type == KokubanColorValueType.Rgb)
                {
                    if (options.Mode >= KokubanColorMode.TrueColor)
                    {
                        sb.Append(isForeground ? "38;2;" : "48;2;");
                        sb.Append(color.R);
                        sb.Append(';');
                        sb.Append(color.G);
                        sb.Append(';');
                        sb.Append(color.B);

                        return true;
                    }

                    // fallback to ANSI 256 code
                    color = KokubanColorValue.FromIndex(KokubanColorValue.FromRgbToAnsi256(color));
                }

                // 8-bit
                if (color.Type == KokubanColorValueType.Indexed)
                {
                    if (options.Mode >= KokubanColorMode.Indexed)
                    {
                        sb.Append(isForeground ? "38;5;" : "48;5;");
                        sb.Append(color.Index);
                        return true;
                    }

                    // fallback to ANSI code
                    color = KokubanColorValue.FromBasic(KokubanColorValue.FromAnsi256ToAnsi(color));
                }

                // Basic (3, 4-bit)
                sb.Append(color.Index + (isForeground ? 0 : 10) /* Offset */);
                return true;
            }

            return false;
        }

        private static bool TryAppend(StringBuilder sb, bool enable, int value, bool hasCode)
        {
            if (enable)
            {
                if (hasCode)
                {
                    sb.Append(';');
                }
                sb.Append(value);
                return true;
            }

            return false;
        }

        private static bool TryAppendReset(StringBuilder sb, bool? prev, bool? current, (int Begin, int End) code, bool hasCode)
        {
            if (current.HasValue)
            {
                if (hasCode)
                {
                    sb.Append(';');
                }

                if (prev.HasValue)
                {
                    sb.Append(code.Begin);
                }
                else
                {
                    sb.Append(code.End);
                }
                return true;
            }

            return false;
        }

        private static bool TryAppendResetColor(StringBuilder sb, bool isForeground, in KokubanColorValue? prev, in KokubanColorValue? current, bool hasCode, KokubanOptions options)
        {
            if (current.HasValue)
            {
                if (prev.HasValue)
                {
                    if (prev.Value == current.Value)
                    {
                        return false;
                    }
                    return TryAppendColor(sb, isForeground, prev, hasCode, options);
                }
                else
                {
                    if (options.Mode != KokubanColorMode.None)
                    {
                        if (hasCode)
                        {
                            sb.Append(';');
                        }
                        sb.Append(isForeground ? 39 : 49);
                        return true;
                    }

                    return false;
                }
            }

            return false;
        }
    }
}
