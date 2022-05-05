using AgarioServer;

public class ServerMain
{
    public static void Main()
    {
        // new ConnectionListener().WaitForLoginAttempts();
        new GameServer().StartServer();
    }
}

public class GameServer
{
    public List<ConnectedPlayer> Players = new();
    private const int MaxUpdateTime = 1000 / 60;


    public void StartServer()
    {
        new Thread(ServerLoop).Start();
        new Thread(() =>
        {
            new ConnectionListener().WaitForLoginAttempts(this);
        }).Start();
    }

    
    /// <summary>
    /// Main server loop. Handles game updates and other events.
    /// </summary>
    private async void ServerLoop()
    {
        while (true)
        {
            var Timeout = UpdateTimeout();
            
            SendUpdatedPlayerPositionsAndSizes();
            
            
            if (Timeout.IsCompleted)
                Console.WriteLine($"Warning! Update took longer than {MaxUpdateTime}ms! Is the server overloaded?");
            await Timeout;
        }
    }

    
    
    /// <summary>
    /// Sends all current player positions, sizes, and id's to all the clients.
    /// </summary>
    private void SendUpdatedPlayerPositionsAndSizes()
    {
        PlayerData[] data = new PlayerData[Players.Count];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = Players[i].PlayerData;
        }

        NetworkPackage<PlayerData[]> networkPackage =
            new NetworkPackage<PlayerData[]>((int) NetworkProtocol.RequestType.PlayerData, data);

        SendTcpPackageToAllClients(networkPackage);
    }




    private async Task UpdateTimeout()
    {
        await Task.Delay(MaxUpdateTime);
    }

    private void SendTcpPackageToAllClients(NetworkPackage networkPackage)
    {
        foreach (var connectedPlayer in Players)
        {
            connectedPlayer.SendTcpPackage(networkPackage);
        }
    }
}