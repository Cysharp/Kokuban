namespace Kokuban
{
    public enum KokubanColorMode
    {
        /// <summary>
        /// Disable colors
        /// </summary>
        None,
        /// <summary>
        /// Enable 3/4-bit colors (16 colors)
        /// </summary>
        Standard,
        /// <summary>
        /// Enable 8-bit colors (256 colors)
        /// </summary>
        Indexed,
        /// <summary>
        /// Enable 24-bit colors
        /// </summary>
        TrueColor,
    }
}