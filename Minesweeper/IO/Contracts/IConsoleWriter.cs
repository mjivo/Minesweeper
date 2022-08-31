namespace Minesweeper.IO.Contracts
{
    using System;

    internal interface IConsoleWriter : IWriter
    {
        public void Write(string input, ConsoleColor consoleForegroundColor);

        public void WriteLine(string input, ConsoleColor consoleForegroundColor);

    }
}
