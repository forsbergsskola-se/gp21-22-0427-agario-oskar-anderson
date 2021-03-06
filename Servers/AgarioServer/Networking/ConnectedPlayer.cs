using System.Net.Sockets;
using System.Numerics;
using AgarioServer.Data;

namespace AgarioServer.Networking;

public class ConnectedPlayer
{
    public UserData UserData;
    public PlayerData PlayerData = new();
    
    public GameServer gameServer;

    public UdpConnection UdpConnection;
    public TcpConnection TcpConnection;


    public ConnectedPlayer(TcpClient tcpClient, GameServer gameServer, UdpBeacon udpBeacon)
    {
        Console.WriteLine("Attempting to establish connection...");
        UdpConnection = new UdpConnection(udpBeacon);
        TcpConnection = new TcpConnection(tcpClient, this);
        this.gameServer = gameServer;

        gameServer.PendingConnections.Add(this);

        // We only need to wait for the Udp connection because it won't finish before a tcp connection has been 
        // established.
        WaitForUdpConnection();

        PlayerData.PlayerId = UserData.id;
        SendCurrentPlayers();
        SendCurrentFoodPositions();
        
        gameServer.AddPlayerToGameLoop(this);
    }

    private void SendCurrentPlayers()
    {
        if (gameServer.playerManager.Players.Count == 0)
            return;
        
        UserData[] userData = new UserData[gameServer.playerManager.Players.Count];
        for (int i = 0; i < userData.Length; i++)
        {
            userData[i] = gameServer.playerManager.Players[i].UserData;
        }

        var currentUsersPackage = new NetworkPackage<UserData[]>(PackageType.NewUsers, userData);
        TcpConnection.SendTcpPackage(currentUsersPackage);
    }

    private void SendCurrentFoodPositions()
    {
        var foodPackage = gameServer.foodControl.GetCurrentFoodPositionsPackage();
        TcpConnection.SendTcpPackage(foodPackage);
    }
    
    private void WaitForUdpConnection()
    {
        UdpConnection.UdpConnectionComplete.WaitOne();
    }

    public void Disconnect()
    {
        gameServer.RemovePlayerFromGameLoop(this);
    }
}