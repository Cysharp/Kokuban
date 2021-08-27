using System.Text;
using Kokuban.AnsiEscape;

namespace Kokuban.Internal
{
    internal class AnsiStyledStringRenderer
    {
        public static AnsiStyledStringRenderer Default { get; } = new AnsiStyledStringRenderer();

        public string Render<T>(T style, string value)
            where T: IAnsiStyleBuilder
        {
            var options = style.Options ?? KokubanOptions.Default;

            if (options.AutoEnableEscapeSequenceOnWindows)
            {
                WindowsConsole.TryAutoEnableEscapeSequenceOnce();
            }

            if (
                options.Mode == KokubanColorMode.None ||
                (style.Foreground is null && style.Background is null && !style.Underline && !style.Overline && !style.Italic && !style.Bold && !style.Dim && !style.Inverse)
            )
            {
                return value;
            }

            var sb = new StringBuilder();
            var hasCode = false;
            sb.Append(StringAnsiEscape.EscapeSequenceCsi);

            hasCode |= TryAppendColor(sb, false, style.Background, hasCode, options);
            hasCode |= TryAppendColor(sb, true, style.Foreground, hasCode, options);
            hasCode |= TryAppend(sb, style.Bold, 1, hasCode);
            hasCode |= TryAppend(sb, style.Dim, 2, hasCode);
            hasCode |= TryAppend(sb, style.Italic, 3, hasCode);
            hasCode |= TryAppend(sb, style.Underline, 4, hasCode);
            hasCode |= TryAppend(sb, style.Inverse, 7, hasCode);
            hasCode |= TryAppend(sb, style.Overline, 53, hasCode);
            if (hasCode)
            {
                sb.Append('m');
            }

            sb.Append(value);

            hasCode = false;
            sb.Append(StringAnsiEscape.EscapeSequenceCsi);
            hasCode |= TryAppendResetColor(sb, false, style.Background.HasValue, hasCode, options);
            hasCode |= TryAppendResetColor(sb, true, style.Foreground.HasValue, hasCode, options);
            hasCode |= TryAppend(sb, style.Bold, 22, hasCode);
            hasCode |= TryAppend(sb, style.Dim, 22, hasCode);
            hasCode |= TryAppend(sb, style.Italic, 23, hasCode);
            hasCode |= TryAppend(sb, style.Underline, 24, hasCode);
            hasCode |= TryAppend(sb, style.Inverse, 27, hasCode);
            hasCode |= TryAppend(sb, style.Overline, 55, hasCode);
            if (hasCode)
            {
                sb.Append('m');
            }

            return sb.ToString();
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
                    color = KokubanColorValue.FromIndexed(KokubanColorValue.FromRgbToAnsi256(color));
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

        private static bool TryAppendResetColor(StringBuilder sb, bool isForeground, bool enable, bool hasCode, KokubanOptions options)
        {
            if (enable && options.Mode != KokubanColorMode.None)
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
}
