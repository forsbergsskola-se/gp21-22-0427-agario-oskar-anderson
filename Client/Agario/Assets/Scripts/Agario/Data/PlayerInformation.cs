using Agario.Entities;
using UnityEngine;

namespace Agario.Data
{
    public class PlayerInformation : MonoBehaviour
    {
        public PlayerData PlayerData;
        public UserData UserData;

        [SerializeField] private Size size;
        [SerializeField] private SpriteRenderer bodyRenderer;

        public void SetData2dPosition(Vector3 position)
        {
            PlayerData.XPosition = position.x;
            PlayerData.YPosition = position.y;
        }

        public void AddToSize(int sizeToAdd)
        {
            PlayerData.Size += sizeToAdd;
            size.SetSizeFromScore(PlayerData.Size);
        }

        public void ApplyColor()
        {
            bodyRenderer.color = UserData.UserColor;
        }
    }
}
