namespace Minesweeper.IO
{
    using System;

    using Contracts;

    internal class ConsoleWrter : IConsoleWriter
    {
        public ConsoleWrter()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        public void Write(string input)
        {
            Console.Write(input);
        }

        public void Write(string input, ConsoleColor consoleForegroundColor)
        {
            Console.ForegroundColor = consoleForegroundColor;
            Console.Write(input);
            Console.ResetColor();
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        //Write lines could be caling console writes, but with envirment new Line
        public void WriteLine(string input)
        {
            Console.WriteLine(input);
        }

        public void WriteLine(string input, ConsoleColor consoleForegroundColor)
        {
            Console.ForegroundColor = consoleForegroundColor;
            Console.WriteLine(input);
            Console.ResetColor();
        }
    }
}
