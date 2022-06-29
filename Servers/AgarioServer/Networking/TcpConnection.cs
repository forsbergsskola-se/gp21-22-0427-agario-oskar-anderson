using System.Net.Sockets;
using System.Text.Json;
using AgarioServer.Data;

namespace AgarioServer.Networking;

public class TcpConnection
{
    private ConnectedPlayer connectedPlayer;
    private TcpClient tcpClient;
    private StreamReader? streamReader;
    private StreamWriter streamWriter;
    
    private readonly JsonSerializerOptions serializeAllFields = new() {IncludeFields = true};
    
    // I really really do not want this field here but I don't have time for a cleaner solution... :/
    private static sbyte nextId;
    
    
    
    public TcpConnection(TcpClient tcpClient, ConnectedPlayer connectedPlayer)
    {
        this.tcpClient = tcpClient;
        this.connectedPlayer = connectedPlayer;
        streamReader = new StreamReader(tcpClient.GetStream());
        streamWriter = new StreamWriter(tcpClient.GetStream());
        
        new Thread(TcpListener).Start();
    }

    
    
    private void TcpListener()
    {
        while (streamReader != null)
        {
            try
            {
                string receivedJson = streamReader.ReadLine();

                // If the string is null then it means the streamReader has been closed. So stop this listener.
                if (receivedJson == null)
                    return;

                // Console.WriteLine("Tcp: " + receivedJson);

                var basePackage = JsonSerializer.Deserialize<NetworkPackage>(receivedJson, serializeAllFields);
                
                // If the package for some reason is wrong or empty then abort this iteration.
                if (basePackage.Id == PackageType.Empty)
                    continue;

                switch (basePackage.Id)
                {
                    case PackageType.Login:
                        var temp = JsonSerializer.Deserialize<NetworkPackage<UserData>>(receivedJson,
                            serializeAllFields);
                        AcceptLoginPackage(temp);
                        break;
                    case PackageType.Disconnect:
                        connectedPlayer.Disconnect();
                        return;
                    case PackageType.EatenPlayer:
                        var otherPlayer =
                            JsonSerializer.Deserialize<NetworkPackage<PlayerData>>(receivedJson, serializeAllFields).Value;
                        connectedPlayer.gameServer.playerManager.PlayerEatPlayer(connectedPlayer, otherPlayer);
                        break;
                    case PackageType.Show:
                        connectedPlayer.gameServer.playerManager.PlayerRespawned(connectedPlayer.PlayerData);
                        break;
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Tcp: socketException...");
                Console.WriteLine(e);

                connectedPlayer.Disconnect();

                // throw;
            }
            catch (IOException e)
            {
                Console.WriteLine("User crashed!");
                connectedPlayer.Disconnect();
            }
        }
    }

    
    
    public void StopListener()
    {
        streamReader?.Close();
        streamReader = null;
    }
    
    

    private void AcceptLoginPackage(NetworkPackage<UserData> userdataPackage)
    {
        var userData = userdataPackage.Value;
        Console.WriteLine("Tcp: connection was successful...");
        
        // TODO: I don't want this here.
        userData.id = nextId;
        nextId++;

        connectedPlayer.UserData = userData;
        
        // Send confirmation package.
        var returnPackage = new NetworkPackage<UserData>(PackageType.UserData, userData);
        SendTcpPackage(returnPackage);
    }
    
    
    
    public void SendTcpPackage<T>(NetworkPackage<T> networkPackage)
    {
        string json = JsonSerializer.Serialize(networkPackage, serializeAllFields);
        streamWriter.WriteLine(json);
        // Console.WriteLine("Tcp send: " + JsonSerializer.Serialize(networkPackage, serializeAllFields));
        streamWriter.Flush();
    }
}