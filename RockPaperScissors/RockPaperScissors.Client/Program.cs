using System.Net.Sockets;
using System.Text;

namespace RockPaperScissors.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Rock, Paper, Scissors - Client");

            string sendMessage;
            bool firstInput = true;
            do
            {
                if (!firstInput)
                {
                    Console.WriteLine("You didn't enter anything...");
                }
                Console.Write("Please enter a message: ");
                sendMessage = Console.ReadLine();
                firstInput = false;
            }
            while (sendMessage != null && sendMessage.Length <= 0);

            using (var udpClient = new UdpClient())
            {
                udpClient.Connect("localhost", 11000);
                Console.WriteLine("Sending message bytes...");
                udpClient.Send(Encoding.ASCII.GetBytes(sendMessage));
            }

            Console.WriteLine("Application is exiting...");
        }
    }
}
