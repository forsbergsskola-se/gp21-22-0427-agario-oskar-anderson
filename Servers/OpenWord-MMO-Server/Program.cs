using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    private const int Port = 4444;
    private const byte MaximumWordLength = 20;
    private const byte WhiteSpaceByte = 32;

    private static byte[] WordToLongErrorMessage;
    private static byte[] MultipleWordsReceivedErrorMessage;
    private static byte[] NothingReceivedErrorMessage;

    private static async Task CreateByteArraysForErrorMessages()
    {
        WordToLongErrorMessage =
            Encoding.ASCII.GetBytes(" Error! Sent word is to long! Please limit your words to a length of " +
                                    MaximumWordLength + " letters!");
        MultipleWordsReceivedErrorMessage =
            Encoding.ASCII.GetBytes(" Error! You are only allowed to send one word at a time! Spaces are banned!");

        NothingReceivedErrorMessage = Encoding.ASCII.GetBytes(" Error! Nothing received!");
    }

    static void Main()
    {
        Console.WriteLine("Server starting...");
        
        // I know there is no way it it worth it to put loading this small things in a separate thread like this. But, I
        // wanted the training so I did it anyway.
        var InitializationTask = CreateByteArraysForErrorMessages();
        
        byte[] fullMessage = Array.Empty<byte>();

        UdpClient udpClient = new UdpClient(Port);

        IPEndPoint remoteEndPoint = null;
        
        // Again, not something you should probably do in a case like this, but did just for training purposes.
        // TODO: Why did I use .Wait() here and not await? :/
        InitializationTask.Wait();
        
        while (true)
        {
            Console.WriteLine("Waiting for connections...");
            
            var receivedBytes = udpClient.Receive(ref remoteEndPoint);
            
            Console.WriteLine($"Received {receivedBytes.Length} bytes...");
            
            // Check if the received message was to long.
            if (receivedBytes.Length > MaximumWordLength)
            {
                udpClient.Send(WordToLongErrorMessage, WordToLongErrorMessage.Length, remoteEndPoint);
                Console.WriteLine("Denied! Word was to long!");
                continue;
            }

            // Check if it contains whitespaces.
            if (receivedBytes.Contains(WhiteSpaceByte))
            {
                udpClient.Send(MultipleWordsReceivedErrorMessage, MultipleWordsReceivedErrorMessage.Length, remoteEndPoint);
                Console.WriteLine("Denied! Word had spaces!");
                continue;
            }
            
            // Check if empty.
            if (receivedBytes.Length == 0)
            {
                udpClient.Send(NothingReceivedErrorMessage, NothingReceivedErrorMessage.Length, remoteEndPoint);
                Console.WriteLine("Denied! Nothing was received!");
                continue;
            }

            Console.WriteLine("Accepted!");

            MergeArraysWithWhiteSpace(ref fullMessage, receivedBytes);
            
            udpClient.Send(fullMessage, fullMessage.Length, remoteEndPoint);

            Console.WriteLine(Encoding.ASCII.GetString(fullMessage));
        }

        udpClient.Close();
    }

    private static void MergeArraysWithWhiteSpace(ref byte[] fullMessage, byte[] receivedBytes)
    {
        int index = fullMessage.Length;
        Array.Resize(ref fullMessage, fullMessage.Length + receivedBytes.Length + 1);
        foreach (var letterByte in receivedBytes)
        {
            fullMessage[index] = letterByte;
            index++;
        }

        fullMessage[index] = WhiteSpaceByte;
    }
}