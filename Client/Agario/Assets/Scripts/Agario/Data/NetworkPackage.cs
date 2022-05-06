using System;


namespace Agario.Data
{
    [Serializable]
    public class NetworkPackage
    {
        public PackageType Id;
        
        public NetworkPackage(PackageType id)
        {
            Id = id;
        }
    }

    [Serializable]
    public class NetworkPackage<T> : NetworkPackage
    {
        public T Value;

        public NetworkPackage(PackageType Id, T value) : base(Id)
        {
            Value = value;
        }
    }
}