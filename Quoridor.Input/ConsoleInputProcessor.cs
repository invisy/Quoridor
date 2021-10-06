using Quoridor.Core.Abstraction.Common;

namespace Quoridor.Input;

public class ConsoleInputProcessor
{
    public delegate void MovePlayer(Point point);
    public delegate void PlaceWall(Point point, FenceDirection direction);
    public event MovePlayer MovePlayerEvent;
    public event PlaceWall PlaceWallEvent;

    public ConsoleInputProcessor()
    {
        string command = null;
        Console.WriteLine("Enter command: ");
        while (true)
        {
            command = Console.ReadLine();
            var splitCommand = command.Split(new char[0]);
            switch (splitCommand[0].ToLower())
            {
                case "pawn":
                    var x = int.Parse(splitCommand[1]);
                    var y = int.Parse(splitCommand[2]);
                    Point point = new Point(x, y);
                    MovePlayerEvent?.Invoke(point);
                    break;
                    //TODO
            }

        }
    }
}
