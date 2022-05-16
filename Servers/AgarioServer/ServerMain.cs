using System.Numerics;
using AgarioServer;
using AgarioServer.Data;
using AgarioServer.Networking;

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
    public List<ConnectedPlayer> PendingConnections = new();
    
    public List<ConnectedPlayer> Players = new();
    private const int MaxUpdateTime = 1000 / 60;

    private UdpBeacon udpBeacon;
    public FoodControl foodControl = new(500);
    private InactivityChecker inactivityChecker;
    

    public void StartServer()
    { 
        udpBeacon = new UdpBeacon(this);
        
        // TODO: Disabled because of strange problems and the code being low priority.
        // inactivityChecker = new InactivityChecker(this);
        
        // Main server loop.
        new Thread(ServerLoop).Start();
        
        // Udp listener handling accepting common data from players.
        new Thread(() =>
        {
            udpBeacon.ListenForPackages();
        }).Start();
        
        // Tcp listener handling new connections from possible players.
        new Thread(() =>
        {
            new ConnectionListener().WaitForLoginAttempts(this, udpBeacon);
        }).Start();
    }



    public void AddPlayerToGameLoop(ConnectedPlayer player)
    {
        PendingConnections.Remove(player);
        lock (Players)
        {
            Players.Add(player);
        }

        var newPlayerPackage = new NetworkPackage<UserData[]>(PackageType.NewUsers, new[] {player.UserData});
        
        SendTcpPackageToAllClients(newPlayerPackage);
    }
    
    

    public void RemovePlayerFromGameLoop(ConnectedPlayer player)
    {
        lock (Players)
        {
            Players.Remove(player);
        }

        player.TcpConnection.StopListener();
        udpBeacon.clientEndpoints.Remove(player.UdpConnection.PlayerEndpoint);

        var playerDisconnectPackage = new NetworkPackage<UserData>(PackageType.UserDisconnect, player.UserData);

        SendTcpPackageToAllClients(playerDisconnectPackage);
    }
    


    /// <summary>
    /// Main server loop. Handles game updates and other events.
    /// </summary>
    private async void ServerLoop()
    {
        while (true)
        {
            var Timeout = UpdateTimeout();
            
            // Update body.
            lock (Players)
            {
                SendUpdatedPlayerPositionsAndSizes();
                
                GenerateAndSendNewFoodPositions();
            }
            
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
        if (Players.Count == 0)
            return;

        PlayerData[] data = new PlayerData[Players.Count];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = Players[i].PlayerData;
        }

        NetworkPackage<PlayerData[]> playersData =
            new NetworkPackage<PlayerData[]>(PackageType.PlayerData, data);

        SendUdpPackageToAllClients(playersData);
    }

    
    
    /// <summary>
    /// Tries to spawn new food and send them to the current players.
    /// </summary>
    private void GenerateAndSendNewFoodPositions()
    {
        if (foodControl.TrySpawnFood(out var foodPackage, 1))
        {
            SendUdpPackageToAllClients(foodPackage);
        }
    }

    



    private async Task UpdateTimeout()
    {
        await Task.Delay(MaxUpdateTime);
    }

    private void SendTcpPackageToAllClients<T>(NetworkPackage<T> networkPackage)
    {
        foreach (var connectedPlayer in Players)
        {
            connectedPlayer.TcpConnection.SendTcpPackage(networkPackage);
        }
    }

    private void SendUdpPackageToAllClients<T>(NetworkPackage<T> networkPackage)
    {
        foreach (var connectedPlayer in Players)
        {
            connectedPlayer.UdpConnection.SendUdpPackage(networkPackage);
        }
    }
}