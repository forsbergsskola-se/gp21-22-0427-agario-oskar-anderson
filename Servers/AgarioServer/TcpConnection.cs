using System.Net.Sockets;
using System.Text.Json;

namespace AgarioServer;

public class TcpConnection
{
    private TcpClient tcpClient;
    private StreamReader streamReader;
    private StreamWriter streamWriter;
    
    private readonly JsonSerializerOptions serializeAllFields = new() {IncludeFields = true};
    
    // I really really do not want this field here but I don't have time for a cleaner solution... :/
    private static sbyte nextId;
    
    
    
    public TcpConnection(TcpClient tcpClient)
    {
        this.tcpClient = tcpClient;
        streamReader = new StreamReader(tcpClient.GetStream());
        streamWriter = new StreamWriter(tcpClient.GetStream());
        
        new Thread(TcpListener).Start();
    }

    private void TcpListener()
    {
        while (true)
        {
            try
            {
                string receivedJson = streamReader.ReadLine();

                var basePackage = JsonSerializer.Deserialize<NetworkPackage>(receivedJson, serializeAllFields);

                switch (basePackage.Id)
                {
                    case PackageType.Login:
                        AcceptLoginPackage(JsonSerializer.Deserialize<NetworkPackage<UserData>>(receivedJson, serializeAllFields));
                        break;
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Tcp: socketException...");
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public void StopListener()
    {
        streamReader.Close();
    }
    
    

    private void AcceptLoginPackage(NetworkPackage<UserData> userdataPackage)
    {
        var userData = userdataPackage.Value;
        Console.WriteLine("Tcp: connection was successful...");
        
        // TODO: I don't want this here.
        userData.id = nextId;
        nextId++;
        
        // Send confirmation package.
        var returnPackage = new NetworkPackage<UserData>(PackageType.UserData, userData);
        SendTcpPackage(returnPackage);
    }
    
    
    
    public void SendTcpPackage(NetworkPackage networkPackage)
    {
        streamWriter.WriteLine(JsonSerializer.Serialize(networkPackage, serializeAllFields));
        streamWriter.Flush();
    }
}