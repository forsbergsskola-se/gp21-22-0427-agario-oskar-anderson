using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace AgarioServer;

public class UdpConnection
{
    private UdpBeacon udpBeacon;
    
    private UdpClient udpClient;
    
    public IPEndPoint PlayerEndpoint;
    
    private readonly JsonSerializerOptions serializeAllFields = new() {IncludeFields = true};

    public UdpConnection(UdpBeacon udpBeacon)
    {
        this.udpBeacon = udpBeacon;
    }
    
    public void SendUdpPackage(NetworkPackage networkPackage)
    {
        var json = JsonSerializer.Serialize(networkPackage, serializeAllFields);
        var bytes = Encoding.UTF8.GetBytes(json);
        udpBeacon.UdpClient.Send(bytes, bytes.Length, PlayerEndpoint);
    }
}