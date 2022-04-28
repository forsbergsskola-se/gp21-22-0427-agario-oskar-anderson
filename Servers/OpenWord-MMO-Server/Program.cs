using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    private const int port = 4444;
    
    static void Main()
    {
        byte[] fullMessage = Array.Empty<byte>();

        UdpClient udpClient = new UdpClient(port);

        IPEndPoint remoteEndPoint = null;


        
   
        
        while (true)
        {
            var receivedBytes = udpClient.Receive(ref remoteEndPoint);

            Console.WriteLine(Encoding.ASCII.GetString(receivedBytes));
        }
        
        udpClient.Close();
    }
}