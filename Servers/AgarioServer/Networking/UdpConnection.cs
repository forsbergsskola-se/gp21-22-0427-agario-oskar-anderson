using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using AgarioServer.Data;

namespace AgarioServer.Networking;

public class UdpConnection
{
    private UdpBeacon udpBeacon;
    
    private UdpClient udpClient;
    
    public IPEndPoint PlayerEndpoint;
    
    private readonly JsonSerializerOptions serializeAllFields = new() {IncludeFields = true};

    public readonly EventWaitHandle UdpConnectionComplete = new(false, EventResetMode.ManualReset);

    public UdpConnection(UdpBeacon udpBeacon)
    {
        this.udpBeacon = udpBeacon;
    }
    
    public void SendUdpPackage<T>(NetworkPackage<T> networkPackage)
    {
        var json = JsonSerializer.Serialize(networkPackage, serializeAllFields);
        // Console.WriteLine("Udp send: " + json);
        var bytes = Encoding.UTF8.GetBytes(json);
        udpBeacon.UdpClient.Send(bytes, bytes.Length, PlayerEndpoint);
    }
}