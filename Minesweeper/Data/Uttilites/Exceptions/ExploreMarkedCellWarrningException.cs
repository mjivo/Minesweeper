namespace Minesweeper.Data.Uttilites.Exceptions
{
    using System;
    internal class ExploreMarkedCellWarrningException : Exception
    {
        public ExploreMarkedCellWarrningException()
            : this("WARRANING this cell has been marked from you as bomb.Are you sure you want to explore it.")
        {
        }
        public ExploreMarkedCellWarrningException(string msg)
            : base(msg)
        {
        }
    }
}
