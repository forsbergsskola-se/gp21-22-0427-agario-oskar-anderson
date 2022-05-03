using System.Net.Sockets;
using System.Text;

namespace AgarioServer;

public class ConnectedPlayer
{
    public string UserName { get; private set; }
    public readonly sbyte id;

    private static sbyte nextId = 0;

    private TcpClient connectionClient;

    public ConnectedPlayer(TcpClient tcpClient)
    {
        id = nextId;
        nextId++;
        
        EstablishConnection(tcpClient);
    }

    private void EstablishConnection(TcpClient tcpClient)
    {
        var stream = tcpClient.GetStream();

        MemoryStream memoryStream = new();
        stream.CopyTo(memoryStream);
        byte[] bytes = memoryStream.ToArray();

        // Check that the package is a login attempt.
        if (bytes[0] == (int)NetworkProtocol.RequestType.Login)
        {
            UserName = Encoding.UTF8.GetString(bytes, 1, bytes.Length - 1);
            stream.Write(Encoding.UTF8.GetBytes($"Welcome to the server {UserName} with id {id}"));
        }
        
        stream.Dispose();
        stream.Close();
        tcpClient.Close();
    }
}