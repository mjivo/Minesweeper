namespace Minesweeper.Build.Contracts
{
    internal interface IMesh
    {
        public IArea Area { get; }

        public ICoordinates Sizes { get; }

        public string Difficulty { get; }

        public int Bombs { get; }

        public int FoundedBombs { get; }

        public int MarkedBombs { get; }

        public void SetGameArea();

        public void CalucateBombsByDifficulty(string difficulty);

        public void PlantBombs();

        public bool IsCellExplored(ICoordinates coordinates);

        public void MarkCellAsABomb(ICoordinates coordinates);

        public void UnmarkCellAsABomb(ICoordinates coordinates);

        public void ExploreCell(ICoordinates coordinates);

        public string GetGameArea();
    }
}
