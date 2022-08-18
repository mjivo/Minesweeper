namespace Minesweeper.Build
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using Contracts;
    using Minesweeper.Data.Uttilites;
    using Minesweeper.Data.Uttilites.Exceptions;

    internal class Area : IArea
    {
        private int _cells;
        private HashSet<ICoordinates> _bombsCoordinates;
        private string _topCoordinates;
        private Area()
        {
            //this._bombsCoordinates.Add(new Coordinates(0, 1));
        }
        public Area(ICoordinates coordinates)
            : this()
        {
            this.Size = coordinates;
            this.GameArea = new char[this.Size.Y, this.Size.X];
            this._cells = this.Size.X * this.Size.Y;

            this._topCoordinates = this.GenerateTopCoordinates();

            this._bombsCoordinates = new HashSet<ICoordinates>();
        }

        public ICoordinates Size { get; private set; }

        public char[,] GameArea { get; private set; }

        public int Cells => this._cells;

        public IReadOnlyCollection<ICoordinates> BombsCoordinates { get => this._bombsCoordinates; }

        public void AddBombCordinates(ICoordinates coordinates)
        {
            _bombsCoordinates.Add(coordinates);
        }

        public void RemoveBombCordinates(ICoordinates coordinates)
        {
            throw new System.NotImplementedException();
        }

        public void ExploreCell(ICoordinates coordinates)
        {
            //check if cell is a bomb
            if (this._bombsCoordinates.Contains(coordinates))
            {
                throw new GameEndedException(GameEndedMessages.CellIsBomb);
            }

            int numOfSurroundingBombs = NumOfSurroundingBombs(coordinates);
            if (numOfSurroundingBombs == 8)
            {
                if (this._bombsCoordinates.Contains(coordinates))
                {
                    this._bombsCoordinates.Remove(coordinates);
                }
            }
            if (numOfSurroundingBombs == 0)
            {

            }
            this.SetCellValue(coordinates, $"{numOfSurroundingBombs}"[0]);
        }

        public void SetCellValue(ICoordinates coordinates, char symbol)
        {
            this.GameArea[coordinates.Y, coordinates.X] = symbol;
        }

        public void SetGameArea(char symbol)
        {
            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    this.GameArea[x, y] = symbol;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for (int y = 0; y < Size.Y; y++)
            {
                result.Append(y + 1 + " ");
                for (int x = 0; x < Size.X; x++)
                {
                    result.Append(GameArea[y, x] + " ");
                }
                result.AppendLine();
            }
            result.Insert(0, "* " + this._topCoordinates + Environment.NewLine);
            return result.ToString().TrimEnd();
        }

        private int NumOfSurroundingBombs(ICoordinates coordinates)
        {
            int numOfsurroundingBombs = 0;

            bool isCornered = false;
            bool isEaged = false;
            if (coordinates.X == 0 || coordinates.Y == 0)
            {
                isEaged = true;
                if (coordinates.X == 0 && coordinates.Y == 0
                    || coordinates.X == 0 && coordinates.Y == this.Size.Y
                    || coordinates.X == this.Size.X && coordinates.Y == 0
                    || coordinates.X == this.Size.X && coordinates.Y == this.Size.Y)
                {
                    isCornered = true;
                }
            }
            if (isCornered)
            {
                if (coordinates.X == 0)
                {
                    if (coordinates.Y == 0)//right up corrner
                    {
                        ICoordinates leftCell = new Coordinates(coordinates.X + 1, coordinates.Y);
                        ICoordinates downLeftCell = new Coordinates(coordinates.X + 1, coordinates.Y + 1);
                        ICoordinates downCell = new Coordinates(coordinates.X, coordinates.Y + 1);

                        HashSet<ICoordinates> cellsToCheck = new HashSet<ICoordinates>()
                        {
                            leftCell,
                            downLeftCell,
                            downCell,
                        };
                        foreach (ICoordinates cords in cellsToCheck)
                        {
                            if (this._bombsCoordinates.Contains(cords))
                            {
                                numOfsurroundingBombs++;
                            }
                        }
                    }
                }
            }
            else if (isEaged)
            {

            }

            return numOfsurroundingBombs;
        }

        private string GenerateTopCoordinates()
        {
            StringBuilder topCoordinates = new StringBuilder();

            for (int x = 0; x < Size.X; x++)
            {
                topCoordinates.Append(x + 1 + " ");
            }
            return topCoordinates.ToString().TrimEnd();
        }
    }
}
