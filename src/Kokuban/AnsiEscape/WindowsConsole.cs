using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Kokuban.Internal;

namespace Kokuban.AnsiEscape
{
    public static class WindowsConsole
    {
        private static int _autoEnabled;

        internal static void TryAutoEnableEscapeSequenceOnce()
        {
            if (Interlocked.CompareExchange(ref _autoEnabled, 1, 0) == 0)
            {
                TryEnableEscapeSequence();
            }
        }

        public static bool TryEnableEscapeSequence()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var stdOutput = Win32.GetStdHandle(Win32.StdHandle.STD_OUTPUT_HANDLE);
                if (Win32.GetConsoleMode(stdOutput, out var mode))
                {
                    if (Win32.SetConsoleMode(stdOutput, mode | Win32.ConsoleMode.ENABLE_VIRTUAL_TERMINAL_PROCESSING))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
