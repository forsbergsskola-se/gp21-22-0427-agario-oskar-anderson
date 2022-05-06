using System.Net.Sockets;
using System.Threading;
using UnityEngine;

using Agario.Data;



namespace Agario.Networking
{
    public class LoginHandler : MonoBehaviour
    {
        public string ReturnMessage;
        public string ReturnMessage2;
        public string output;


        [SerializeField] private TcpConnection tcpConnection;
        [SerializeField] private UdpConnection udpConnection;
        [SerializeField] private PlayerInformation playerInformation;
        [SerializeField] private int loginTimeOutInMS;

        public EventWaitHandle ConnectionComplete = new(false, EventResetMode.ManualReset);



        public bool ConnectToServer(string ipAddress, int port, string userName, Color color)
        {
            try
            {
                tcpConnection.SetupTcpConnection(ipAddress, port);
            }
            catch (SocketException e)
            {
                output = "Could not reach server!";
                // throw;
                return false;
            }

            var loginPackage = new NetworkPackage<UserLoginPackage>(0, new UserLoginPackage(color, userName));
            tcpConnection.SendPackage(loginPackage);

            if (ConnectionComplete.WaitOne(loginTimeOutInMS))
            {
                // Todo: Temporary text output.
                output =
                    $"Welcome to the server {playerInformation.UserData.UserName}{playerInformation.UserData.id} with color {playerInformation.UserData.UserColor}";

                udpConnection.SetupUdpConnection(ipAddress, port);
                udpConnection.SendPackage(new NetworkPackage<UserData>((int)NetworkProtocol.RequestType.UserData, playerInformation.UserData));

                return true;
            }

            output = "Server did not respond in time!";
            return false;
        }



        public void CompleteLoginSequence(NetworkPackage<UserData> userDataPackage)
        {
            playerInformation.UserData = userDataPackage.Value;
            ConnectionComplete.Set();
        }


        
        
        public void TestConnectionTEMP()
        {
            if (!ConnectToServer("192.168.1.248", 25565, "Oskar", new Color(23f, 21f, 100f)))
            {
                // This runs if the code failed. We want to retry the connection here, or possible ask the user for a 
                // new ip and port.
            }
        }

        private void Start()
        {
            new Thread(TestConnectionTEMP);
        }
    }
}
