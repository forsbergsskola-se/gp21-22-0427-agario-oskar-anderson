using Agario.Data;
using Agario.Entities;
using Agario.Entities.Remote_Player;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    [SerializeField] private RemotePlayer remotePlayer;
    [SerializeField] private PlayerInformation playerInformation;
    
    public void HideAndReset()
    {
        if (remotePlayer != null)
        {
            remotePlayer.PlayerData.Size = 0;
            remotePlayer.LastPosition = Vector3.zero;
        }

        if (playerInformation != null)
        {
            playerInformation.PlayerData.Size = 0;
        }

        gameObject.GetComponent<Size>().SetSizeFromScore(0);
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
