using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

using Agario.Data;



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