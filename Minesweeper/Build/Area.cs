namespace Minesweeper.Build
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    using Contracts;
    using Minesweeper.Data.Uttilites;
    using Minesweeper.Data.Uttilites.Exceptions;

    internal class Area : IArea
    {
        private string _topCoordinates;

        private int _cells;
        private int _exploredCells;

        private HashSet<ICoordinates> _bombsCoordinates;

        private const char GameAreaSymbol = '\u25A0';

        private Area()
        {
        }

        public Area(ICoordinates size)
            : this()
        {
            this.Size = size;
            this.GameArea = new char[this.Size.Y, this.Size.X];
            this._cells = this.Size.X * this.Size.Y;

            this._topCoordinates = this.GenerateTopCoordinates();
            this._bombsCoordinates = new HashSet<ICoordinates>();
        }
        public ICoordinates Size { get; private set; }

        public char[,] GameArea { get; private set; }

        public int Cells => this._cells;

        public int ExploredCells
        {
            get => this._exploredCells;
        }

        public IReadOnlyCollection<ICoordinates> BombsCoordinates { get => this._bombsCoordinates; }

        public char GetGameAreaSymbol()
        {
            return GameAreaSymbol;
        }

        public void AddBombCordinates(ICoordinates coordinates)
        {
            _bombsCoordinates.Add(coordinates);
        }

        public void RemoveBombCordinates(ICoordinates coordinates)
        {
            this._bombsCoordinates.Remove(coordinates);
        }

        public void ExploreCell(ICoordinates coordinates)
        {
            //lose check
            if (this._bombsCoordinates.Contains(coordinates))
            {
                this.RevealBombs();
                throw new GameEndedException(GameEndedMessages.CellIsBomb);
            }

            //explore
            this.Explorer(coordinates);

            //win check
            if (this.Cells - this._exploredCells == this.BombsCoordinates.Count)
            {
                this.RevealBombs();
                throw new GameWonException();
            }
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

        public void SetGameArea()
        {
            this.SetGameArea(GameAreaSymbol);
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

        private void RevealBombs()
        {
            foreach (ICoordinates coordinates in this._bombsCoordinates)
            {
                this.GameArea[coordinates.Y, coordinates.X] = 'B';
            }
        }

        private void Explorer(ICoordinates coordinates)
        {
            int numOfsurroundingBombs = 0;

            HashSet<ICoordinates> cellsToCheck = new HashSet<ICoordinates>();

            bool isCornered = false;
            bool isEaged = false;
            if (coordinates.X == 0 || coordinates.Y == 0 || coordinates.X == this.Size.X - 1 || coordinates.Y == this.Size.Y - 1)
            {
                isEaged = true;
                if (coordinates.X == 0 && coordinates.Y == 0
                    || coordinates.X == 0 && coordinates.Y == this.Size.Y - 1
                    || coordinates.X == this.Size.X - 1 && coordinates.Y == 0
                    || coordinates.X == this.Size.X - 1 && coordinates.Y == this.Size.Y - 1)
                {
                    isCornered = true;
                }
            }
            if (isCornered)
            {
                if (coordinates.X == 0)
                {
                    if (coordinates.Y == 0)//left up corrner
                    {
                        ICoordinates rightCell = new Coordinates(coordinates.X + 1, coordinates.Y);
                        ICoordinates downLeftCell = new Coordinates(coordinates.X + 1, coordinates.Y + 1);
                        ICoordinates downCell = new Coordinates(coordinates.X, coordinates.Y + 1);

                        cellsToCheck = new HashSet<ICoordinates>()
                        {
                            rightCell,
                            downLeftCell,
                            downCell,
                        };
                    }
                    else if (coordinates.Y == this.Size.Y - 1)//left down corrner
                    {
                        ICoordinates rightCell = new Coordinates(coordinates.X + 1, coordinates.Y);
                        ICoordinates rightUpCell = new Coordinates(coordinates.X + 1, coordinates.Y - 1);
                        ICoordinates upCell = new Coordinates(coordinates.X, coordinates.Y - 1);

                        cellsToCheck = new HashSet<ICoordinates>()
                        {
                            rightCell,
                            rightUpCell,
                            upCell,
                        };
                    }
                }
                else if (coordinates.X == this.Size.X - 1)
                {
                    if (coordinates.Y == 0)
                    {
                        ICoordinates leftCell = new Coordinates(coordinates.X - 1, coordinates.Y);
                        ICoordinates leftDownCell = new Coordinates(coordinates.X - 1, coordinates.Y + 1);
                        ICoordinates downCell = new Coordinates(coordinates.X, coordinates.Y + 1);

                        cellsToCheck = new HashSet<ICoordinates>()
                        {
                            leftCell,
                            leftDownCell,
                            downCell,
                        };
                    }
                    else if (coordinates.Y == this.Size.Y - 1)
                    {
                        ICoordinates leftCell = new Coordinates(coordinates.X - 1, coordinates.Y);
                        ICoordinates leftUpCell = new Coordinates(coordinates.X - 1, coordinates.Y - 1);
                        ICoordinates upCell = new Coordinates(coordinates.X, coordinates.Y - 1);

                        cellsToCheck = new HashSet<ICoordinates>()
                       {
                           leftCell,
                           leftUpCell,
                           upCell
                       };

                    }
                }
            }
            else if (isEaged)
            {
                if (coordinates.Y == 0)
                {
                    ICoordinates leftCell = new Coordinates(coordinates.X - 1, coordinates.Y);
                    ICoordinates rightCell = new Coordinates(coordinates.X + 1, coordinates.Y);
                    ICoordinates downLeftCell = new Coordinates(coordinates.X - 1, coordinates.Y + 1);
                    ICoordinates downCell = new Coordinates(coordinates.X, coordinates.Y + 1);
                    ICoordinates downRightCell = new Coordinates(coordinates.X + 1, coordinates.Y + 1);

                    cellsToCheck = new HashSet<ICoordinates>()
                    {
                        leftCell,
                        rightCell,
                        downRightCell,
                        downCell,
                        downLeftCell,
                    };
                }
                else if (coordinates.Y == this.Size.Y - 1)
                {
                    ICoordinates leftCell = new Coordinates(coordinates.X - 1, coordinates.Y);
                    ICoordinates rightCell = new Coordinates(coordinates.X + 1, coordinates.Y);
                    ICoordinates upLeftCell = new Coordinates(coordinates.X - 1, coordinates.Y - 1);
                    ICoordinates upCell = new Coordinates(coordinates.X, coordinates.Y - 1);
                    ICoordinates upRightCell = new Coordinates(coordinates.X + 1, coordinates.Y - 1);

                    cellsToCheck = new HashSet<ICoordinates>()
                    {
                        leftCell,
                        rightCell,
                        upLeftCell,
                        upCell,
                        upRightCell,
                    };
                }
                else if (coordinates.X == 0)
                {
                    ICoordinates upCell = new Coordinates(coordinates.X, coordinates.Y - 1);
                    ICoordinates downCell = new Coordinates(coordinates.X, coordinates.Y + 1);
                    ICoordinates rightUpCell = new Coordinates(coordinates.X + 1, coordinates.Y - 1);
                    ICoordinates rightCell = new Coordinates(coordinates.X + 1, coordinates.Y);
                    ICoordinates rightDownCell = new Coordinates(coordinates.X + 1, coordinates.Y + 1);

                    cellsToCheck = new HashSet<ICoordinates>()
                    {
                        upCell,
                        downCell,
                        rightUpCell,
                        rightCell,
                        rightDownCell,
                    };
                }
                else if (coordinates.X == this.Size.X - 1)
                {
                    ICoordinates upCell = new Coordinates(coordinates.X, coordinates.Y - 1);
                    ICoordinates downCell = new Coordinates(coordinates.X, coordinates.Y + 1);
                    ICoordinates leftUpCell = new Coordinates(coordinates.X - 1, coordinates.Y - 1);
                    ICoordinates leftCell = new Coordinates(coordinates.X - 1, coordinates.Y);
                    ICoordinates leftDownCell = new Coordinates(coordinates.X - 1, coordinates.Y + 1);

                    cellsToCheck = new HashSet<ICoordinates>()
                    {
                        upCell,
                        downCell,
                        leftUpCell,
                        leftCell,
                        leftDownCell,
                    };
                }
            }
            else
            {
                ICoordinates upLeftCell = new Coordinates(coordinates.X - 1, coordinates.Y - 1);
                ICoordinates upCell = new Coordinates(coordinates.X, coordinates.Y - 1);
                ICoordinates upRightCell = new Coordinates(coordinates.X + 1, coordinates.Y - 1);
                ICoordinates leftCell = new Coordinates(coordinates.X - 1, coordinates.Y);
                ICoordinates rightCell = new Coordinates(coordinates.X + 1, coordinates.Y);
                ICoordinates downLeftCell = new Coordinates(coordinates.X - 1, coordinates.Y + 1);
                ICoordinates downCell = new Coordinates(coordinates.X, coordinates.Y + 1);
                ICoordinates downRightCell = new Coordinates(coordinates.X + 1, coordinates.Y + 1);

                cellsToCheck = new HashSet<ICoordinates>()
                {
                        upLeftCell,
                        upCell,
                        upRightCell,
                        leftCell,
                        rightCell,
                        downLeftCell,
                        downCell,
                        downRightCell,
                };
            }

            
            this._exploredCells++;
            foreach (ICoordinates cords in cellsToCheck)
            {
                if (this._bombsCoordinates.Contains(cords))
                {
                    numOfsurroundingBombs++;
                }
            }

            this.SetCellValue(coordinates, $"{numOfsurroundingBombs}"[0]);
            if (numOfsurroundingBombs == 0)
            {
                foreach (ICoordinates cords in cellsToCheck)
                {
                    if (this.GameArea[cords.Y, cords.X] == GameAreaSymbol)
                    {
                        this.Explorer(cords);
                    }
                }
            }
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
