using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Agario.Entities.Food
{
    public class FoodSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject foodPrefab;
        private Queue<Action> spawnQueue = new();


        public void AddFoodToSpawnQueue(Vector2[] positions)
        {
            spawnQueue.Enqueue(() =>
            {
                foreach (var position in positions)
                {
                    Instantiate(foodPrefab, position, quaternion.identity);
                }
            });
        }
    
        private void Update()
        {
            lock (spawnQueue)
            {
                for (int i = 0; i < spawnQueue.Count; i++)
                {
                    spawnQueue.Dequeue().Invoke();
                }
            }
        }
    }
}
