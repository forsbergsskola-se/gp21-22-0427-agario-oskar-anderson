using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteEatFood : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Food"))
        {
            Destroy(col.gameObject);
        }
    }
}