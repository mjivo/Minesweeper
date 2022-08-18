namespace Minesweeper.IO
{
    using System;

    using Contracts;

    internal class ConsoleReader : IReader
    {
        public string ReadLine()
        {
            string input = Console.ReadLine();
            return input;
        }
    }
}
