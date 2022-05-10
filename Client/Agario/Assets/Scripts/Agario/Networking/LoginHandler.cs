using System;
using System.Collections;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

using Agario.Data;
using Agario.Entities.Player;


namespace Agario.Networking
{
    public class LoginHandler : MonoBehaviour
    {
        [SerializeField] private TcpConnection tcpConnection;
        [SerializeField] private UdpConnection udpConnection;
        [SerializeField] private PlayerInformation playerInformation;
        [SerializeField] private int loginTimeOutInMS;

        public EventWaitHandle TcpConnectionComplete = new(false, EventResetMode.ManualReset);

        private delegate void PostLoginAttemptCode();

        private PostLoginAttemptCode postLoginAttemptCode;


        private bool ConnectToServer(string ipAddress, int port, string userName, Color color)
        {
            try
            {
                tcpConnection.SetupTcpConnection(ipAddress, port);
            }
            catch (SocketException)
            {
                // output = "Could not reach server!";
                // throw;
                return false;
            }

            var loginPackage = new NetworkPackage<UserLoginPackage>(PackageType.Login, new UserLoginPackage(color, userName));
            tcpConnection.SendPackage(loginPackage);

            if (TcpConnectionComplete.WaitOne(loginTimeOutInMS))
            {
                // Todo: Temporary text output.
                // output =
                //     $"Welcome to the server {playerInformation.UserData.UserName}{playerInformation.UserData.id} with color {playerInformation.UserData.UserColor}";

                udpConnection.SetupUdpConnection(ipAddress, port);
                udpConnection.SendPackage(new NetworkPackage<UserData>(PackageType.UserData, playerInformation.UserData));

                return true;
            }

            // output = "Server did not respond in time!";
            return false;
        }



        public void CompleteTcpLoginSequence(NetworkPackage<UserData> userDataPackage)
        {
            playerInformation.UserData = userDataPackage.Value;
            playerInformation.PlayerData.PlayerId = playerInformation.UserData.id;
            TcpConnectionComplete.Set();
        }


        [SerializeField] private PlayerDataSender playerDataSender;
        [SerializeField] private PlayerMovementInput playerMovementInput;
        
        private void EnableNetworkingObjects()
        {
            playerDataSender.enabled = true;
            playerMovementInput.enabled = true;
        }

        private void ReturnToLoginScreenAfterFailedAttempt()
        {
            throw new NotImplementedException();
        }

        

        private void Start()
        {
            // new Thread(TestConnectionTEMP).Start();
            // StartCoroutine(Login("192.168.1.248", 25565, "Oskar", new Color(23f, 21f, 100f)));
            Login("192.168.1.248", 25565, "Oskar", new Color(23f, 21f, 100f));
        }
        
        
        

        private void Login(string ipAddress, int port, string userName, Color color)
        {
            new Thread(() =>
            {
                if (ConnectToServer(ipAddress, port, userName, color)) {
                    postLoginAttemptCode = EnableNetworkingObjects;
                }
                else {
                    postLoginAttemptCode = ReturnToLoginScreenAfterFailedAttempt;
                }
                
            }).Start();
        }

        private void FixedUpdate()
        {
            if (postLoginAttemptCode != null)
            {
                postLoginAttemptCode();
                postLoginAttemptCode = null;
            }
        }
    }
}
