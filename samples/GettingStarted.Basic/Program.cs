using System;
using Kokuban;
using Kokuban.AnsiEscape;

namespace GettingStarted.Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Chalk.BgYellow.White + ("Hello " + (Chalk.BrightBlue.Underline + "Konnichiwa") + "!"));
        }
    }
}
