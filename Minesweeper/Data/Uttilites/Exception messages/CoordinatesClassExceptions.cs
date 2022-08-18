using Minesweeper.Build.Contracts;

namespace Minesweeper.Data.Uttilites.Exception_messages
{
    internal class CoordinatesClassExceptions
    {
        public const string NegativeValueForCoordinate = "{0} - coordinate cannot be negative.";
        public const string CompearToCanNotAcceptNullObject = "Compearing to null objecct is not possible!";
        public const string CompearToCanNotAcceptAnythingThanICoordinates = $"Object is not form type: {nameof(ICoordinates)}";
    }
}
