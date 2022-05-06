namespace AgarioServer;


public class NetworkPackage
{
    public PackageType Id;

    public NetworkPackage(PackageType id)
    {
        Id = id;
    }
}


public class NetworkPackage<T> : NetworkPackage
{
    public T Value;

    public NetworkPackage(PackageType Id, T value) : base(Id)
    {
        Value = value;
    }
}