using Quoridor.Core.Abstraction;

namespace Qouridor.Server.Renderers;

static class GameStateRender
{
    public static string Render(IEnumerable<IReadablePawn> pawnList, IReadablePawn currentPlayer)
    {
        var result = string.Empty;

        foreach (var player in pawnList)
        {
            result += player.Name + " has " + player.NumberOfFences.ToString();
            result += (player.NumberOfFences == 1 ? " fence" : " fences") + "\r\n";
        }

        result += "Current turn: " + currentPlayer.Name + "\r\n";
        
        return result;
    }
}
