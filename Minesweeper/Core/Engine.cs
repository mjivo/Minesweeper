namespace Minesweeper.Core
{
    using System;


    using Contracts;
    using Build.Contracts;
    using Minesweeper.Build;
    using Minesweeper.IO.Contracts;
    using Minesweeper.Data.Uttilites.Exceptions;
    using Minesweeper.Data.Uttilites.OutputMessages;

    internal class Engine : IEngine
    {
        private IMesh _mesh;
        private const string Difficulty = "easy";
        private IWriter _writer;
        private IReader _reader;
        public Engine(IMesh mesh, IWriter writer, IReader reader)//dependancy ingections
        {
            this._mesh = mesh;
            this._reader = reader;
            this._writer = writer;
        }

        public void Start()
        {
            //reset
            //ask for difficulty
            this._mesh.SetGameArea();
            this._mesh.CalucateBombsByDifficulty(Difficulty);
            this._mesh.PlantBombs();

            this._writer.Write(this._mesh.GetGameArea());


            while (true)
            {
                ICoordinates coordinatesSelectedByUser = null;
                try
                {
                    coordinatesSelectedByUser = GetCordinates();

                    if (this._mesh.IsCellExplored(coordinatesSelectedByUser))
                    {
                        this._writer.WriteLine("This cell is already explored. Try with ather coordinates.");
                        continue;
                    }
                    this._writer.WriteLine("Enter /m to select the cell as marked bomb and /u to unmarked already marked cell or enter /e to explore the cell:");
                    string input = this._reader.ReadLine();
                    if (input.ToLower() == "/m")
                    {
                        this._mesh.MarkCellAsABomb(coordinatesSelectedByUser);
                        this._writer.WriteLine($"You have marked cell [{coordinatesSelectedByUser.X + 1}][{coordinatesSelectedByUser.Y + 1}] as bomb.");
                    }
                    else if (input.ToLower() == "/u")
                    {
                        this._mesh.UnmarkCellAsABomb(coordinatesSelectedByUser);
                    }
                    else if (input.ToLower() == "/e")
                    {
                        this._mesh.ExploreCell(coordinatesSelectedByUser);
                    }
                    this._writer.Write(this._mesh.GetGameArea());
                }
                catch (MarkNotABombException marknotabombexception)
                {
                    this._writer.WriteLine(marknotabombexception.Message);
                }
                catch (UnMarkNotMarkedCellException unMarkNotMarkedCellException)
                {
                    this._writer.WriteLine(unMarkNotMarkedCellException.Message);
                }
                catch (ExploreMarkedCellWarrningException exploreMarkedCellWarrningException)
                {
                    while (true)
                    {
                        this._writer.WriteLine(exploreMarkedCellWarrningException.Message);
                        this._writer.WriteLine("Y/N");
                        string input = this._reader.ReadLine();
                        if (input.Length == 0 || input.Length > 1)
                        {
                            continue;
                        }
                        else if (input.ToLower()[0] != 'y' && input.ToLower()[0] != 'n')
                        {
                            continue;
                        }
                        else
                        {
                            if (input.ToLower()[0] == 'y')
                            {
                                this._mesh.ExploreCell(coordinatesSelectedByUser);
                            }
                            this._writer.Write(this._mesh.GetGameArea());
                            break;
                        }
                    }
                }
                catch (GameEndedException gameEndedException)
                {
                    this._writer.WriteLine(gameEndedException.Message);
                    //this._writer.WriteLine(this._mesh.Stats());
                }
                catch (Exception exception)
                {
                    this._writer.WriteLine(exception.Message);
                    throw;
                }
            }
        }

        private ICoordinates GetCordinates()
        {
            while (true)
            {
                try
                {
                    this._writer.Write("Enter 'X' - coordinate:");
                    int x = int.Parse(this._reader.ReadLine());
                    if (x > this._mesh.Sizes.X || x <= 0)
                    {
                        this._writer.WriteLine(string.Format(EngineOutputMessages.CoordinatesOutOfTheGameAreaSize, this._mesh.Sizes.X));
                        continue;
                    }

                    this._writer.Write("Enter 'Y' - coordinate:");
                    int y = int.Parse(this._reader.ReadLine());
                    if (y > this._mesh.Sizes.Y || x <= 0)
                    {
                        this._writer.WriteLine(string.Format(EngineOutputMessages.CoordinatesOutOfTheGameAreaSize, this._mesh.Sizes.Y));
                        continue;
                    }

                    ICoordinates coordinatesSelectedByUser = new Coordinates(x - 1, y - 1);
                    return coordinatesSelectedByUser;
                }
                catch (ArgumentNullException)
                {
                    this._writer.WriteLine(EngineOutputMessages.CoordinateseCannotBeNull);
                    continue;
                }
                catch (FormatException)
                {
                    this._writer.WriteLine(EngineOutputMessages.EnterValidCoordinates);
                    continue;
                }
                catch (OverflowException)
                {
                    this._writer.WriteLine(EngineOutputMessages.NumberOverflow);
                }
                catch (ArgumentException)
                {
                    this._writer.WriteLine(EngineOutputMessages.CoordinatesCannotBeNegative);
                    continue;
                }
            }
        }
    }
}
