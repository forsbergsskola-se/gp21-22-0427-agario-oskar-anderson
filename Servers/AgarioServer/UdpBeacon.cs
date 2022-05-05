using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AgarioServer;

public class UdpBeacon
{
    public UdpClient UdpClient = new UdpClient(ServerSettings.Port);

    private Dictionary<IPEndPoint, ConnectedPlayer> clientEndpoints = new();

    public void ListenForPackages()
    {
        IPEndPoint remoteEndpoint = null;
        var Json = Encoding.UTF8.GetString(UdpClient.Receive(ref remoteEndpoint));

        if (clientEndpoints.ContainsKey(remoteEndpoint))
        {
            // If package sender is known: Send package to appropriate ConnectedPlayer
        }
        else
        {
            
            // If not known:
            // Check package for player id
            // If a valid id is found and that player does not have a endpoint 
            // Set this endpoint as that players endpoint
            // If they do
            // Discard this package as it was not sent by the correct user.
            // If not found
            // Discard package for wrong format.
        }
    }
}