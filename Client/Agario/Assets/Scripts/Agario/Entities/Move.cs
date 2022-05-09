using UnityEngine;



namespace Agario.Entities
{
    public class Move : MonoBehaviour
    {
        [SerializeField] private Transform userTransform;
        
        
        
        /// <summary>
        /// Set this GameObject to the provided position.
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector3 position)
        {
            userTransform.position = position;
        }
    }

}