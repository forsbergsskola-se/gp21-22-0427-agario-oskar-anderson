using System;
using System.Collections.Generic;
using Agario.Data;
using Agario.Entities.RemotePlayer;
using Agario.Networking;
using UnityEngine;

public class RemotePlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject remotePlayerPrefab;
    [SerializeField] private PlayerDataUnpacker playerDataUnpacker;
    [SerializeField] private PlayerInformation playerInformation;
    
    public void SpawnRemotes(UserData[] remotePlayersData)
    {
        actionQueue.Enqueue(() =>
        {
            foreach (var userData in remotePlayersData)
            {
                if (userData.id == playerInformation.UserData.id)
                    continue;

                var remotePlayerObject = Instantiate(remotePlayerPrefab);
                var remotePlayer = remotePlayerObject.GetComponent<RemotePlayer>();
                remotePlayer.ApplyUserData(userData);
                playerDataUnpacker.currentRemotePlayers[userData.id] = remotePlayer;
            }
        });
    }

    public Queue<Action> actionQueue = new();


    private void FixedUpdate()
    {
        
        // TODO: I am quite worried here that the position updates gets irregular because of fixed update not being perfectly synced with the server.
        // Might be worth using a coroutine with wait handles and update positions right after they are available instead.
        
        
        lock (actionQueue)
        {
            for (int i = 0; i < actionQueue.Count; i++)
            {
                actionQueue.Dequeue().Invoke();
            }
        }
        
    }
}
