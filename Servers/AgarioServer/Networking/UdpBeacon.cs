using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Text.Json;
using AgarioServer.Data;

namespace AgarioServer.Networking;

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
            var receivedJson = Encoding.UTF8.GetString(UdpClient.Receive(ref remoteEndpoint));
            Console.WriteLine("Udp: received package...");
            Console.WriteLine("Udp: " + receivedJson);

            if (receivedJson == null)
                return;
            
            var basePackage = JsonSerializer.Deserialize<NetworkPackage>(receivedJson, serializeAllFields);
            
            // If the package for some reason is wrong or empty then abort this iteration.
            if (basePackage.Id == PackageType.Empty)
                continue;

            if (clientEndpoints.ContainsKey(remoteEndpoint))
            {
                Console.WriteLine("Udp: endpoint was known, passing data to connected player...");
                // If package sender is known: Set the correct playerdata to the package information.
                // TODO: We want to add a package id check to make sure the received package actually is a PlayerData one.
            
                // TODO: This will eventually be replaced to handle a different data type for both food eaten and PlayerData

                
                switch (basePackage.Id)
                {
                    case PackageType.PlayerData:
                        var playerData = JsonSerializer.Deserialize<NetworkPackage<PlayerData>>(receivedJson, serializeAllFields).Value;
                        clientEndpoints[remoteEndpoint].PlayerData = playerData;
                        break;
                    case PackageType.EatenFood:
                        var eatenFood = JsonSerializer.Deserialize<NetworkPackage<Vector2[]>>(receivedJson, serializeAllFields).Value;
                        gameServer.foodControl.RemoveFood(eatenFood);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Udp: endpoint was not known, attempting to add...");
                // var package = JsonSerializer.Deserialize<NetworkPackage>(receivedJson, serializeAllFields);

                if (basePackage.Id == PackageType.UserData)
                {
                    var userData = JsonSerializer.Deserialize<NetworkPackage<UserData>>(receivedJson, serializeAllFields).Value;

                    var possibleConnectedPlayer = gameServer.PendingConnections.First(p => p.UserData.id == userData.id);
                    if (possibleConnectedPlayer != null)
                    {
                        Console.WriteLine("Udp: successfully added new player...");
                        clientEndpoints[remoteEndpoint] = possibleConnectedPlayer;
                        possibleConnectedPlayer.UdpConnection.PlayerEndpoint = remoteEndpoint;
                        possibleConnectedPlayer.UdpConnection.UdpConnectionComplete.Set();
                    }
                }
                else
                {
                    Console.WriteLine("Package had wrong id! Who is sending these packages?");
                }
            }
        }
    }

    
    /// <summary>
    /// Stops the udp listener. WARNING! This affects the whole server!
    /// </summary>
    public void StopListener()
    {
        UdpClient.Close();
        UdpClient = null;
    }
}