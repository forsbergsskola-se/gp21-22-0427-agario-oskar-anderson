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
        private const byte WhiteSpaceByte = 32;
        [SerializeField] private string ipAddress;

        private UdpClient udpClient;
        private IPEndPoint endPoint;

        public delegate string GetUserInput();
        public GetUserInput OnGetUserInput;

        public delegate void UpdateText(string s);
        public UpdateText OnUpdateText;

        public delegate void ErrorText(string s);
        public ErrorText OnError;

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
            ReceiveWord();
        }

        private void SendWord()
        {
            // Get bytes.
            var bytes = Encoding.ASCII.GetBytes(OnGetUserInput.Invoke());

            udpClient.Send(bytes, bytes.Length, endPoint);
        }

        private void ReceiveWord()
        {
            var receivedBytes = udpClient.Receive(ref endPoint);

            if (ErrorCheck(receivedBytes))
            {
                return;
            }
            OnUpdateText.Invoke(Encoding.ASCII.GetString(receivedBytes));
        }

        /// <summary>
        /// Returns true if a error was found and handled, or false if nothing was found.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private bool ErrorCheck(byte[] bytes)
        {
            if (bytes[0] == WhiteSpaceByte)
            {
                // There was an error!
                // TODO: Find out which error, and display a extra warning in that case.
                
                OnError.Invoke(Encoding.ASCII.GetString(bytes));
                
                return true;
            }

            return false;
        }
    }
}