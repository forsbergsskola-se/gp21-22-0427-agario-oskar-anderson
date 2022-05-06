using System.Collections.Generic;
using Agario.Data;
using Agario.Entities.RemotePlayer;
using UnityEngine;



namespace Agario.Networking
{
    public class PlayerDataUnpacker : MonoBehaviour
    {
        [SerializeField] private PlayerInformation playerInformation;
        
        private Dictionary<int, RemotePlayer> currentRemotePlayers;


        public void UnpackRemotePlayersData(PlayerData[] remotePlayersData)
        {
            foreach (var remotePlayerData in remotePlayersData)
            {
                if (remotePlayerData.PlayerId == playerInformation.UserData.id)
                {
                    continue;
                }
                
                if (currentRemotePlayers.ContainsKey(remotePlayerData.PlayerId))
                {
                    continue;
                }

                currentRemotePlayers[remotePlayerData.PlayerId].PlayerData = remotePlayerData;
            }
        }
    }
}