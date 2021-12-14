using Qouridor.Client;

try
{
    var client = new GameClient();
    client.Start();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    Console.ReadKey();
}