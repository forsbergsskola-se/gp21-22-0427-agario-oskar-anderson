using System;
using Agario.Data;
using Agario.Entities.Food;
using Agario.Networking;
using JetBrains.Annotations;
using UnityEngine;

namespace Agario.Entities.Player
{
    public class UserEatFood : MonoBehaviour
    {
        [SerializeField] private EatenFoodSender eatenFoodSender;
        [SerializeField] private PlayerInformation playerInformation;
        [SerializeField] private Size size;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            // Debug.Log(col.gameObject.name);

            switch (col.gameObject.tag)
            {
                case "Food":
                    // Collided object is food
                    EatFood(col.gameObject);
                    
                    break;
                case "Player":
                    // Collided object is a player
                    break;
            }
        }

        private void EatFood(GameObject foodObject)
        {
            playerInformation.AddToSize(1);
            eatenFoodSender.AddFoodToSendQueue(foodObject.transform.position);
            Destroy(foodObject);
        }
    }
}
