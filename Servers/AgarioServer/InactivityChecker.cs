namespace AgarioServer;

public class InactivityChecker
{
    private GameServer gameServer;
    private Dictionary<ConnectedPlayer, PlayerData> oldData = new();
    private Queue<Action> inactivityKickQueue = new();

    // Creates the object and starts checking the player list for inactive players, kicking any that it finds.
    public InactivityChecker(GameServer gameServer)
    {
        this.gameServer = gameServer;
        new Thread(LoopInactivityCheck).Start();
    }

    private void LoopInactivityCheck()
    {
        while (true)
        {
            lock (gameServer.Players)
            {
                CheckForInactivity(gameServer.Players);
            }

            KickInactivePlayers();

            Task.Delay(1000);
        }
    }

    private void CheckForInactivity(List<ConnectedPlayer> players)
    {
        foreach (var connectedPlayer in players)
        {
            var playerData = new PlayerData();
            if (oldData.TryGetValue(connectedPlayer, out playerData))
            {
                if (connectedPlayer.PlayerData.SamePositionAs(playerData))
                {
                    inactivityKickQueue.Enqueue(connectedPlayer.Disconnect);
                }
            }
            else
            {
                oldData.Add(connectedPlayer, connectedPlayer.PlayerData);
            }

            oldData[connectedPlayer] = connectedPlayer.PlayerData;
        }
    }

    private void KickInactivePlayers()
    {
        for (int i = 0; i < inactivityKickQueue.Count; i++)
        {
            inactivityKickQueue.Dequeue().Invoke();
        }
    }
}