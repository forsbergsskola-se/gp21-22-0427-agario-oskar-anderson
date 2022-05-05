using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

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
        IPEndPoint remoteEndpoint = null;
        var Json = Encoding.UTF8.GetString(UdpClient.Receive(ref remoteEndpoint));

        if (clientEndpoints.ContainsKey(remoteEndpoint))
        {
            // If package sender is known: Set the correct playerdata to the package information.
            // TODO: We want to add a package id check to make sure the received package actually is a PlayerData one.
            
            // TODO: This will eventually be replaced to handle a different data type for both food eaten and PlayerData
            
            var playerData = JsonSerializer.Deserialize<NetworkPackage<PlayerData>>(Json).Value;
            clientEndpoints[remoteEndpoint].PlayerData = playerData;
        }
        else
        {
            var package = JsonSerializer.Deserialize<NetworkPackage>(Json);

            if (package.Id == (int) NetworkProtocol.RequestType.Login)
            {
                var userData = JsonSerializer.Deserialize<NetworkPackage<UserData>>(Json, serializeAllFields).Value;

                var possibleConnectedPlayer = gameServer.PendingConnections.First(x => x.PlayerData.PlayerId == userData.id);
                if (possibleConnectedPlayer != null)
                {
                    clientEndpoints[remoteEndpoint] = possibleConnectedPlayer;
                    possibleConnectedPlayer.PlayerEndpoint = remoteEndpoint;
                }
            }
        }
    }
}