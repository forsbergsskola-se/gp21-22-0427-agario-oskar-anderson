namespace AgarioServer;

public class NetworkProtocol
{
    public const int Port = 25565;
    
    public enum RequestType
    {
        Login = 0,
        Disconnect = 1,
    }
}