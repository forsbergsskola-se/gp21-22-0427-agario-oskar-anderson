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

            TcpListener server = new TcpListener(port);

            server.Start();

            Console.WriteLine("Server started...");

            while (true)
            {
                Console.WriteLine("Listening for connections...");
            
                var connectedClient = server.AcceptTcpClient();

                Console.WriteLine("Connection accepted...");

                var stream = connectedClient.GetStream();

                Console.WriteLine("Sending bytes...");
            
                stream.Write(Encoding.ASCII.GetBytes(DateTime.Now.ToString()));

                Console.WriteLine("Bytes sent...");
            
            
                Console.WriteLine("Closing down...");

                stream.Close();
                connectedClient.Close();
            }
            
            server.Stop();
        }
    }
}