namespace Minesweeper.Data.Uttilites.Exceptions
{
    using System;
    internal class UnMarkNotMarkedCellException : Exception
    {
        public UnMarkNotMarkedCellException()
            : this("Cannot unmark cell that is not previously marked as bomb.")
        {
        }
        public UnMarkNotMarkedCellException(string msg)
            : base(msg)
        {
        }
    }
}
