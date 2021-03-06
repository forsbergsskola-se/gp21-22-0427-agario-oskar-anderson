using System;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

using Agario.Data;
using Agario.Entities.Food;
using Agario.Entities.Player;

using Random = UnityEngine.Random;


namespace Agario.Networking
{
    public class LoginHandler : MonoBehaviour
    {
        [SerializeField] private TcpConnection tcpConnection;
        [SerializeField] private UdpConnection udpConnection;
        [SerializeField] private PlayerInformation playerInformation;
        [SerializeField] private int loginTimeOutInMS;
        [SerializeField] private MainThreadQueue mainThreadQueue;
        [SerializeField] private LaunchData launchData;

        public EventWaitHandle TcpConnectionComplete = new(false, EventResetMode.ManualReset);

        private delegate void PostLoginAttemptCode();

        private PostLoginAttemptCode postLoginAttemptCode;


        private bool ConnectToServer()
        {
            try
            {
                tcpConnection.SetupTcpConnection(launchData.IpAddress, launchData.Port);
            }
            catch (SocketException)
            {
                // output = "Could not reach server!";
                // throw;
                return false;
            }

            var loginPackage = new NetworkPackage<UserLoginPackage>(PackageType.Login, new UserLoginPackage(launchData.UserColor, launchData.UserName));
            tcpConnection.SendPackage(loginPackage);

            if (TcpConnectionComplete.WaitOne(loginTimeOutInMS))
            {
                // Todo: Temporary text output.
                // output =
                //     $"Welcome to the server {playerInformation.UserData.UserName}{playerInformation.UserData.id} with color {playerInformation.UserData.UserColor}";

                udpConnection.SetupUdpConnection(launchData.IpAddress, launchData.Port);
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
            mainThreadQueue.ActionQueue.Enqueue(() =>
            {
                playerInformation.gameObject.transform.position = new Vector3(Random.Range(-49, 49), Random.Range(-49, 49), 0);
            });

            mainThreadQueue.ActionQueue.Enqueue(() => playerInformation.ApplyColor()); 
            
            TcpConnectionComplete.Set();
        }


        [SerializeField] private PlayerDataSender playerDataSender;
        [SerializeField] private PlayerMovementInput playerMovementInput;
        [SerializeField] private EatenFoodSender eatenFoodSender;
        
        private void EnableNetworkingObjects()
        {
            playerDataSender.enabled = true;
            eatenFoodSender.enabled = true;
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
            Login();
        }
        
        
        

        private void Login()
        {
            new Thread(() =>
            {
                if (ConnectToServer()) {
                    postLoginAttemptCode = EnableNetworkingObjects;
                }
                else {
                    postLoginAttemptCode = ReturnToLoginScreenAfterFailedAttempt;
                }

                mainThreadQueue.ActionQueue.Enqueue(() => postLoginAttemptCode());

            }).Start();
        }

        // private void FixedUpdate()
        // {
        //     if (postLoginAttemptCode != null)
        //     {
        //         postLoginAttemptCode();
        //         postLoginAttemptCode = null;
        //     }
        // }
    }
}
