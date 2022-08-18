namespace Minesweeper.Data.Uttilites.Exceptions
{
    using System;
    internal class GameEndedException : Exception
    {
        public GameEndedException()
            : this("Game ended!")
        {
        }
        public GameEndedException(string msg)
            : base(msg)
        {
        }
    }
}
