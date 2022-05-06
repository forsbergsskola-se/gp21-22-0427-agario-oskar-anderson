using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkProtocol
{
    public enum RequestType
    {
        Login = 0,
        Disconnect = 1,
        UserData = 2,
        PlayerData = 3,
    }
}
