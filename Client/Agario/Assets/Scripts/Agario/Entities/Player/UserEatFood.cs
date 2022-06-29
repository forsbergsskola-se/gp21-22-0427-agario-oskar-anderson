using Agario.Data;
using Agario.Entities.Food;
using UnityEngine;

namespace Agario.Entities.Player
{
    public class UserEatFood : MonoBehaviour
    {
        [SerializeField] private EatenFoodSender eatenFoodSender;
        [SerializeField] private PlayerInformation playerInformation;

        private void OnTriggerEnter2D(Collider2D col)
        {
            // Debug.Log(col.gameObject.name);
            if (col.gameObject.CompareTag("Food"))
                EatFood(col.gameObject);
        }

        private void EatFood(GameObject foodObject)
        {
            playerInformation.AddToSize(1);
            eatenFoodSender.AddFoodToSendQueue(foodObject.transform.position);
            Destroy(foodObject);
        }
    }
}
