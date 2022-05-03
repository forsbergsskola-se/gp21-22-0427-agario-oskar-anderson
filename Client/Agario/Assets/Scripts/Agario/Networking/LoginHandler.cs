using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Agario.Networking
{
    public class LoginHandler : MonoBehaviour
    {
        private TcpClient tcpClient;


        public string ReturnMessage;
        
        public void ConnectToServer(string ipAddress, int port, string userName, Color color)
        {
            tcpClient = new TcpClient(ipAddress, port);

            var stream = tcpClient.GetStream();

            
            var nameBytes = Encoding.UTF8.GetBytes(userName);
            var bytesToSend = new byte[nameBytes.Length + 1];

            nameBytes.CopyTo(bytesToSend, 1);
            
            // stream.Write(new byte[]{0});
            // stream.Write(Encoding.UTF8.GetBytes(userName));
            stream.Write(bytesToSend);
            
            // TODO: We need to have this for the copyto method on the server to work. How do we fix this?
            // stream.Close();
            
            stream = tcpClient.GetStream();
            
            MemoryStream memoryStream = new();
            stream.CopyTo(memoryStream);
            byte[] bytes = memoryStream.ToArray();

            ReturnMessage = Encoding.UTF8.GetString(bytes);
        }

        public void TestConnectionTEMP()
        {
            new Thread(() => ConnectToServer("192.168.1.8", 25565, "Oskar", Color.black)).Start();
        }

        private void Start()
        {
            TestConnectionTEMP();
        }
    }
}
