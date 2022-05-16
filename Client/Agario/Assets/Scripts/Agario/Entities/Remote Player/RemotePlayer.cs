using Agario.Data;
using UnityEngine;

namespace Agario.Entities.RemotePlayer
{
    public class RemotePlayer : MonoBehaviour
    {
        [SerializeField] private Move playerMove;
        [SerializeField] private Size playerSize;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [HideInInspector] public UserData UserData;
        [HideInInspector] public PlayerData PlayerData;

        private void FixedUpdate()
        {
            var playerPosition = new Vector3(PlayerData.XPosition, PlayerData.YPosition, 0);
            playerMove.SetPosition(playerPosition);
            playerSize.SetSizeFromScore(PlayerData.Size);
        }

        public void ApplyUserData(UserData userData)
        {
            UserData = userData;
            spriteRenderer.color = userData.UserColor / 255;
        }
    }
}
