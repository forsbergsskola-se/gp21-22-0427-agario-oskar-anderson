using AgarioServer;

public class ServerMain
{
    public static void Main()
    {
        new ConnectionListener().WaitForLoginAttempts();
    }
}