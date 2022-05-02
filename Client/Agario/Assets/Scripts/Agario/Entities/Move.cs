using UnityEngine;

namespace Agario.Entities
{
    public class Move : MonoBehaviour
    {
        [SerializeField] private Transform userTransform;
        [SerializeField] private float movementSpeed = 0;


    
        /// <summary>
        /// Move towards the point given with the maximum speed of this Move object.
        /// </summary>
        /// <param name="targetPosition"></param>
        public void MoveTowards(Vector3 targetPosition)
        {
            userTransform.position = Vector3.MoveTowards(userTransform.position, targetPosition, movementSpeed);
        }
    
    
    
        /// <summary>
        /// Move this GameObject to the provided position.
        /// </summary>
        /// <param name="position"></param>
        public void MoveToPosition(Vector3 position)
        {
            userTransform.position = position;
        }
    }

}