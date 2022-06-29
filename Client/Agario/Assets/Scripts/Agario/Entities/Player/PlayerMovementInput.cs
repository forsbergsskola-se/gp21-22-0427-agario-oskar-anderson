using System;
using UnityEngine;

namespace Agario.Entities.Player
{
    public class PlayerMovementInput : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private PlayerMovement playerMovement;


        private void Start()
        {
            enabled = false;
        }


        private void FixedUpdate()
        {
            if (TryGetWorldSpaceMousePosition(out Vector3 position))
            {
                position.z = transform.position.z;
                playerMovement.MoveTowards(position);
            }
        }
    


        /// <summary>
        /// Gets the mouse position over the game area.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>bool if over the game world or not, and position of mouse if over the world</returns>
        private bool TryGetWorldSpaceMousePosition(out Vector3 position)
        {
            var t = playerCamera.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = LayerMask.GetMask("Background");
            if (Physics.Raycast(t, out RaycastHit hit, 100f, mask))
            {
                position = hit.point;
                return true;
            }

            position = Vector3.zero;
            return false;
        }
    }
}