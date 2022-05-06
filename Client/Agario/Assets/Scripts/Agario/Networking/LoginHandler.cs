using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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


        [SerializeField] private TcpConnection tcpConnection;
        [SerializeField] private UdpConnection udpConnection;
        [SerializeField] private PlayerInformation playerInformation;
        [SerializeField] private int loginTimeOutInMS;

        public EventWaitHandle ConnectionComplete = new EventWaitHandle(false, EventResetMode.ManualReset);
        
        
        public void ConnectToServer(string ipAddress, int port, string userName, Color color)
        {
            // tcpClient = new TcpClient(ipAddress, port);
            //
            // var stream = tcpClient.GetStream();
            // var streamWriter = new StreamWriter(stream);

            tcpConnection.SetupTcpConnection(ipAddress, port);
            

            var loginPackage = new NetworkPackage<UserLoginPackage>(0, new UserLoginPackage(color, userName));
            tcpConnection.SendPackage(loginPackage);

            if (ConnectionComplete.WaitOne(loginTimeOutInMS))
            {
                // Login was successful!
                
                // Todo: Temporary text output.
                output =
                    $"Welcome to the server {playerInformation.UserData.UserName}{playerInformation.UserData.id} with color {playerInformation.UserData.UserColor}";

                udpConnection.SetupUdpConnection(ipAddress, port);
                udpConnection.SendPackage(new NetworkPackage<UserData>((int)NetworkProtocol.RequestType.UserData, playerInformation.UserData));
            }
            else
            {
                throw new Exception("Login attempt failed!");
            }



            // var returnedPackage = tcpConnection.ReceivePackage<>();



            // string JsonMessage = JsonUtility.ToJson(loginPackage);
            // ReturnMessage = JsonMessage;
            // streamWriter.WriteLine(JsonMessage);
            // streamWriter.Flush();
            //
            // var streamReader = new StreamReader(stream);
            // string returnJsonMessage = streamReader.ReadLine();

            // var returnedPackage = JsonUtility.FromJson<NetworkPackage>(returnJsonMessage);

            // ReturnMessage2 = returnJsonMessage;
            
            // if (returnedPackage.Id == 2)
            // {
            //     var package = JsonUtility.FromJson<NetworkPackage<UserData>>(returnJsonMessage);
            //     var userData = package.Value;
            //
            //     
            //     
            //     
            //     
            //     var udpClient = new UdpClient(port);
            //     var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            //
            //
            //     var bytes = Encoding.UTF8.GetBytes(returnJsonMessage);
            //     udpClient.Send(bytes, bytes.Length, endPoint);
            // }
            // else
            // {
            //     output = "Returned package had incorrect id. " + returnedPackage.Id;
            // }
        }



        public void CompleteLoginSequence(NetworkPackage<UserData> userDataPackage)
        {
            playerInformation.UserData = userDataPackage.Value;
            ConnectionComplete.Set();
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
