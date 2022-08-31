namespace Minesweeper.Data.Uttilites.Exceptions
{
    using System;

    internal class GameWonException : Exception
    {
        public GameWonException()
            : this("Winner! You won the game.")
        {

        }
        public GameWonException(string message)
            : base(message)
        {
        }
    }
}
