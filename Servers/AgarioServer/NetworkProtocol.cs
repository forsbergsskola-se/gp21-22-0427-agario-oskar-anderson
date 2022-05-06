namespace AgarioServer;

public class NetworkProtocol
{
    public const int Port = 25565;
}

public enum PackageType
{
    Login = 0,
    Disconnect = 1,
    UserData = 2,
    PlayerData = 3,
    Spawn = 4,
}