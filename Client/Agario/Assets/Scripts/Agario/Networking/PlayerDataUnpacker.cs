using System.Collections.Generic;
using Agario.Data;
using Agario.Entities.Remote_Player;
using UnityEngine;



namespace Agario.Networking
{
    public class PlayerDataUnpacker : MonoBehaviour
    {
        [SerializeField] private PlayerInformation playerInformation;
        [SerializeField] private MainThreadQueue mainThreadQueue;
        
        
        public Dictionary<int, RemotePlayer> currentRemotePlayers = new();


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
                currentRemotePlayers[remotePlayerData.PlayerId].NewPlayerDataAvailable.Set();
            }
        }

        public void HandlePlayerHideRequest(int playerId)
        {
            mainThreadQueue.ActionQueue.Enqueue(() =>
            {
                if (currentRemotePlayers.TryGetValue(playerId, out var remoteUser))
                {
                    remoteUser.GetComponent<ResetPlayer>().HideAndReset();
                }
                // else if (playerId == playerInformation.PlayerData.PlayerId)
                // {
                //     playerInformation.GetComponent<ResetPlayer>().HideAndReset();
                // }
            });
        }

        public void HandlePlayerShowRequest(int playerId)
        {
            mainThreadQueue.ActionQueue.Enqueue(() =>
            {
                if (currentRemotePlayers.TryGetValue(playerId, out var remoteUser))
                {
                    remoteUser.GetComponent<ResetPlayer>().Show();
                }
                else if (playerId == playerInformation.PlayerData.PlayerId)
                {
                    playerInformation.GetComponent<ResetPlayer>().Show();
                }
            });
        }
    }
}