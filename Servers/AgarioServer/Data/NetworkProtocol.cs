namespace AgarioServer.Data;

public class NetworkProtocol
{
    // public const int Port = 25565;
}

public enum PackageType
{
    Empty = 0,
    Login = 10,
    Disconnect = 1,
    UserData = 2,
    PlayerData = 3,
    Show = 4,
    Hide = 5,
    NewUsers = 6,
    UserDisconnect = 7,
    FoodSpawning = 8,
    EatenFood = 9,
}