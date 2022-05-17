using AgarioServer.Networking;

namespace AgarioServer;

public class PlayerManager
{
    public List<ConnectedPlayer> Players = new();


    public void AddPlayer(ConnectedPlayer player)
    {
        lock (Players)
        {
            Players.Add(player);
        }
    }

    public void RemovePlayer(ConnectedPlayer player)
    {
        lock (Players)
        {
            Players.Remove(player);
        }
    }
}