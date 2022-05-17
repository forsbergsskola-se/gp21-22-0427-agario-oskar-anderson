using System;
using System.Collections;
using Agario.Data;
using UnityEngine;
using Agario.Entities.RemotePlayer;
using Random = UnityEngine.Random;


namespace Agario.Entities.Player
{
    public class PlayerEatPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerInformation playerInformation;
        [SerializeField] private TcpConnection tcpConnection;
        [SerializeField] private ResetPlayer userReset;

        [SerializeField] private float respawnTime;
        
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
                        Debug.Log("User ate a remote user! Sending this to server!");
                        var package = new NetworkPackage<PlayerData>(PackageType.EatenPlayer, otherPlayer.PlayerData);
                        tcpConnection.SendPackage(package);
                        otherPlayer.GetComponent<ResetPlayer>().HideAndReset();
                    }
                    else
                    {
                        Debug.Log("User was eaten by another user!");
                        
                        StartCoroutine(RespawnTime());
                    }
                }
            }
        }

        private IEnumerator RespawnTime()
        {
            userReset.HideAndReset();
            yield return new WaitForSeconds(respawnTime);
            userReset.Show();
            transform.position = new Vector3(Random.Range(-49, 49), Random.Range(-49, 49), 0);
            var package = new NetworkPackage<int>(PackageType.Show, playerInformation.PlayerData.PlayerId);
            tcpConnection.SendPackage(package);
        }
    }
}
