namespace Minesweeper.Data.Uttilites.Exceptions
{
    using System;
    internal class MarkNotABombException : Exception
    {
        public MarkNotABombException()
            : this("You can mark as bomb only unexplored cells.")
        {
        }
        public MarkNotABombException(string msg)
            : base(msg)
        {
        }
    }
}
