namespace Minesweeper.Core
{
    using System;
    using System.Threading;

    using Contracts;
    using Build;
    using Build.Contracts;
    using IO.Contracts;
    using Data.Uttilites.Exceptions;
    using Data.Uttilites.OutputMessages;

    internal class Engine : IEngine
    {

        private IMesh _mesh;
        private IConsoleWriter _consoleWriter;
        private IReader _reader;

        private const ConsoleColor InputMessagesColor = ConsoleColor.Green;
        private const ConsoleColor ErrorMessagesColor = ConsoleColor.DarkRed;

        private const char SplitInputSymbol = ',';

        private Engine()
        {
            Console.SetWindowPosition(0, 0);
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        public Engine(IMesh mesh, IConsoleWriter consoleWriter, IReader reader)
            : this()//dependancy ingections
        {
            this._mesh = mesh;
            this._reader = reader;
            this._consoleWriter = consoleWriter;

            this._consoleWriter.WriteLine(EngineOutputMessages.GameStartsMessage);
            this._consoleWriter.Write(this._mesh.ToString());
            this._consoleWriter.WriteLine(EngineOutputMessages.EnterCommand, InputMessagesColor);
        }

        public bool Start()
        {
            while (true)
            {
                string command = string.Empty;
                this._consoleWriter.WriteLine(EngineOutputMessages.EnterCoordinates, InputMessagesColor);
                ICoordinates coordinatesSelectedByUser = GetCordinates(out command);
                command = command.ToLower();
                try
                {
                    if (command == "f")
                    {
                        this._mesh.MarkCellAsABomb(coordinatesSelectedByUser);
                    }
                    else if (command == "u")
                    {
                        this._mesh.UnmarkCellAsABomb(coordinatesSelectedByUser);
                    }
                    else if (command == "e")
                    {
                        this._mesh.ExploreCell(coordinatesSelectedByUser);
                    }
                    else
                    {
                        this._consoleWriter.WriteLine(EngineOutputMessages.UnknownCommand, ErrorMessagesColor);
                        continue;
                    }

                    Console.ResetColor();
                    this.PrintGameAreaCoolurful();
                    Thread.Sleep(450);
                    Console.Beep();

                }
                catch (MarkNotABombException marknotabombexception)
                {
                    this._consoleWriter.WriteLine(marknotabombexception.Message, ErrorMessagesColor);
                }
                catch (UnMarkNotMarkedCellException unMarkNotMarkedCellException)
                {
                    this._consoleWriter.WriteLine(unMarkNotMarkedCellException.Message, ErrorMessagesColor);
                }
                catch (ExploreMarkedCellWarrningException exploreMarkedCellWarrningException)
                {
                    while (true)
                    {
                        this._consoleWriter.WriteLine(exploreMarkedCellWarrningException.Message, ErrorMessagesColor);
                        this._consoleWriter.WriteLine("Y/N", ErrorMessagesColor);

                        command = this._reader.ReadLine();
                        if (command.Length == 0 || command.Length > 1)
                        {
                            continue;
                        }
                        else if (command.ToLower()[0] != 'y' && command.ToLower()[0] != 'n')
                        {
                            continue;
                        }
                        else
                        {
                            if (command.ToLower()[0] == 'y')
                            {
                                this._mesh.ExploreCell(coordinatesSelectedByUser);
                            }
                            this.PrintGameAreaCoolurful();

                            break;
                        }
                    }//todo
                }
                catch (GameWonException gameWonException)//won
                {
                    this.PrintGameAreaCoolurful();
                    this._consoleWriter.WriteLine(gameWonException.Message, ErrorMessagesColor);

                    return this.PlayAgain();
                }
                catch (GameEndedException gameEndedException)//lose
                {
                    this._consoleWriter.WriteLine(gameEndedException.Message, ErrorMessagesColor);
                    this.PrintGameAreaCoolurful();

                    return this.PlayAgain();
                }
                catch (Exception exception)
                {
                    this._consoleWriter.WriteLine(exception.Message, ErrorMessagesColor);
                    throw;
                }
            }
        }

        private ICoordinates GetCordinates(out string command)
        {
            while (true)
            {
                string[] args = this._reader
                    .ReadLine()
                    .Split(SplitInputSymbol,
                    StringSplitOptions.RemoveEmptyEntries);

                if (args.Length == 0)
                {
                    this._consoleWriter.WriteLine(EngineOutputMessages.EnterValidCoordinates, InputMessagesColor);
                    continue;
                }

                if (args.Length == 2)
                {
                    command = "e";
                }
                else if (args.Length == 3)
                {
                    command = args[2];
                }
                else
                {
                    this._consoleWriter.WriteLine(EngineOutputMessages.EnterValidCoordinates, InputMessagesColor);
                    continue;
                }

                string XCoordinateIn = args[0];
                string YCoordinateIn = args[1];


                int XCoordinate = int.MinValue;
                int YCoordinate = int.MinValue;
                if (AreCoordinatesValid(in XCoordinateIn, out XCoordinate, in YCoordinateIn, out YCoordinate))//Coude be returning ICoordinates
                {
                    ICoordinates coordinatesSelectedByUser = new Coordinates(x: XCoordinate, y: YCoordinate);
                    return coordinatesSelectedByUser;
                }
                this._consoleWriter.WriteLine(EngineOutputMessages.EnterValidCoordinates, InputMessagesColor);
            }

            bool AreCoordinatesValid(in string XCoordinateIn, out int XCoordinate, in string YCoordinateIn, out int YCoordinate)
            {
                XCoordinate = int.MinValue;
                YCoordinate = int.MinValue;

                //X Validations
                int x = int.MinValue;
                if (!int.TryParse(XCoordinateIn, out x))
                {
                    return false;
                }
                x--;
                if (x >= this._mesh.Area.Size.X || x < 0)
                {
                    this._consoleWriter.WriteLine(string.Format(EngineOutputMessages.CoordinatesOutOfTheGameAreaSize,
                        this._mesh.Area.Size.X), InputMessagesColor);
                    return false;
                }

                //Y Validations
                int y = int.MinValue;
                if (!int.TryParse(YCoordinateIn, out y))
                {
                    return false;
                }
                y--;
                if (y >= this._mesh.Area.Size.Y || y < 0)
                {
                    this._consoleWriter.WriteLine(string.Format(EngineOutputMessages.CoordinatesOutOfTheGameAreaSize,
                        this._mesh.Area.Size.Y), InputMessagesColor);
                    return false;
                }

                //Cell validation
                if (this._mesh.IsCellExplored(new Coordinates(x, y)))
                {
                    this._consoleWriter.WriteLine(EngineOutputMessages.CellIsAreadyExplored, InputMessagesColor);
                    return false;
                }
                XCoordinate = x;
                YCoordinate = y;
                return true;
            }
        }

        private void PrintGameAreaCoolurful()
        {
            //this._consoleWriter.WriteLine($"Bombs left: {this._mesh.Bombs - this._mesh.MarkedBombs}");

            char[,] area = this._mesh.Area.GameArea;

            this._consoleWriter.Write("* ");
            for (int i = 0; i < area.GetLength(1); i++)
            {
                this._consoleWriter.Write(i + 1 + " ");
            }
            this._consoleWriter.WriteLine();

            for (int y = 0; y < area.GetLength(0); y++)
            {
                this._consoleWriter.Write(y + 1 + " ");
                for (int x = 0; x < area.GetLength(1); x++)
                {
                    char currentSymbol = area[y, x];
                    if (char.IsDigit(currentSymbol))
                    {
                        ConsoleColor cellConsoleColor = area[y, x] switch
                        {
                            '0' => ConsoleColor.Black,
                            '1' => ConsoleColor.Blue,
                            '2' => ConsoleColor.Green,
                            '3' => ConsoleColor.DarkBlue,
                            '4' => ConsoleColor.DarkGreen,
                            '5' => ConsoleColor.Red,
                            '6' => ConsoleColor.Magenta,
                            '7' => ConsoleColor.DarkRed,
                            '8' => ConsoleColor.DarkMagenta,
                            _ => throw new ArgumentException()
                        };
                        this._consoleWriter.Write(currentSymbol + " ", cellConsoleColor);
                    }
                    else
                    {
                        this._consoleWriter.Write(area[y, x] + " ");
                    }
                }
                this._consoleWriter.WriteLine(String.Empty);
            }
        }

        private bool PlayAgain()
        {
            this._consoleWriter.Write(Environment.NewLine);

            Thread.Sleep(100);
            Console.Beep();

            this._consoleWriter.WriteLine(EngineOutputMessages.PlayAgainAskMsg, ErrorMessagesColor);
            while (true)
            {
                this._consoleWriter.WriteLine("Y/N", ErrorMessagesColor);

                string command = this._reader.ReadLine().Trim();
                if (command.Length != 1)
                {
                    continue;
                }
                else if (command.ToLower()[0] != 'y' && command.ToLower()[0] != 'n')
                {
                    continue;
                }
                else
                {
                    if (command.ToLower()[0] == 'y')
                    {
                        Console.Clear();
                        return true;
                    }
                    return false;
                }
            }

        }

    }
}
