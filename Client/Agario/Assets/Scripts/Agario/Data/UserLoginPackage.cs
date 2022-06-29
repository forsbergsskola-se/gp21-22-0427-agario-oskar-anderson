using System;
using UnityEngine;


namespace Agario.Data
{
    [Serializable]
    public class UserLoginPackage
    {
        public string UserName;
        public Color UserColor;

        public UserLoginPackage(Color color, string userName = "Player")
        {
            UserName = userName;
            UserColor = color;
        }
    }
}