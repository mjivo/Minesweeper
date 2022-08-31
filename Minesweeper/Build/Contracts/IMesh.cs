namespace Minesweeper.Build.Contracts
{
    internal interface IMesh
    {
        public IArea Area { get; }

        public string Difficulty { get; }

        public int Bombs { get; }

        public int MarkedBombs { get; }

        public void SetGameArea();

        public void CalucateBombsByDifficulty();

        public void PlantBombs();

        public bool IsCellExplored(ICoordinates coordinates);

        public void MarkCellAsABomb(ICoordinates coordinates);

        public void UnmarkCellAsABomb(ICoordinates coordinates);

        public void ExploreCell(ICoordinates coordinates);

        //public string GetGameArea();

        //public string GetGameAreaRevealedBombs();

        public string GetStats();
    }
}
