using System;


namespace Agario.Data
{
    [Serializable]
    public class NetworkPackage
    {
        public int Id;
        
        public NetworkPackage(int id)
        {
            Id = id;
        }
    }

    [Serializable]
    public class NetworkPackage<T> : NetworkPackage
    {
        public T Value;

        public NetworkPackage(int Id, T value) : base(Id)
        {
            Value = value;
        }
    }
}