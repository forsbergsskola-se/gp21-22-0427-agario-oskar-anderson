using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Agario.Entities.Food
{
    public class FoodSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject foodPrefab;
        [SerializeField] private MainThreadQueue mainThreadQueue;
        
        private Queue<Action> spawnQueue = new();


        public void AddFoodToSpawnQueue(AnnoyingFakeVector2[] positions)
        {
            lock (spawnQueue)
            {
                mainThreadQueue.ActionQueue.Enqueue(() =>
                {
                    foreach (var position in positions)
                    {
                        var unityVector = new Vector3(position.X, position.Y, 0);
                        Instantiate(foodPrefab, unityVector, quaternion.identity);
                    }
                });
            }
        }
    }
}
