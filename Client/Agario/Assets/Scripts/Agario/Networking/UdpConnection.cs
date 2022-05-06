using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Agario.Data;
using UnityEngine;

namespace Agario.Networking
{
    public class UdpConnection : MonoBehaviour
    {
        private UdpClient udpClient;
        private IPEndPoint serverEndpoint;

        public void SetupUdpConnection(string ipAddress, int port)
        {
            udpClient = new UdpClient(port);
            serverEndpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);

            new Thread(UdpListener).Start();


            // var bytes = Encoding.UTF8.GetBytes(returnJsonMessage);
            // udpClient.Send(bytes, bytes.Length, endPoint);
        }
        
        public void UdpListener()
        {
            while (true)
            {
                var receivedJson = Encoding.UTF8.GetString(udpClient.Receive(ref serverEndpoint));
                var basePackage = JsonUtility.FromJson<NetworkPackage>(receivedJson);

                switch (basePackage.Id)
                {
                }
            }
        }
        
        /// <summary>
        /// Sends the provided networkPackage to the connected server.
        /// </summary>
        /// <param name="networkPackage"></param>
        public void SendPackage(NetworkPackage networkPackage)
        {
            string jsonMessage = JsonUtility.ToJson(networkPackage);
            var bytes = Encoding.UTF8.GetBytes(jsonMessage);
            udpClient.Send(bytes, bytes.Length, serverEndpoint);
        }
    }
}