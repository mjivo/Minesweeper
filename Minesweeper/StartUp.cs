namespace Minesweeper
{
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
            //const string difficulty = "easy";
            const int x = 9;
            const int y = 9;

            ICoordinates coordinates = new Coordinates(x, y);
            IArea area = new Area(coordinates);

            IMesh mesh = new Mesh(coordinates, area);


            IWriter writer = new ConsoleWrter();
            IReader reader = new ConsoleReader();
            IEngine engine = new Engine(mesh, writer, reader);
            engine.Start();
        }
    }
}
