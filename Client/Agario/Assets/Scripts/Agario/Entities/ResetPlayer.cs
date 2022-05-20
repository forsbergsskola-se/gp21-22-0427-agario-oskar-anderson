using Agario.Data;
using Agario.Entities;
using Agario.Entities.RemotePlayer;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    [SerializeField] private RemoteUser remoteUser;
    [SerializeField] private PlayerInformation playerInformation;
    
    public void HideAndReset()
    {
        if (remoteUser != null)
        {
            remoteUser.PlayerData.Size = 0;
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
