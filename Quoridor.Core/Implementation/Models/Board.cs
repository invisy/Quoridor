
using System.Collections.ObjectModel;

namespace Quoridor.Core.Implementation.Models;
public class Board
{
    private readonly Tile[,] _tiles;
    private readonly Dictionary<FenceDirection, Fence[,]> _passages;
    private readonly List<Fence> _fences = new();

    public Tile[,] Tiles => _tiles; //FIXME
    public IReadOnlyList<Fence> Fences => _fences.AsReadOnly();
    public IReadOnlyDictionary<FenceDirection, Fence[,]> Passages => new ReadOnlyDictionary<FenceDirection, Fence[,]>(_passages);

    public Board(int sideSize)
    {
        if (sideSize % 2 == 1 && sideSize > 1)
        {
            _tiles = new Tile[sideSize, sideSize];
            _passages = new Dictionary<FenceDirection, Fence[,]>();
            _passages.Add(FenceDirection.HORIZONTAL, new Fence[sideSize, sideSize-1]);
            _passages.Add(FenceDirection.VERTICAL, new Fence[sideSize-1, sideSize]);
        }
        else
            throw new ArgumentOutOfRangeException("Only odd positive value (except 1) is allowed!");
    }

    public bool TryPutFence(int x, int y, FenceDirection fenceDirection)
    {
        //TODO
        return true;
    }
}
