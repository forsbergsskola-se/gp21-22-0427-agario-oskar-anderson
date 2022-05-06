using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace AgarioServer;

public class ConnectedPlayer
{
    public UserData UserData { get; private set; }
    public PlayerData PlayerData = new PlayerData();

    
    
    private GameServer? gameServer;

    public UdpConnection UdpConnection;
    public TcpConnection TcpConnection;

    // private TcpClient connectionClient;
    // private StreamReader streamReader;
    // private StreamWriter streamWriter;

    
    
    private readonly JsonSerializerOptions serializeAllFields = new() {IncludeFields = true};
    

    
    
    public ConnectedPlayer(TcpClient tcpClient, GameServer gameServer, UdpBeacon udpBeacon)
    {
        Console.WriteLine("Attempting to establish connection...");
        UdpConnection = new UdpConnection(udpBeacon);
        TcpConnection = new TcpConnection(tcpClient);
        this.gameServer = gameServer;

        gameServer.PendingConnections.Add(this);

        // We only need to wait for the Udp connection because it won't finish before a tcp connection has been 
        // established.
        WaitForUdpConnection();
        
        AddPlayerToGameLoop();
    }
    
    
    
    
    private void WaitForUdpConnection()
    {
        UdpConnection.UdpConnectionComplete.WaitOne();
    }
    
    private void AddPlayerToGameLoop()
    {
        PlayerData.PlayerId = UserData.id;
        
        gameServer.PendingConnections.Remove(this);
        gameServer.Players.Add(this);


        // Add the player to the main game loop here.
        // player should be dead as default, letting them pick when to spawn.

        // We probably want to lock the list of connected players so we here wait to add the player right before the 
        // next loop iteration.
    }
}