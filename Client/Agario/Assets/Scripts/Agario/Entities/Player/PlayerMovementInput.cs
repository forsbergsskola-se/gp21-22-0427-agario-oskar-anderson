using UnityEngine;

namespace Agario.Entities.Player
{
    public class PlayerMovementInput : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Move playerMove;
    


        private void FixedUpdate()
        {
            if (TryGetWorldSpaceMousePosition(out Vector3 position))
            {
                playerMove.MoveTowards(position);
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
            if (Physics.Raycast(t, out RaycastHit hit))
            {
                position = hit.point;
                return true;
            }

            position = Vector3.zero;
            return false;
        }
    }
}