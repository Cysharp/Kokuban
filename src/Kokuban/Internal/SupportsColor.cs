using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Kokuban.Internal
{
    // This class based on chalk/supports-color
    // The original source code is licensed under the MIT License.
    // Copyright (c) Sindre Sorhus <sindresorhus@gmail.com> (https://sindresorhus.com)
    // https://github.com/chalk/supports-color
    public static class SupportsColor
    {
        private static readonly string[] WellknownCIs = new[] { "TRAVIS", "CIRCLECI", "APPVEYOR", "GITLAB_CI", "GITHUB_ACTIONS", "BUILDKITE", "DRONE" };

        public static KokubanColorMode Output { get; private set; }
        public static KokubanColorMode Error { get; private set; }

        static SupportsColor()
        {
            Refresh();
        }

        public static void Refresh()
        {
            Output = GetSupportedColorMode(!Console.IsOutputRedirected);
            Error = GetSupportedColorMode(!Console.IsErrorRedirected);
        }

        private static bool TryGetForceColorByEnvironmentVariable(out KokubanColorMode mode)
        {
            if (TryGetEnvironmentVariable("FORCE_COLOR", out var envForceColor))
            {
                switch (envForceColor)
                {
                    case "true":
                        mode = KokubanColorMode.Indexed;
                        return true;
                    case "false":
                        mode = KokubanColorMode.None;
                        return true;
                    default:
                        return Enum.TryParse(envForceColor, out mode);
                }
            }

            mode = KokubanColorMode.None;
            return false;
        }

        public static KokubanColorMode GetSupportedColorMode(bool isTty)
        {
            var min = KokubanColorMode.None;

            var hasForceColor = TryGetForceColorByEnvironmentVariable(out var forceColor);
            if (hasForceColor)
            {
                min = forceColor;
                if (forceColor == KokubanColorMode.None)
                {
                    return KokubanColorMode.None;
                }
            }

            if (!isTty && !hasForceColor)
            {
                return KokubanColorMode.None;
            }

            var envTerm = GetEnvironmentVariable("TERM");
            if (envTerm == "dump")
            {
                return min;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Windows 10 build 10586 is the first Windows release that supports 256 colors.
                // Windows 10 build 14931 is the first release that supports 16m/TrueColor.
                var osVersion = Environment.OSVersion.Version;
                if (osVersion.Major > 10)
                {
                    // Windows 8, 7 ...
                    return KokubanColorMode.None;
                }
                else if (osVersion.Major >= 10 && osVersion.Build >= 10586)
                {
                    // Windows 10
                    return osVersion.Build >= 14931 ? KokubanColorMode.TrueColor : KokubanColorMode.Indexed;
                }
                else
                {
                    // Windows 11 or later.
                    return KokubanColorMode.TrueColor;
                }
            }

            if (TryGetEnvironmentVariable("CI", out var envCI))
            {
                if (WellknownCIs.Contains(envCI) || (TryGetEnvironmentVariable("CI_NAME", out var envVar) && envVar == "codeship"))
                {
                    return KokubanColorMode.Standard;
                }

                return min;
            }

            if (TryGetEnvironmentVariable("TEAMCITY_VERSION", out var envTeamcityVersion))
            {
                return Regex.IsMatch(envTeamcityVersion, @"^(9\.(0*[1-9]\d*)\.|\d{2,}\.)")
                    ? KokubanColorMode.Indexed
                    : KokubanColorMode.None;
            }

            if (GetEnvironmentVariable("COLORTERM") == "truecolor")
            {
                return KokubanColorMode.TrueColor;
            }

            if (TryGetEnvironmentVariable("TERM_PROGRAM", out var envTermProgram))
            {
                if (!int.TryParse(GetEnvironmentVariable("TERM_PROGRAM_VERSION").Split('.')[0], out var programVersion))
                {
                    programVersion = 0;
                }

                switch (envTermProgram)
                {
                    case "iTerm.app":
                        return programVersion >= 3 ? KokubanColorMode.TrueColor : KokubanColorMode.Indexed;
                    case "Apple_Terminal":
                        return KokubanColorMode.Indexed;
                    // No default
                }
            }

            if (Regex.IsMatch(envTerm, @"-256(color)?$"))
            {
                return KokubanColorMode.Indexed;
            }

            if (Regex.IsMatch(envTerm, @"^screen|^xterm|^vt100|^vt220|^rxvt|color|ansi|cygwin|linux"))
            {
                return KokubanColorMode.Standard;
            }

            if (!string.IsNullOrEmpty(GetEnvironmentVariable("COLORTERM")))
            {
                return KokubanColorMode.Standard;
            }

            return min;
        }

        private static string GetEnvironmentVariable(string key)
            => Environment.GetEnvironmentVariable(key) ?? string.Empty;

        private static bool TryGetEnvironmentVariable(string key, out string output)
        {
            var value = Environment.GetEnvironmentVariable(key);
            if (string.IsNullOrEmpty(value))
            {
                output = string.Empty;
                return false;
            }

            output = value;
            return true;

        }
    }
}
