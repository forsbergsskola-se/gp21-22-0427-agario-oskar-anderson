using System.Collections;
using System.Collections.Generic;
using Agario.Entities;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Move playerMove;
    [SerializeField] private PlayerInformation playerInformation;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float movementSpeed = 0;
    
    /// <summary>
    /// Move towards the point given with the maximum speed of this Move object.
    /// </summary>
    /// <param name="targetPosition"></param>
    public void MoveTowards(Vector3 targetPosition)
    {
        var newPos = Vector3.MoveTowards(playerTransform.position, targetPosition, movementSpeed);

        playerInformation.SetData2dPosition(newPos);
        
        playerMove.SetPosition(newPos);
    }
}
