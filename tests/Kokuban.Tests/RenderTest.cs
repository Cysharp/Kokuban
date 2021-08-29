using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Kokuban.AnsiEscape;
using Xunit;

namespace Kokuban.Tests
{
    public class RenderTest
    {
        [Fact]
        public void Basic()
        {
            var chalk = Chalk.Create(new KokubanOptions() { Mode = KokubanColorMode.TrueColor });
            var x = (chalk.Bold.Underline.BgBlue + "Hello").ToString();
            x.Should().Be($"{AnsiEscapeCode.EscapeSequenceCsi}44;1;4mHello{AnsiEscapeCode.EscapeSequenceCsi}49;22;24m");
        }

        [Fact]
        public void None()
        {
            var chalk = Chalk.Create(new KokubanOptions() { Mode = KokubanColorMode.None });
            var x = (chalk.Bold.Underline.BgBlue + "Hello").ToString();
            x.Should().Be($"Hello");
        }

        [Fact]
        public void StringMixed_1()
        {
            var chalk = Chalk.Create(new KokubanOptions() { Mode = KokubanColorMode.TrueColor });
            var x = ((chalk.Bold.Underline.BgBlue + "Hello") + "Konnichiwa");
            x.ToString().Should().Be($"{AnsiEscapeCode.EscapeSequenceCsi}44;1;4mHello{AnsiEscapeCode.EscapeSequenceCsi}49;22;24mKonnichiwa");
        }

        [Fact]
        public void StringMixed_2()
        {
            var chalk = Chalk.Create(new KokubanOptions() { Mode = KokubanColorMode.TrueColor });
            var x = (chalk.BgYellow.White + ("Hello " + (chalk.BrightBlue.Underline + "Konnichiwa") + "!"));
            x.ToString().Should().Be($"{AnsiEscapeCode.EscapeSequenceCsi}43;37mHello {AnsiEscapeCode.EscapeSequenceCsi}94;4mKonnichiwa{AnsiEscapeCode.EscapeSequenceCsi}37;24m!{AnsiEscapeCode.EscapeSequenceCsi}49;39m");
        }

        [Fact]
        public void Nested()
        {
            var chalk = Chalk.Create(new KokubanOptions() { Mode = KokubanColorMode.TrueColor });
            var x = ("[" + (chalk.Italic + ((chalk.Bold.Underline.BgBlue + "Hello") + "Konnichiwa")) + "!]").ToString();
            // "[" + Italic(Bold.Underline.BgBlue("Hello") + "Konnichiwa")) + "!]"
            x.Should().Be($"[{AnsiEscapeCode.EscapeSequenceCsi}3m{AnsiEscapeCode.EscapeSequenceCsi}44;1;4mHello{AnsiEscapeCode.EscapeSequenceCsi}49;22;3;24mKonnichiwa{AnsiEscapeCode.EscapeSequenceCsi}23m!]");
        }

        [Fact]
        public void RgbFg()
        {
            var chalk = Chalk.Create(new KokubanOptions() { Mode = KokubanColorMode.TrueColor });
            chalk.Rgb(255, 12, 34).Render("Hello").ToString().Should().Be($"{AnsiEscapeCode.EscapeSequenceCsi}38;2;255;12;34mHello{AnsiEscapeCode.EscapeSequenceCsi}39m");
        }

        [Fact]
        public void RgbBg()
        {
            var chalk = Chalk.Create(new KokubanOptions() { Mode = KokubanColorMode.TrueColor });
            chalk.BgRgb(255, 12, 34).Render("Hello").ToString().Should().Be($"{AnsiEscapeCode.EscapeSequenceCsi}48;2;255;12;34mHello{AnsiEscapeCode.EscapeSequenceCsi}49m");
        }

        [Fact]
        public void Ansi256Fg()
        {
            var chalk = Chalk.Create(new KokubanOptions() { Mode = KokubanColorMode.TrueColor });
            chalk.Ansi256(128).Render("Hello").ToString().Should().Be($"{AnsiEscapeCode.EscapeSequenceCsi}38;5;128mHello{AnsiEscapeCode.EscapeSequenceCsi}39m");
        }

        [Fact]
        public void Ansi256Bg()
        {
            var chalk = Chalk.Create(new KokubanOptions() { Mode = KokubanColorMode.TrueColor });
            chalk.BgAnsi256(128).Render("Hello").ToString().Should().Be($"{AnsiEscapeCode.EscapeSequenceCsi}48;5;128mHello{AnsiEscapeCode.EscapeSequenceCsi}49m");
        }

        [Fact]
        public void FallbackRgbToAnsi()
        {
            var chalk = Chalk.Create(new KokubanOptions() { Mode = KokubanColorMode.Standard });
            // rgb(255, 12, 34) -> BrightRed
            chalk.Rgb(255, 12, 34).Render("Hello").ToString().Should().Be($"{AnsiEscapeCode.EscapeSequenceCsi}{AnsiEscapeCode.BrightRed.Begin}mHello{AnsiEscapeCode.EscapeSequenceCsi}39m");
        }

        [Fact]
        public void FallbackAnsi256ToAnsi()
        {
            var chalk = Chalk.Create(new KokubanOptions() { Mode = KokubanColorMode.Standard });
            // ANSI256(196) -> BrightRed
            chalk.Ansi256(196).Render("Hello").ToString().Should().Be($"{AnsiEscapeCode.EscapeSequenceCsi}{AnsiEscapeCode.BrightRed.Begin}mHello{AnsiEscapeCode.EscapeSequenceCsi}39m");
        }
    }
}
