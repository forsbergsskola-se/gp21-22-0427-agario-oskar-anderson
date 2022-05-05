namespace AgarioServer;

public class NetworkPackage
{
    public int Id;

    public NetworkPackage(int id)
    {
        Id = id;
    }
}

public class NetworkPackage<T> : NetworkPackage
{
    public T Value;

    public NetworkPackage(int Id, T value) : base(Id)
    {
        Value = value;
    }
}