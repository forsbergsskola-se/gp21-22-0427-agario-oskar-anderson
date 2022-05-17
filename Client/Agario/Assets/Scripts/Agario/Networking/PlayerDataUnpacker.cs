using System.Collections.Generic;
using Agario.Data;
using Agario.Entities.RemotePlayer;
using UnityEngine;



namespace Agario.Networking
{
    public class PlayerDataUnpacker : MonoBehaviour
    {
        [SerializeField] private PlayerInformation playerInformation;
        
        public Dictionary<int, RemoteUser> currentRemotePlayers = new();


        public void UnpackRemotePlayersData(PlayerData[] remotePlayersData)
        {
            foreach (var remotePlayerData in remotePlayersData)
            {
                if (remotePlayerData.PlayerId == playerInformation.UserData.id)
                {
                    continue;
                }
                
                if (!currentRemotePlayers.ContainsKey(remotePlayerData.PlayerId))
                {
                    continue;
                }

                currentRemotePlayers[remotePlayerData.PlayerId].PlayerData = remotePlayerData;
            }
        }

        public void HandlePlayerHideRequest(int playerId)
        {
            if (currentRemotePlayers.TryGetValue(playerId, out var remoteUser))
            {
                remoteUser.GetComponent<ResetPlayer>().HideAndReset();
            }
            else if (playerId == playerInformation.PlayerData.PlayerId)
            {
                playerInformation.GetComponent<ResetPlayer>().HideAndReset();
            }
        }

        public void HandlePlayerShowRequest(int playerId)
        {
            if (currentRemotePlayers.TryGetValue(playerId, out var remoteUser))
            {
                remoteUser.GetComponent<ResetPlayer>().Show();
            }
            else if (playerId == playerInformation.PlayerData.PlayerId)
            {
                playerInformation.GetComponent<ResetPlayer>().Show();
            }
        }
    }
}