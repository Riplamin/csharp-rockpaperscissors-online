namespace RockPaperScissors.Server
{
    public class ConsoleMessage
    {
        public string Message { get; private set; }
        public long TimeStamp { get; private set; } = DateTime.UtcNow.Ticks;

        public ConsoleMessage(string message)
        {
            Message = message;
        }
    }
}
