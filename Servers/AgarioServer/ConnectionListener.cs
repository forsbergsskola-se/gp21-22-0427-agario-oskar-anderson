using System.Net;
using System.Net.Sockets;

namespace AgarioServer;

public class ConnectionListener
{
    public void WaitForLoginAttempts()
    {
        Console.WriteLine("Waiting for connections...");
        TcpListener tcpListener = new TcpListener(IPAddress.Loopback, ServerSettings.Port);
        tcpListener.Start();

        while (true)
        {
            var client = tcpListener.AcceptTcpClient();

            var user = new ConnectedPlayer(client);
        }
    }
}