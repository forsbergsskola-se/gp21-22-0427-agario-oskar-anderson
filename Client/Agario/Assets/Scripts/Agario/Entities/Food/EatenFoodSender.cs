using System;
using System.Collections.Generic;
using Agario.Data;
using Agario.Networking;
using UnityEngine;

namespace Agario.Entities.Food
{
    public class EatenFoodSender : MonoBehaviour
    {
        [SerializeField] private UdpConnection udpConnection;
        [SerializeField] private PlayerInformation playerInformation;

        private List<AnnoyingFakeVector2> eatenFoodPositionList = new();
        
        private void Start()
        {
            enabled = false;
        }

        public void AddFoodToSendQueue(Vector2 position)
        {
            Debug.Log("Food eaten at " + position);
            Debug.Log("Send this to the server!");
            
            eatenFoodPositionList.Add(new AnnoyingFakeVector2(position.x, position.y));
        }

        private void FixedUpdate()
        {
            var eatenFoodPackage =
                new NetworkPackage<AnnoyingFakeVector2[]>(PackageType.EatenFood, eatenFoodPositionList.ToArray());
            eatenFoodPositionList.Clear();
            udpConnection.SendPackage(eatenFoodPackage);
        }
    }
}
