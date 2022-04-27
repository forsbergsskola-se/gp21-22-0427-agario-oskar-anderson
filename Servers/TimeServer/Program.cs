using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Servers.Time
{
    class TimeServer
    {
        private const int port = 4444;
        
        static void Main()
        {
            Console.WriteLine("Starting server...");

            // IPAddress address = IPAddress.Parse("172.0.0.1");
            
            TcpListener server = new TcpListener(port);

            server.Start();

            Console.WriteLine("Server started...");
            Console.WriteLine("Listening for connections...");
            
            var connectedClient = server.AcceptTcpClient();

            Console.WriteLine("Connection accepted...");

            var stream = connectedClient.GetStream();

            byte[] p = new[] {(byte) 255, (byte) 165, (byte) 42};

            Console.WriteLine("Sending bytes...");
            
            stream.Write(Encoding.ASCII.GetBytes(DateTime.Now.ToString()));

            Console.WriteLine("Bytes sent...");

            Console.WriteLine("Closing down...");

            stream.Close();
            connectedClient.Close();
            server.Stop();
        }
    }
}