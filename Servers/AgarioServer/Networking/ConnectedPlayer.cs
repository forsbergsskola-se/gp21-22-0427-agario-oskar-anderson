using System.Net.Sockets;
using AgarioServer.Data;

namespace AgarioServer.Networking;

public class ConnectedPlayer
{
    public UserData UserData;
    public PlayerData PlayerData = new PlayerData();
    
    private GameServer gameServer;

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
        
        gameServer.AddPlayerToGameLoop(this);
    }

    private void SendCurrentPlayers()
    {
        if (gameServer.Players.Count == 0)
            return;
        
        UserData[] userData = new UserData[gameServer.Players.Count];
        for (int i = 0; i < userData.Length; i++)
        {
            userData[i] = gameServer.Players[i].UserData;
        }

        var currentUsersPackage = new NetworkPackage<UserData[]>(PackageType.NewUsers, userData);
        TcpConnection.SendTcpPackage(currentUsersPackage);
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