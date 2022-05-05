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
            var user = JsonSerializer.Deserialize<NetworkPackage<User>>(jsonMessage, options).Value;
            UserData = new UserData(user.UserName, user.UserColor);

            var returnPackage = new NetworkPackage<UserData>((int)NetworkProtocol.RequestType.UserData ,UserData);
            streamWriter.WriteLine(JsonSerializer.Serialize(returnPackage, options));
            streamWriter.Flush();
            Console.WriteLine($"Welcome to the server {UserData.UserName}{UserData.id} with color {UserData.UserColor}");
        }
        else
        {
            Console.WriteLine("Connection refused due to incorrect package id...");
        }
        
        
        
        
        // var stream = tcpClient.GetStream();
        //
        // MemoryStream memoryStream = new();
        // stream.CopyTo(memoryStream);
        // byte[] bytes = memoryStream.ToArray();
        //
        // // Check that the package is a login attempt.
        // if (bytes[0] == (int)NetworkProtocol.RequestType.Login)
        // {
        //     Console.WriteLine("Connection was successful...");
        //     UserName = Encoding.UTF8.GetString(bytes, 1, bytes.Length - 1);
        //     stream.Write(Encoding.UTF8.GetBytes($"Welcome to the server {UserName} with id {id}"));
        //     Console.WriteLine($"Welcome to the server {UserName} with id {id}");
        // }
        // else
        // {
        //     Console.WriteLine("Connection refused due to incorrect package id...");
        // }
        //
        // stream.Dispose();
        // stream.Close();
        tcpClient.Close();
    }
}

public class UserData
{
    public string UserName { get; private set; }
    public Color UserColor { get; private set; }
    public readonly sbyte id;
    private static sbyte nextId = 0;
    
    
    
    public UserData(string userName, Color userColor)
    {
        id = nextId;
        nextId++;
        UserName = userName;
        UserColor = userColor;
    }
}

public struct Color
{
    public float r;
    public float g;
    public float b;
    public float a;

    public Color(float r, float g, float b, float a = 1)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }
}

public class NetworkPackage
{
    public int Id;

    public NetworkPackage(int id)
    {
        Id = id;
    }
}

public class NetworkPackage<T> : NetworkPackage
{
    public T Value;

    public NetworkPackage(int Id, T value) : base(Id)
    {
        Value = value;
    }
}

public class User
{
    public string UserName = "Player";
    public Color UserColor = new Color(1, 0, 0);
}