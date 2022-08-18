namespace Minesweeper.Data.Uttilites.OutputMessages
{
    internal static class EngineOutputMessages
    {
        public const string CellIsAreadyExplored = "This cell is already explored. Try with ather coordinates.";
        public const string CoordinateseCannotBeNull = "Coordinates cannot be null.";
        public const string EnterValidCoordinates = "Plase enter valid coordinates!";
        public const string CoordinatesCannotBeNegative = "Coordinates cannot be negative!";
        public const string NumberOverflow = "Coordinate is too big to be valid number!";
        public const string CoordinatesOutOfTheGameAreaSize = "Coordinate is out of the game size. Place eter a coordinate that is between 1-{0}";
    }
}
