using Quoridor.Client;

try
{
    while (true)
    {
        var client = new GameClient();
        client.Start();
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    Console.ReadKey();
}