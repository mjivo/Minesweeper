namespace Minesweeper.Build
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Contracts;

    using Data.Enums;
    using Data.Uttilites.Exception_messages;
    using Minesweeper.Data.Uttilites.Exceptions;

    internal class Mesh : IMesh
    {
        private int _percentageOfBombs;
        private string _difficulty;
        private int _foundBombs;
        private int _bombs;


        private bool _isAlerted;

        private List<ICoordinates> MarkedBombsCoordinates;

        private const char GameAreaSymbol = '#';

        public Mesh(ICoordinates sizes, IArea area)
        {
            this.Area = area;
            this.Sizes = sizes;

            this.MarkedBombsCoordinates = new List<ICoordinates>();
        }

        public ICoordinates Sizes { get; private set; }

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

        public int FoundedBombs
        {
            get => this._foundBombs;
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(string.Format(MeshClassExceptions.BombsCannotBeNegative, nameof(this.FoundedBombs), value));
                _foundBombs = value;
            }
        }

        public int MarkedBombs
        {
            get => this.MarkedBombsCoordinates.Count;
        }

        public void SetGameArea()
        {
            this.Area.SetGameArea(GameAreaSymbol);
        }

        public void CalucateBombsByDifficulty(string difficulty)
        {
            this.Difficulty = difficulty;
            this.Bombs = ((int)Math.Round(this.Area.Cells * this._percentageOfBombs / 100.00, 0));
        }

        public void PlantBombs()
        {
            Random random = new Random();

            for (int b = 0; b < Bombs; b++)
            {
                int bombX = random.Next(this.Sizes.X);
                int bombY = random.Next(this.Sizes.Y);
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
            if (this.Area.GameArea[coordinates.Y, coordinates.X] == '#')
            {
                this.Area.SetCellValue(coordinates, 'M');
                this.MarkedBombsCoordinates.Add(coordinates);
            }
            else
            {
                throw new MarkNotABombException();
            }
        }

        public void UnmarkCellAsABomb(ICoordinates coordinates)
        {
            if (this.Area.GameArea[coordinates.Y, coordinates.X] == 'M')
            {
                this.Area.SetCellValue(coordinates, GameAreaSymbol);
                this.MarkedBombsCoordinates.Remove(coordinates);
            }
            else
            {
                throw new UnMarkNotMarkedCellException();
            }
        }

        public void ExploreCell(ICoordinates coordinates)
        {
            if (this.Area.GameArea[coordinates.Y, coordinates.X] == 'M' && !this._isAlerted)
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

        public string GetGameArea()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"Bombs left: {this.Bombs - this.MarkedBombs - this.FoundedBombs}");
            result.AppendLine(this.Area.ToString());
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
