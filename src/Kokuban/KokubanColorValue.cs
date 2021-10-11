using System;
using System.Runtime.InteropServices;

namespace Kokuban
{
    public enum KokubanColorValueType : byte
    {  
        /// <summary>
        /// 4-bit colors
        /// </summary>
        Code,
        /// <summary>
        /// 8-bit indexed colors
        /// </summary>
        Indexed,
        /// <summary>
        /// 24-bit full colors
        /// </summary>
        Rgb
    }

    /// <summary>
    /// Represents a text color of <see cref="AnsiEscape.AnsiStyle"/>.
    /// </summary>
    public readonly struct KokubanColorValue : IEquatable<KokubanColorValue>
    {
        public KokubanColorValueType Type { get; }

        public byte Index { get; }

        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        private KokubanColorValue(KokubanColorValueType type, byte index)
        {
            Type = type;
            R = G = B = 0;
            Index = index;
        }

        private KokubanColorValue(KokubanColorValueType type, byte r, byte g, byte b)
        {
            Type = type;
            Index = 0;
            R = r;
            G = g;
            B = b;
        }

        public static KokubanColorValue FromBasic(byte index)
            => new KokubanColorValue(KokubanColorValueType.Code, index);
        public static KokubanColorValue FromColor(KokubanColor color)
            => new KokubanColorValue(KokubanColorValueType.Code, (byte)color);
        public static KokubanColorValue FromIndex(byte index)
            => new KokubanColorValue(KokubanColorValueType.Indexed, index);
        public static KokubanColorValue FromRgb(byte r, byte g, byte b)
            => new KokubanColorValue(KokubanColorValueType.Rgb, r, g, b);

        // Qix-/color-convert
        // The original source code is licensed under the MIT License.
        // Copyright © 2011-2016, Heather Arthur. Copyright © 2016-2021, Josh Junon.
        // https://github.com/Qix-/color-convert/blob/3f0e0d4e92e235796ccb17f6e85c72094a651f49/conversions.js
        internal static byte FromRgbToAnsi256(KokubanColorValue color)
        {
            if (color.Type != KokubanColorValueType.Rgb) throw new ArgumentException($"ColorValueType should be Rgb. (Mode={color.Type})", nameof(color));

            var r = color.R;
            var g = color.G;
            var b = color.B;
            // We use the extended greyscale palette here, with the exception of
            // black and white. normal palette only has 4 greyscale shades.
            if (r == g && g == b)
            {
                if (r < 8)
                {
                    return 16;
                }

                if (r > 248)
                {
                    return 231;
                }

                return (byte)(Math.Round((((double)r - 8) / 247) * 24) + 232);
            }

            var ansi = 16
                + (36 * Math.Round((double)r / 255 * 5))
                + (6 * Math.Round((double)g / 255 * 5))
                + Math.Round((double)b / 255 * 5);

            return (byte)ansi;
        }

        // chalk/ansi-styles
        // The original source code is licensed under the MIT License.
        // Copyright (c) Sindre Sorhus <sindresorhus@gmail.com> (https://sindresorhus.com)
        // https://github.com/chalk/ansi-styles/blob/cd0b0cb59337bfd7d3669b2d0fcde7ff661a83a6/index.js#L157
        internal static byte FromAnsi256ToAnsi(KokubanColorValue color)
        {
            if (color.Type != KokubanColorValueType.Indexed) throw new ArgumentException($"ColorValueType should be Indexed. (Mode={color.Type})", nameof(color));

            var code = color.Index;
            if (code < 8)
            {
                return (byte)(30 + code);
            }

            if (code < 16)
            {
                return (byte)(90 + (code - 8));
            }

            var red = 0d;
            var green = 0d;
            var blue = 0d;

            if (code >= 232)
            {
                red = (((code - 232) * 10) + 8) / 255d;
                green = red;
                blue = red;
            }
            else
            {
                code -= 16;

                var remainder = code % 36;

                red = Math.Floor((double)code / 36) / 5;
                green = Math.Floor((double)remainder / 6) / 5;
                blue = (remainder % 6) / 5d;
            }

            var value = (byte)(Math.Max(Math.Max(red, green), blue) * 2);

            if (value == 0)
            {
                return 30;
            }

            var result = 30 + (((int)Math.Round(blue) << 2) | ((int)Math.Round(green) << 1) | (int)Math.Round(red));

            if (value == 2)
            {
                result += 60;
            }

            return (byte)result;
        }

        public static bool operator ==(KokubanColorValue l, KokubanColorValue r)
            => l.Equals(r);
        public static bool operator !=(KokubanColorValue l, KokubanColorValue r)
            => !l.Equals(r);

        public bool Equals(KokubanColorValue other)
        {
            return Type == other.Type && Index == other.Index && R == other.R && G == other.G && B == other.B;
        }

        public override bool Equals(object? obj)
        {
            return obj is KokubanColorValue other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Type;
                hashCode = (hashCode * 397) ^ Index.GetHashCode();
                hashCode = (hashCode * 397) ^ R.GetHashCode();
                hashCode = (hashCode * 397) ^ G.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                return hashCode;
            }
        }
    }
}
