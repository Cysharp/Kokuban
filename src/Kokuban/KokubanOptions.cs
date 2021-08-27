using System;
using Kokuban.Internal;

namespace Kokuban
{
    public class KokubanOptions
    {
        public static KokubanOptions Default { get; } = new KokubanOptions();

        public KokubanColorMode Mode { get; set; } = SupportsColor.Output;

        public bool AutoEnableEscapeSequenceOnWindows { get; set; } = true;
    }
}