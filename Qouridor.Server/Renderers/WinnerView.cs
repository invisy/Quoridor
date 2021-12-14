namespace Qouridor.Server.Renderers;

static class WinnerMessageRenderer
{
    public static string Render(string winner)
    {
         return @$"Game ended\r\nCongratulations to {winner}!";
    }
}
