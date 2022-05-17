using System.Collections;
using System.Collections.Generic;
using Agario.Data;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private PlayerInformation playerInformation;

    // Update is called once per frame
    void FixedUpdate()
    {
        // float height = -12 - playerInformation.PlayerData.Size * 0.01f;
        transform.position = target.transform.position + new Vector3(0, 0, -12);
    }
}
