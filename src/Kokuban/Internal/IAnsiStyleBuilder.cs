namespace Kokuban.Internal
{
    internal interface IAnsiStyleBuilder
    {
        KokubanOptions? Options { get; }
        KokubanColorValue? Foreground { get; }
        KokubanColorValue? Background { get; }
        bool Underline { get; }
        bool Overline { get; }
        bool Italic { get; }
        bool Bold { get; }
        bool Dim { get; }
        bool Inverse { get; }
    }
}