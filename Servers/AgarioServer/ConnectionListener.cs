using System.Net;
using System.Net.Sockets;

namespace AgarioServer;

public class ConnectionListener
{
    public void WaitForLoginAttempts(GameServer gameServer, UdpBeacon udpBeacon)
    {
        TcpListener tcpListener = new TcpListener(IPAddress.Any, ServerSettings.Port);
        tcpListener.Start();

        Console.WriteLine("Tcp: waiting for connections on: " + tcpListener.LocalEndpoint);
        
        while (true)
        {
            var client = tcpListener.AcceptTcpClient();

            Console.WriteLine("Tcp: found connection...");
            var user = new ConnectedPlayer(client, gameServer, udpBeacon);
        }
    }
}