using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace OpenWordClient
{
    public class ServerCommunicator : MonoBehaviour
    {
        private const int Port = 4444;
        [SerializeField] private string ipAddress;

        private UdpClient udpClient;
        private IPEndPoint endPoint;

        public delegate string GetUserInput();
        public GetUserInput OnGetUserInput;

        public delegate void UpdateText(string s);
        public UpdateText OnUpdateText;
        
        public void Start()
        {
            EstablishConnection();
        }

        private void EstablishConnection()
        {
            udpClient = new UdpClient(Port);
            endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), Port);
        }

        public void SendAndReceive()
        {
            SendWord();
        }

        private void SendWord()
        {
            // Get bytes.
            var bytes = Encoding.ASCII.GetBytes(OnGetUserInput.Invoke());

            udpClient.Send(bytes, bytes.Length, endPoint);
        }

        private string ReceiveWord()
        {
            return null;
        }
    }
}