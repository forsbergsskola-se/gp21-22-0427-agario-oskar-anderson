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
    public List<ConnectedPlayer> Players;
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
            
            
            
            
            if (Timeout.IsCompleted)
                Console.WriteLine($"Warning! Update took longer than {MaxUpdateTime}ms! Is the server overloaded?");
            await Timeout;
        }
    }




    private async Task UpdateTimeout()
    {
        await Task.Delay(MaxUpdateTime);
    }
}