using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace AgarioServer;

public class ConnectedPlayer
{
    public UserData UserData { get; private set; }

    private TcpClient connectionClient;

    public ConnectedPlayer(TcpClient tcpClient)
    {
        EstablishConnection(tcpClient);
    }

    private void EstablishConnection(TcpClient tcpClient)
    {
        Console.WriteLine("Attempting to establish connection...");
        var streamReader = new StreamReader(tcpClient.GetStream());
        var streamWriter = new StreamWriter(tcpClient.GetStream());
        var options = new JsonSerializerOptions{IncludeFields = true};

        string? jsonMessage = streamReader.ReadLine();

        var networkPackage = JsonSerializer.Deserialize<NetworkPackage>(jsonMessage, options);
        if (networkPackage.Id == (int) NetworkProtocol.RequestType.Login)
        {
            Console.WriteLine("Connection was successful...");
            UserData = JsonSerializer.Deserialize<NetworkPackage<UserData>>(jsonMessage, options).Value;

            var returnPackage = new NetworkPackage<UserData>((int)NetworkProtocol.RequestType.UserData ,UserData);
            streamWriter.WriteLine(JsonSerializer.Serialize(returnPackage, options));
            streamWriter.Flush();
            Console.WriteLine($"Welcome to the server {UserData.UserName}{UserData.id} with color {UserData.UserColor}");
        }
        else
        {
            Console.WriteLine("Connection refused due to incorrect package id...");
        }

        connectionClient = tcpClient;
    }
}