using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

using Agario.Data;


namespace Agario.Networking
{
    public class LoginHandler : MonoBehaviour
    {
        private TcpClient tcpClient;


        public string ReturnMessage;
        public string ReturnMessage2;
        public string output;
        
        public void ConnectToServer(string ipAddress, int port, string userName, Color color)
        {
            tcpClient = new TcpClient(ipAddress, port);

            var stream = tcpClient.GetStream();



           
            var streamWriter = new StreamWriter(stream);

            var networkPackage = new NetworkPackage<UserLoginPackage>(0, new UserLoginPackage(color, userName));

            networkPackage.Value = new UserLoginPackage(color, userName);
            
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
                var package = JsonUtility.FromJson<NetworkPackage<UserData>>(returnJsonMessage);
                var userData = package.Value;
                output =
                    $"Welcome to the server {userData.UserName}{userData.id} with color {userData.UserColor}";
                
                var udpClient = new UdpClient(port);
                var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);


                var bytes = Encoding.UTF8.GetBytes(returnJsonMessage);
                udpClient.Send(bytes, bytes.Length, endPoint);
            }
            else
            {
                output = "Returned package had incorrect id. " + returnedPackage.Id;
            }
        }

        
        
        
        
        
        public void TestConnectionTEMP()
        {
            new Thread(() => ConnectToServer("192.168.1.248", 25565, "Oskar", new Color(23f, 21f, 100f))).Start();
        }

        private void Start()
        {
            TestConnectionTEMP();
        }
    }
}
