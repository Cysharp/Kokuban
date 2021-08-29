using System;
using FluentAssertions;
using Kokuban.AnsiEscape;
using Xunit;

namespace Kokuban.Tests
{
    public class AnsiStyledStringTest
    {
        [Fact]
        public void BuilderPlusString()
        {
            var styled = Chalk.Bold + "Foo";
            styled.Style.Should().NotBeNull();
            styled.First.Should().BeOfType<string>().And.Be("Foo");
            styled.Second.Should().BeNull();
        }
        [Fact]
        public void StringPlusBuilder()
        {
            var styled = "Foo" + Chalk.Bold;
            styled.Style.Should().BeNull();
            styled.First.Should().BeOfType<string>().And.Be("Foo");
            styled.Second.Should().BeOfType<AnsiStyleBuilder>();
        }

        [Fact]
        public void BuilderPlusString_1()
        {
            // [None, ("Foo", Style:Italic)]
            var styled = Chalk.Bold + "Foo" + Chalk.Italic;
            styled.Style.Should().BeNull();
            styled.First.Should().BeOfType<AnsiStyledString>();
            styled.Second.Should().BeOfType<AnsiStyleBuilder>();
        }

        [Fact]
        public void BuilderPlusStyledComplex()
        {
            // [Style:Bold, ("Foo", (Style:Italic, "Bar"))]
            var styled = Chalk.Bold + ("Foo" + Chalk.Italic + "Bar");
            styled.Style.Should().NotBeNull();
            var first = styled.First.Should().BeOfType<AnsiStyledString>().Subject;
            styled.Second.Should().BeNull();

            first.First.Should().Be("Foo");
            var second2 = first.Second.Should().BeOfType<AnsiStyledString>().Subject;
            second2.Style.Should().NotBeNull();
            second2.First.Should().Be("Bar");
            second2.Second.Should().BeNull();
        }

        [Fact]
        public void BuilderPlusStyled()
        {
            // [Style:Bold, ("Foo")]
            var styled = Chalk.Bold + new AnsiStyledString(null, "Foo");
            styled.Style.Should().NotBeNull();
            styled.First.Should().BeOfType<AnsiStyledString>().Subject.First.Should().Be("Foo");
            styled.Second.Should().BeNull();
        }

        [Fact]
        public void StyledPlusBuilder()
        {
            // [None, ("Foo", Style:Bold)]
            var styled = new AnsiStyledString(null, "Foo") + Chalk.Bold;
            styled.Style.Should().BeNull();
            styled.First.Should().BeOfType<AnsiStyledString>().Subject.First.Should().Be("Foo");
            styled.Second.Should().BeOfType<AnsiStyleBuilder>();
        }

        [Fact]
        public void StyledPlusString()
        {
            // [None, (("Foo"), "Bar")]
            var styled = new AnsiStyledString(null, "Foo") + "Bar";
            styled.Style.Should().BeNull();
            styled.First.Should().BeOfType<AnsiStyledString>().Subject.First.Should().Be("Foo");
            styled.Second.Should().Be("Bar");
        }

    }
}
