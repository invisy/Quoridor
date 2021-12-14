namespace Qouridor.Contracts.Responses
{
    public class Responses
    {
        public BoardUpdatedResponse BoardUpdatedResponse { get; set; }
        public GameFinishedResponse GameFinishedResponse { get; set; }
        public GameStartedResponse GameStartedResponse { get; set; }
        
        public ErrorResponse ErrorResponse { get; set; }
    }
}
