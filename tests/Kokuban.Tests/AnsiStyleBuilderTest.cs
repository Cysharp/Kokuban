using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Kokuban.AnsiEscape;
using Kokuban.Internal;
using Xunit;

namespace Kokuban.Tests
{
    public class AnsiStyleBuilderTest
    {
        [Fact]
        public void Render_Indexer()
        {
            var builder = new AnsiStyleBuilder().Bold;

            var styled = builder["foo"];
            styled.Style.Should().NotBeNull();
            styled.First.Should().Be("foo");
            styled.Second.Should().BeNull();
        }

        [Fact]
        public void Render()
        {
            var builder = new AnsiStyleBuilder().Bold;

            var styled = builder.Render("foo");
            styled.Style.Should().NotBeNull();
            styled.First.Should().Be("foo");
            styled.Second.Should().BeNull();
        }

        [Fact]
        public void Flags()
        {
            {
                var builder = ((IAnsiStyleBuilder)new AnsiStyleBuilder().Bold);
                builder.Bold.Should().BeTrue();
                builder.Dim.Should().BeFalse();
                builder.Italic.Should().BeFalse();
                builder.Underline.Should().BeFalse();
                builder.Overline.Should().BeFalse();
                builder.Inverse.Should().BeFalse();
                builder.Background.Should().BeNull();
                builder.Foreground.Should().BeNull();
            }
            {
                var builder = ((IAnsiStyleBuilder)new AnsiStyleBuilder().Bold.Dim);
                builder.Bold.Should().BeTrue();
                builder.Dim.Should().BeTrue();
                builder.Italic.Should().BeFalse();
                builder.Underline.Should().BeFalse();
                builder.Overline.Should().BeFalse();
                builder.Inverse.Should().BeFalse();
                builder.Background.Should().BeNull();
                builder.Foreground.Should().BeNull();
            }
            {
                var builder = ((IAnsiStyleBuilder)new AnsiStyleBuilder().Bold.Dim.Italic);
                builder.Bold.Should().BeTrue();
                builder.Dim.Should().BeTrue();
                builder.Italic.Should().BeTrue();
                builder.Underline.Should().BeFalse();
                builder.Overline.Should().BeFalse();
                builder.Inverse.Should().BeFalse();
                builder.Background.Should().BeNull();
                builder.Foreground.Should().BeNull();
            }
            {
                var builder = ((IAnsiStyleBuilder)new AnsiStyleBuilder().Bold.Dim.Italic.Underline);
                builder.Bold.Should().BeTrue();
                builder.Dim.Should().BeTrue();
                builder.Italic.Should().BeTrue();
                builder.Underline.Should().BeTrue();
                builder.Overline.Should().BeFalse();
                builder.Inverse.Should().BeFalse();
                builder.Background.Should().BeNull();
                builder.Foreground.Should().BeNull();
            }
            {
                var builder = ((IAnsiStyleBuilder)new AnsiStyleBuilder().Bold.Dim.Italic.Underline.Overline);
                builder.Bold.Should().BeTrue();
                builder.Dim.Should().BeTrue();
                builder.Italic.Should().BeTrue();
                builder.Underline.Should().BeTrue();
                builder.Overline.Should().BeTrue();
                builder.Inverse.Should().BeFalse();
                builder.Background.Should().BeNull();
                builder.Foreground.Should().BeNull();
            }
            {
                var builder = ((IAnsiStyleBuilder)new AnsiStyleBuilder().Bold.Dim.Italic.Underline.Overline.Inverse);
                builder.Bold.Should().BeTrue();
                builder.Dim.Should().BeTrue();
                builder.Italic.Should().BeTrue();
                builder.Underline.Should().BeTrue();
                builder.Overline.Should().BeTrue();
                builder.Inverse.Should().BeTrue();
                builder.Background.Should().BeNull();
                builder.Foreground.Should().BeNull();
            }
        }
    }
}
