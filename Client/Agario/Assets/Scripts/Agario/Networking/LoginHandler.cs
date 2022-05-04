using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Agario.Networking
{
    public class LoginHandler : MonoBehaviour
    {
        private TcpClient tcpClient;


        public string ReturnMessage;
        public string ReturnMessage2;
        public string output;
        
        public void ConnectToServer(string ipAddress, int port, string userName, System.Drawing.Color color)
        {
            tcpClient = new TcpClient(ipAddress, port);

            var stream = tcpClient.GetStream();



           
            var streamWriter = new StreamWriter(stream);
            // var options = new DataContractJsonSerializerSettings(){InludeFields = true};

            var networkPackage = new NetworkPackage<User>(0, new User(color, userName));

            networkPackage.Value = new User(color, userName);
            
            string JsonMessage = JsonUtility.ToJson(networkPackage);
            ReturnMessage = JsonMessage;
            streamWriter.WriteLine(JsonMessage);
            streamWriter.Flush();

            var streamReader = new StreamReader(stream);
            string returnJsonMessage = streamReader.ReadLine();

            var returnedPackage = JsonUtility.FromJson<NetworkPackage>(returnJsonMessage);

            ReturnMessage2 = returnJsonMessage;
            
            if (returnedPackage.Id == 2)
            {
                var userData = JsonUtility.FromJson<NetworkPackage<UserData>>(returnJsonMessage).Value;
                output =
                    $"Welcome to the server {userData.UserName}{userData.id}";
            }
            else
            {
                output = "Returned package had incorrect id. " + returnedPackage.Id;
            }
        }

        public void TestConnectionTEMP()
        {
            new Thread(() => ConnectToServer("192.168.1.8", 25565, "Oskar", System.Drawing.Color.Blue)).Start();
        }

        private void Start()
        {
            TestConnectionTEMP();
        }
    }
    
    
    public class UserData
    {
        // TODO: Unitys stupid serializer doesn't work well with things that are totally fine for the C# one. 
        public string UserName { get; private set; }
        public System.Drawing.Color UserColor { get; private set; }
        public readonly sbyte id;
        private static sbyte nextId = 0;
    
    
    
        public UserData(string userName, System.Drawing.Color userColor)
        {
            id = nextId;
            nextId++;
            UserName = userName;
            UserColor = userColor;
        }
    }

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

    [Serializable]
    public class User
    {
        public string UserName;
        public System.Drawing.Color UserColor;

        public User(System.Drawing.Color color, string userName = "Player")
        {
            UserName = userName;
            UserColor = color;
        }
    }
}
