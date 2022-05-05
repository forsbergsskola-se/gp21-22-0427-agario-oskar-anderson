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

    public IPEndPoint PlayerEndpoint;
    
    private GameServer? gameServer;
    private UdpBeacon udpBeacon;
    

    private TcpClient connectionClient;
    private StreamReader streamReader;
    private StreamWriter streamWriter;

    private UdpClient udpClient;
    
    private readonly JsonSerializerOptions serializeAllFields = new() {IncludeFields = true};

    public ConnectedPlayer(TcpClient tcpClient, GameServer gameServer, UdpBeacon udpBeacon)
    {
        this.udpBeacon = udpBeacon;
        this.gameServer = gameServer;
        EstablishConnection(tcpClient);
    }

    private void EstablishConnection(TcpClient tcpClient)
    {
        Console.WriteLine("Attempting to establish connection...");
        streamReader = new StreamReader(tcpClient.GetStream());
        streamWriter = new StreamWriter(tcpClient.GetStream());
        

        string? jsonMessage = streamReader.ReadLine();

        var networkPackage = JsonSerializer.Deserialize<NetworkPackage>(jsonMessage, serializeAllFields);
        if (networkPackage.Id == (int) NetworkProtocol.RequestType.Login)
        {
            Console.WriteLine("Connection was successful...");
            UserData = JsonSerializer.Deserialize<NetworkPackage<UserData>>(jsonMessage, serializeAllFields).Value;

            var returnPackage = new NetworkPackage<UserData>((int)NetworkProtocol.RequestType.UserData ,UserData);
            streamWriter.WriteLine(JsonSerializer.Serialize(returnPackage, serializeAllFields));
            streamWriter.Flush();
            Console.WriteLine($"Welcome to the server {UserData.UserName}{UserData.id} with color {UserData.UserColor}");
        }
        else
        {
            Console.WriteLine("Connection refused due to incorrect package id...");
            return;
        }

        connectionClient = tcpClient;
        
        CreateUdpConnection();
        AddPlayerToGameLoop();
    }

    private void CreateUdpConnection()
    {
        // Create a udp connection here to handle common packages like position updates and food information.
        

        // Try to connect over udp as well. If unsuccessful send a disconnect package over tcp and then quit the
        // connection.
    }
    
    private void AddPlayerToGameLoop()
    {
        PlayerData.PlayerId = UserData.id;

        // Add the player to the main game loop here.
        // player should be dead as default, letting them pick when to spawn.

        // We probably want to lock the list of connected players so we here wait to add the player right before the 
        // next loop iteration.
    }

    private void WaitForTcpPackages()
    {
        
    }

    public void SendTcpPackage(NetworkPackage networkPackage)
    {
        streamWriter.WriteLine(JsonSerializer.Serialize(networkPackage, serializeAllFields));
        streamWriter.Flush();
    }

    public void SendUdpPackage(NetworkPackage networkPackage)
    {
        var json = JsonSerializer.Serialize(networkPackage, serializeAllFields);
        var bytes = Encoding.UTF8.GetBytes(json);
        udpBeacon.UdpClient.Send(bytes, bytes.Length, PlayerEndpoint);
    }
}