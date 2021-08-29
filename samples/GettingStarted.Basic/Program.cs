using System;
using Kokuban;
using Kokuban.AnsiEscape;

namespace GettingStarted.Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(string.Join(" ", Chalk.Bold["Bold"], Chalk.Underline["Underline"], Chalk.Inverse["Inverse"]));
            Console.WriteLine(string.Join(" ", Chalk.Red["Red"], Chalk.Green["Green"], Chalk.Blue["Blue"], Chalk.BgRed["BgRed"], Chalk.BgGreen["BgGreen"], Chalk.BgBlue["BgBlue"]));
            Console.WriteLine();

            Console.WriteLine(Chalk.Red.Underline["Hello"]);
            Console.WriteLine(Chalk.Red.Underline["Hello " + Chalk.Underline.BgBlue["World"] + "!"]);
            Console.WriteLine(Chalk.Bold.Gray.BgYellow + ("＼" + (Chalk.White.BgRed["Hello"] + " " + (Chalk.White.BgBlue + "コンニチハ") + "!!／")));
        }
    }
}
