using Agario.Data;
using UnityEngine;
using Agario.Entities.RemotePlayer;


namespace Agario.Entities.Player
{
    public class PlayerEatPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerInformation playerInformation;
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                // I would prefer to access this from a more central place so I wouldn't need to use get component
                // this much. :/
                var otherPlayer = other.GetComponent<RemoteUser>();

                var maxDistanceBetweenCenterPoints =
                    Mathf.Abs(Size.GetTrueRadius(playerInformation.PlayerData.Size) - Size.GetTrueRadius(otherPlayer.PlayerData.Size));

                var currentCenterPointDistance = Vector3.Distance(transform.position, otherPlayer.transform.position);

                if (currentCenterPointDistance < maxDistanceBetweenCenterPoints)
                {
                    // One player is completely hidden by another, and will be killed.
                    if (playerInformation.PlayerData.Size > otherPlayer.PlayerData.Size)
                    {
                        Debug.Log("User ate a remote user! Send this to server!");
                    }
                    else
                    {
                        Debug.Log("User was eaten by another user!");
                    }
                }
            }
        }
    }
}
