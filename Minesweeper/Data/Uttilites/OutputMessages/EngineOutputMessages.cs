namespace Minesweeper.Data.Uttilites.OutputMessages
{
    internal static class EngineOutputMessages
    {
        public const string GameStartsMessage = "\x1b[0m***MINESWEEPER***\x1b[0m";
        public const string EnterCommand = "Enter 'X' - coordinate than ',' and the 'Y' - coordinate.\n" +
                    "If you want to flag or unflag the cell enter another ',' followed by 'f' (to flag the cell) or '/u' (to unflag the cell):";

        public const string MarkedCellAsBomb = "You have flaged cell [{0},{1}] as bomb.";
        public const string UnMarkedCell = "You have unflaged cell [{0},{1}].";
        public const string CellIsAreadyExplored = "This cell is already explored. Try with ather coordinates.";

        public const string UnknownCommand = "Enter valid command or none to explore the cell.";
        public const string EnterValidCoordinates = "Plase enter valid coordinates!";
        public const string EnterCoordinates = "Plase enter coordinates!";
        public const string CoordinatesOutOfTheGameAreaSize = "Coordinate is out of the game size. Place eter a coordinate that is between 1-{0}!";

        public const string PlayAgainAskMsg = "Play again?";
    }
}
