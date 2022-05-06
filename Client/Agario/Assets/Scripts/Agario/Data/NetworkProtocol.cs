using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agario.Data
{
    public class NetworkProtocol
    {

    }

    public enum PackageType
    {
        Login = 0,
        Disconnect = 1,
        UserData = 2,
        PlayerData = 3,
        Show = 4,
        Hide = 5,
        NewUsers = 6,
    }
}