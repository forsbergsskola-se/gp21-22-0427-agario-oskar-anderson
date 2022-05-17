namespace TNC.DataStructures
{
    public class NetPackage
    {
        public string ObjType;

        public NetPackage()
        {
            ObjType = GetType().FullName;
        }
    }
}