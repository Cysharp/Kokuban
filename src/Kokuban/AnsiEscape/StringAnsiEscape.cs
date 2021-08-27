namespace Kokuban.AnsiEscape
{
    public static partial class StringAnsiEscape
    {
        public const string EscapeSequenceCsi = "\u001B["; // ESC[
        public const string ResetColor = EscapeSequenceCsi + "39;49m";

        public static string ForegroundColor(string s, KokubanColor color)
            => Decorate(s, color.ToAnsiColor().Foreground, 39);
        public static string BackgroundColor(string s, KokubanColor color)
            => Decorate(s, color.ToAnsiColor().Foreground, 49);

        public static string Decorate(string s, int beginCode, int endCode)
            => $"{EscapeSequenceCsi}{beginCode}m{s}{EscapeSequenceCsi}{endCode}m";
    }
}