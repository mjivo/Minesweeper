namespace Minesweeper.IO.Contracts
{
    using System;

    internal interface IWriter
    {
        public void Write(string input);

        public void WriteLine();

        public void WriteLine(string input);
    }
}
