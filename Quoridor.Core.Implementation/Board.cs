using Quoridor.Core.Abstraction;
using Quoridor.Core.Abstraction.Common;
using System.Collections.ObjectModel;

namespace Quoridor.Core.Implementation;
public class Board : IBoard
{
    private readonly IPawn[,] _tiles;
    private readonly Dictionary<FenceDirection, Fence[,]> _passages = new();
    private readonly List<Fence> _fences = new();

    public IReadablePawn[,] Tiles => _tiles.Clone() as IPawn[,];
    public IReadOnlyList<Fence> Fences => _fences.AsReadOnly();
    public IReadOnlyDictionary<FenceDirection, Fence[,]> Passages => new ReadOnlyDictionary<FenceDirection, Fence[,]>(_passages);

    public Board(int sideSize)
    {
        if (sideSize % 2 == 1 && sideSize > 1)
        {
            _tiles = new IPawn[sideSize, sideSize];
            _passages.Add(FenceDirection.HORIZONTAL, new Fence[sideSize, sideSize - 1]);
            _passages.Add(FenceDirection.VERTICAL, new Fence[sideSize - 1, sideSize]);
        }
        else
            throw new ArgumentOutOfRangeException("Only odd positive value (except 1) is allowed!");
    }

    public bool TryMovePawn(Point coordinate)
    {
        //TODO
        return true;
    }

    //This function must be refactored!!!
    public bool TryPutFence(Point coordinate, FenceDirection fenceDirection)
    {
        Fence[,] horizontalPassages = _passages[FenceDirection.HORIZONTAL];
        Fence[,] verticalPassages = _passages[FenceDirection.VERTICAL];

        if (fenceDirection == FenceDirection.HORIZONTAL)
        {
            if (coordinate.X < horizontalPassages.GetLength(0) && coordinate.Y <= horizontalPassages.GetLength(1))
            {
                if (horizontalPassages[coordinate.X, coordinate.Y] == null && horizontalPassages[coordinate.X + 1, coordinate.Y] == null &&
                    (verticalPassages[coordinate.X, coordinate.Y] == null && verticalPassages[coordinate.X, coordinate.Y + 1] == null ||
                    verticalPassages[coordinate.X, coordinate.Y] != verticalPassages[coordinate.X, coordinate.Y + 1]))
                {
                    Point secondCoordinate = new Point(coordinate.X + 1, coordinate.Y);
                    Fence fence = new Fence(coordinate, secondCoordinate);
                    _fences.Add(fence);
                    horizontalPassages[coordinate.X, coordinate.Y] = fence;
                    horizontalPassages[coordinate.X+1, coordinate.Y] = fence;

                    return true;
                }
            }
        }
        else
        {
            if (coordinate.X <= verticalPassages.GetLength(0) && coordinate.Y < verticalPassages.GetLength(1))
            {
                if ((horizontalPassages[coordinate.X, coordinate.Y] == null && horizontalPassages[coordinate.X + 1, coordinate.Y] == null ||
                    horizontalPassages[coordinate.X, coordinate.Y] != horizontalPassages[coordinate.X + 1, coordinate.Y]) &&
                    verticalPassages[coordinate.X, coordinate.Y] == null && verticalPassages[coordinate.X, coordinate.Y+1] == null)
                {
                    Point secondCoordinate = new Point(coordinate.X, coordinate.Y+1);
                    Fence fence = new Fence(coordinate, secondCoordinate);
                    _fences.Add(fence);
                    horizontalPassages[coordinate.X, coordinate.Y] = fence;
                    horizontalPassages[coordinate.X, coordinate.Y+1] = fence;

                    return true;
                }
            }
        }

        return false;
    }
}
