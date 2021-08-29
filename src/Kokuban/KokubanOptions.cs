using System;
using Kokuban.AnsiEscape;
using Kokuban.Internal;

namespace Kokuban
{
    /// <summary>
    /// Options for Kokuban
    /// </summary>
    public class KokubanOptions
    {
        public static KokubanOptions Default { get; } = new KokubanOptions();

        /// <summary>
        /// Gets or sets a supported color mode.
        /// </summary>
        public KokubanColorMode Mode { get; set; } = SupportsColor.Output;

        /// <summary>
        /// Gets or sets whether escape sequences should be automatically enabled in Windows console.
        /// </summary>
        /// <remarks>
        /// Auto-enablement will only trigger once globally.
        /// </remarks>
        public bool AutoEnableEscapeSequenceOnWindows { get; set; } = true;
    }
}
