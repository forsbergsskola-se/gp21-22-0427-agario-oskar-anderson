using AgarioServer.Data;
using AgarioServer.Networking;

namespace AgarioServer;

public class PlayerManager
{
    public List<ConnectedPlayer> Players = new();
    private GameServer gameServer;

    public PlayerManager(GameServer gameServer)
    {
        this.gameServer = gameServer;
    }
    
    
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

    public void PlayerEatPlayer(ConnectedPlayer caller, PlayerData eatenPlayerData)
    {
        // var otherConnectedPlayer = Players.Select(x => x.PlayerData.PlayerId == eatenPlayerData.PlayerId);

        var package = new NetworkPackage<int>(PackageType.Hide, eatenPlayerData.PlayerId);
        gameServer.SendTcpPackageToAllClients(package);
    }

    public void PlayerRespawned(PlayerData playerData)
    {
        var package = new NetworkPackage<int>(PackageType.Show, playerData.PlayerId);
        gameServer.SendTcpPackageToAllClients(package);
    }
}