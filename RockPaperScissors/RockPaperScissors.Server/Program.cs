using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;

namespace RockPaperScissors.Server
{
    internal class Program
    {
        private static CancellationTokenSource applicationExitTokenSource = new();

        private static Task networkIoTask;
        //private static Task gameLogicTask;

        private static ConcurrentQueue<ConsoleMessage> consoleMessages = new();

        static void Main(string[] args)
        {
            Console.WriteLine("Rock, Paper, Scissors - Server");
            Console.WriteLine("Starting up...");

            networkIoTask = Task.Run(NetworkIoTaskLoop);
            applicationExitTokenSource.CancelAfter(10000);

            List<ConsoleMessage> currentMessages = new();

            do
            {
                if (consoleMessages.IsEmpty)
                {
                    Thread.Sleep(100);
                    continue;
                }

                currentMessages.Clear();
                while (consoleMessages.TryDequeue(out var message))
                {
                    currentMessages.Add(message);
                }

                foreach (ConsoleMessage consoleMessage in currentMessages.OrderBy(x => x.TimeStamp).ToList())
                {
                    Console.WriteLine(consoleMessage.Message);
                }
            }
            while (!applicationExitTokenSource.IsCancellationRequested);

            Console.WriteLine("Application is exiting...");

            Task.WaitAll(networkIoTask);

            applicationExitTokenSource.Dispose();
        }

        private static async void NetworkIoTaskLoop()
        {
            using (var udpClient = new UdpClient(11000))
            {
                while (!applicationExitTokenSource.IsCancellationRequested)
                {
                    consoleMessages.Enqueue(new ConsoleMessage("Waiting for packets..."));

                    var receivedResults = await udpClient.ReceiveAsync();
                    consoleMessages.Enqueue(new ConsoleMessage($"Received packet: { Encoding.ASCII.GetString(receivedResults.Buffer) }"));
                }
            }
        }
    }
}
