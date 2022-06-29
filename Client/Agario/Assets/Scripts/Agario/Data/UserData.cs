using System;
using UnityEngine;


namespace Agario.Data
{
    [Serializable]
    public class UserData
    {
        public string UserName;
        public Color UserColor;
        public int id;



        public UserData(string userName, Color userColor)
        {
            UserName = userName;
            UserColor = userColor;
        }
    }

}