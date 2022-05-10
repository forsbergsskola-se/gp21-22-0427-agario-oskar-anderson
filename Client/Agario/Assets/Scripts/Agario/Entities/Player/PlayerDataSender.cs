using System;
using Agario.Data;
using Agario.Networking;
using UnityEngine;

namespace Agario.Entities.Player
{
    public class PlayerDataSender : MonoBehaviour
    {
        [SerializeField] private UdpConnection udpConnection;
        [SerializeField] private PlayerInformation playerInformation;


        private void Start()
        {
            enabled = false;
        }

        private void FixedUpdate()
        {
            var networkPackage = new NetworkPackage<PlayerData>(PackageType.PlayerData, playerInformation.PlayerData);

            udpConnection.SendPackage(networkPackage);
        }
    }
}
