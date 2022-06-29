using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadQueue : MonoBehaviour
{
    /// <summary>
    /// Queue of actions to perform on main thread.
    /// </summary>
    public readonly Queue<Action> ActionQueue = new();
    
    
    /// <summary>
    /// Runs and removes all actions in the current actionQueue.
    /// </summary>
    private void FixedUpdate()
    {
        lock (ActionQueue)
        {
            for (int i = 0; i < ActionQueue.Count; i++)
            {
                ActionQueue.Dequeue().Invoke();
            }
        }
    }
}
