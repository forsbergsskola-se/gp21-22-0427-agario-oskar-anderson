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
        Empty = 0,
        Disconnect = 1,
        UserData = 2,
        PlayerData = 3,
        Show = 4,
        Hide = 5,
        NewUsers = 6,
        UserDisconnect = 7,
        FoodSpawning = 8,
        EatenFood = 9,
        Login = 10,
        EatenPlayer = 11,
    }
}