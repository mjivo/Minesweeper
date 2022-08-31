namespace Minesweeper.Build
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    using Contracts;
    using Data.Enums;
    using Data.Uttilites.Exception_messages;
    using Minesweeper.Data.Uttilites.Exceptions;

    internal class Mesh : IMesh
    {
        private bool _isAlerted;

        private string _difficulty;
        private int _percentageOfBombs;
        private int _bombs;

        private const char FlagedBombSymbol = '\u2690';

        private List<ICoordinates> MarkedBombsCoordinates;

        public Mesh(IArea area, string difficulty)
        {
            this.Area = area;
            this.Difficulty = difficulty;

            this.MarkedBombsCoordinates = new List<ICoordinates>();

            this.SetGameArea();
            this.CalucateBombsByDifficulty();
            this.PlantBombs();
        }

        public IArea Area { get; private set; }

        public string Difficulty
        {
            get => _difficulty;
            private set
            {
                ValidateDifficulty(value);
            }
        }

        public int Bombs
        {
            //get => this.Area.BombsCoordinates.Count;
            get => this._bombs;
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(string.Format(MeshClassExceptions.BombsCannotBeNegative, nameof(this.Bombs), value));
                this._bombs = value;
            }
        }

        public int MarkedBombs
        {
            get => this.MarkedBombsCoordinates.Count;
        }

        public void SetGameArea()
        {
            this.Area.SetGameArea();
        }

        public void CalucateBombsByDifficulty()
        {
            this.Bombs = ((int)Math.Ceiling(this.Area.Cells * this._percentageOfBombs / 100.00));
        }

        public void PlantBombs()
        {
            Random random = new Random();

            for (int b = 0; b < Bombs; b++)
            {
                int bombX = random.Next(this.Area.Size.X);
                int bombY = random.Next(this.Area.Size.Y);
                ICoordinates coordinates = new Coordinates(bombX, bombY);
                this.Area.AddBombCordinates(coordinates);
            }
        }

        public bool IsCellExplored(ICoordinates coordinates)
        {
            return char.IsDigit(this.Area.GameArea[coordinates.Y, coordinates.X]);
        }

        public void MarkCellAsABomb(ICoordinates coordinates)
        {
            if (this.Area.GameArea[coordinates.Y, coordinates.X] == this.Area.GetGameAreaSymbol())
            {
                this.Area.SetCellValue(coordinates, FlagedBombSymbol);
                this.MarkedBombsCoordinates.Add(coordinates);
            }
            else
            {
                throw new MarkNotABombException();
            }
        }

        public void UnmarkCellAsABomb(ICoordinates coordinates)
        {
            if (this.Area.GameArea[coordinates.Y, coordinates.X] == FlagedBombSymbol)//check if cell is flaged before unflaging
            {
                this.Area.SetCellValue(coordinates, this.Area.GetGameAreaSymbol());
                this.MarkedBombsCoordinates.Remove(coordinates);
            }
            else
            {
                throw new UnMarkNotMarkedCellException();
            }
        }

        public void ExploreCell(ICoordinates coordinates)
        {
            if (this.Area.GameArea[coordinates.Y, coordinates.X] == FlagedBombSymbol && !this._isAlerted)// throw a warning
            {
                this._isAlerted = true;
                throw new ExploreMarkedCellWarrningException();
            }
            else
            {
                this.Area.ExploreCell(coordinates);
                _isAlerted = false;
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            //result.AppendLine($"Bombs left: {this.Bombs - this.MarkedBombs}");
            result.AppendLine(this.Area.ToString());
            return result.ToString();
        }

        public string GetStats()
        {
            StringBuilder result = new StringBuilder();

            return result.ToString();
        }

        private void ValidateDifficulty(string value)
        {
            switch (value.ToLower())
            {
                case "easy":
                    this._difficulty = value;
                    this._percentageOfBombs = ((int)Difficulties.easy);
                    break;
                case "medium":
                    this._difficulty = value;
                    this._percentageOfBombs = ((int)Difficulties.medium);
                    break;
                case "hard":
                    this._difficulty = value;
                    this._percentageOfBombs = ((int)Difficulties.hard);
                    break;
                case "verryHard":
                    this._difficulty = value;
                    this._percentageOfBombs = ((int)Difficulties.verryHard);
                    break;
                case "unplayable":
                    this._difficulty = value;
                    this._percentageOfBombs = ((int)Difficulties.unplayable);
                    break;
                default:
                    throw new InvalidOperationException(String.Format(MeshClassExceptions.InvalidMeshDifficulty, value));
            }
        }
    }
}
