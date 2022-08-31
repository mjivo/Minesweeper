namespace Minesweeper
{
    using System;

    using Core;
    using Core.Contracts;
    using Build;
    using Build.Contracts;
    using IO;
    using IO.Contracts;

    internal class StartUp
    {
        static void Main()
        {
            const string difficulty = "easy";
            const int x = 9;
            const int y = 9;

            while (true)
            {
                ICoordinates size = new Coordinates(x, y);
                IArea area = new Area(size);

                IMesh mesh = new Mesh(area, difficulty);


                IConsoleWriter consoleWriter = new ConsoleWrter();
                IReader reader = new ConsoleReader();
                IEngine engine = new Engine(mesh, consoleWriter, reader);

                if (engine.Start())
                {
                    continue;
                }
                Environment.Exit(0);
            }
        }
    }
}
