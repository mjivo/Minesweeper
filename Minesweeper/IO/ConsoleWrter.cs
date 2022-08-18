namespace Minesweeper.IO
{
    using System;

    using Contracts;
    internal class ConsoleWrter : IWriter
    {
        public void Write(string input)
        {
            Console.Write(input);
        }

        public void WriteLine(string input)
        {
            Console.WriteLine(input);
        }
    }
}
