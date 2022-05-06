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
        foreach (var userData in remotePlayersData)
        {
            if (userData.id == playerInformation.UserData.id)
                continue;
            
            var remotePlayerObject = Instantiate(remotePlayerPrefab);
            var remotePlayer = remotePlayerObject.GetComponent<RemotePlayer>();
            remotePlayer.ApplyUserData(userData);
        }
    }
}
