using System.Net;
using System.Net.Sockets;

namespace AgarioServer;

public class ConnectionListener
{
    public void WaitForLoginAttempts(GameServer gameServer)
    {
        Console.WriteLine("Waiting for connections...");
        TcpListener tcpListener = new TcpListener(IPAddress.Any, ServerSettings.Port);
        tcpListener.Start();

        while (true)
        {
            var client = tcpListener.AcceptTcpClient();

            Console.WriteLine("Found connection...");
            var user = new ConnectedPlayer(client, gameServer);
        }
    }
}