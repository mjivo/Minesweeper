using System.Collections.Generic;

namespace Minesweeper.Build.Contracts
{
    internal interface IArea
    {
        public ICoordinates Size { get; }

        public char[,] GameArea { get; }

        public int Cells { get; } //return the nuber of cells

        public IReadOnlyCollection<ICoordinates> BombsCoordinates { get; }

        public void SetGameArea(char symbol);

        public void AddBombCordinates(ICoordinates coordinates);

        public void RemoveBombCordinates(ICoordinates coordinates);

        public void ExploreCell(ICoordinates coordinates);

        public void SetCellValue(ICoordinates coordinates, char symbol);

    }
}
