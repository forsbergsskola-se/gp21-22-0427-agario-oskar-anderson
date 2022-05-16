using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Agario.Data;
using Agario.Entities.RemotePlayer;
using Agario.Networking;
using UnityEngine;

public class RemotePlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject remotePlayerPrefab;
    [SerializeField] private PlayerDataUnpacker playerDataUnpacker;
    [SerializeField] private PlayerInformation playerInformation;
    [SerializeField] private MainThreadQueue mainThreadQueue;
    
    public Queue<Action> actionQueue = new();
    
    public void SpawnRemotes(UserData[] remotePlayersData)
    {
        mainThreadQueue.ActionQueue.Enqueue(() =>
        {
            foreach (var userData in remotePlayersData)
            {
                if (userData.id == playerInformation.UserData.id)
                    continue;

                var remotePlayerObject = Instantiate(remotePlayerPrefab);
                var remotePlayer = remotePlayerObject.GetComponent<RemotePlayer>();
                remotePlayer.ApplyUserData(userData);
                playerDataUnpacker.currentRemotePlayers.Add(userData.id, remotePlayer);
                Debug.Log(userData.id);
                Debug.Log(userData.UserName);
                Debug.Log(userData.UserColor);
            }
        });
    }
    
    

    public void DeSpawnRemotes(UserData userData)
    {
        
        mainThreadQueue.ActionQueue.Enqueue(() =>
        {
            Debug.Log("Removing user: " + userData.UserName + userData.id);
            Destroy(playerDataUnpacker.currentRemotePlayers[userData.id].gameObject);
            playerDataUnpacker.currentRemotePlayers.Remove(userData.id);
            
        });
    }
}
