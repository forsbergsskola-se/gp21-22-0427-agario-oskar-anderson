using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace AgarioServer;

public class UdpBeacon
{
    private GameServer gameServer;
    
    public UdpClient UdpClient = new(ServerSettings.Port);
    public Dictionary<IPEndPoint, ConnectedPlayer> clientEndpoints = new();
    
    private readonly JsonSerializerOptions serializeAllFields = new() {IncludeFields = true};



    public UdpBeacon(GameServer gameServer)
    {
        this.gameServer = gameServer;
    }
    
    
    
    public void ListenForPackages()
    {
        while (true)
        {
            Console.WriteLine("Udp: waiting for package...");
            IPEndPoint remoteEndpoint = null;
            var Json = Encoding.UTF8.GetString(UdpClient.Receive(ref remoteEndpoint));
            Console.WriteLine(Json);
            Console.WriteLine("Udp: received package...");

            if (clientEndpoints.ContainsKey(remoteEndpoint))
            {
                Console.WriteLine("Udp: endpoint was known, passing data to connected player...");
                // If package sender is known: Set the correct playerdata to the package information.
                // TODO: We want to add a package id check to make sure the received package actually is a PlayerData one.
            
                // TODO: This will eventually be replaced to handle a different data type for both food eaten and PlayerData
            
                var playerData = JsonSerializer.Deserialize<NetworkPackage<PlayerData>>(Json, serializeAllFields).Value;
                clientEndpoints[remoteEndpoint].PlayerData = playerData;
            }
            else
            {
                Console.WriteLine("Udp: endpoint was not known, attempting to add...");
                var package = JsonSerializer.Deserialize<NetworkPackage>(Json, serializeAllFields);

                if (package.Id == PackageType.UserData)
                {
                    var userData = JsonSerializer.Deserialize<NetworkPackage<UserData>>(Json, serializeAllFields).Value;

                    var possibleConnectedPlayer = gameServer.PendingConnections.First(p => p.UserData.id == userData.id);
                    if (possibleConnectedPlayer != null)
                    {
                        Console.WriteLine("Udp: successfully added new player...");
                        clientEndpoints[remoteEndpoint] = possibleConnectedPlayer;
                        possibleConnectedPlayer.UdpConnection.PlayerEndpoint = remoteEndpoint;
                        possibleConnectedPlayer.UdpConnection.UdpConnectionComplete.Set();
                    }
                }
            }
        }
    }
}