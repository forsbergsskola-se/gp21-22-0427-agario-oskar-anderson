using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    private const int Port = 4444;
    private const byte MaximumWordLength = 20;

    private static byte[] WordToLongErrorMessage;
    private static byte[] MultipleWordsReceivedErrorMessage;

    private static async Task CreateByteArraysForErrorMessages()
    {
        WordToLongErrorMessage =
            Encoding.ASCII.GetBytes("Error! Sent word is to long! Please limit your words to a length of " +
                                    MaximumWordLength + " letters!");
        MultipleWordsReceivedErrorMessage =
            Encoding.ASCII.GetBytes("Error! You are only allowed to send one word at a time!");
    }

    static void Main()
    {
        // I know there is no way it it worth it to put loading this small things in a separate thread like this. But, I
        // wanted the training so I did it anyway.
        var InitializationTask = CreateByteArraysForErrorMessages();
        
        byte[] fullMessage = Array.Empty<byte>();

        UdpClient udpClient = new UdpClient(Port);

        IPEndPoint remoteEndPoint = null;
        
        // Again, not something you should probably do in a case like this, but did just for training purposes.
        InitializationTask.Wait();
        
        while (true)
        {
            var receivedBytes = udpClient.Receive(ref remoteEndPoint);
            
            Console.WriteLine($"Received {receivedBytes.Length} bytes.");
            
            // Check if the received message was to long.
            if (receivedBytes.Length > MaximumWordLength)
            {
                udpClient.Send(WordToLongErrorMessage, WordToLongErrorMessage.Length, remoteEndPoint);
                continue;
            }



            Console.WriteLine(Encoding.ASCII.GetString(receivedBytes));
        }
        
        udpClient.Close();
    }
}