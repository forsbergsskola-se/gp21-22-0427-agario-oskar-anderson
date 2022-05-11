using Agario.Data;
using Agario.Networking;
using UnityEngine;

namespace Agario.Entities.Food
{
    public class EatenFoodSender : MonoBehaviour
    {
        [SerializeField] private UdpConnection udpConnection;
        [SerializeField] private PlayerInformation playerInformation;

        private void Start()
        {
            enabled = false;
        }

        public void AddFoodToSendQueue(Vector2 position)
        {
            Debug.Log("Food eaten at " + position);
            Debug.Log("Send this to the server!");
        }
    }
}
