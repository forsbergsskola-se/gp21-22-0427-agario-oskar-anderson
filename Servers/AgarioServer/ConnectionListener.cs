﻿using System.Net;
using System.Net.Sockets;

namespace AgarioServer;

public class ConnectionListener
{
    public void WaitForLoginAttempts(GameServer gameServer)
    {
        TcpListener tcpListener = new TcpListener(IPAddress.Any, ServerSettings.Port);
        tcpListener.Start();

        Console.WriteLine("Waiting for connections on: " + tcpListener.LocalEndpoint);
        
        while (true)
        {
            var client = tcpListener.AcceptTcpClient();

            Console.WriteLine("Found connection...");
            var user = new ConnectedPlayer(client, gameServer);
        }
    }
}