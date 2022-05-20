using System;
using System.Threading;
using Agario.Data;
using UnityEngine;

namespace Agario.Entities.Remote_Player
{
    public class RemotePlayer : MonoBehaviour
    {
        [SerializeField] private Move playerMove;
        [SerializeField] private Size playerSize;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [HideInInspector] public UserData UserData;
        [HideInInspector] public PlayerData PlayerData;


        [HideInInspector] public Vector3 LastPosition;

        public EventWaitHandle NewPlayerDataAvailable = new EventWaitHandle(false, EventResetMode.ManualReset);
        
        private void FixedUpdate()
        {
            var playerPosition = new Vector3(PlayerData.XPosition, PlayerData.YPosition, 0);
            playerMove.SetPosition(playerPosition);
            playerSize.SetSizeFromScore(PlayerData.Size);
        }


        private Vector3 currentVelocity;
        
        
        // private Vector3 lastPosition;
        
        // private void Update()
        // {
        //     if (NewPlayerDataAvailable.WaitOne(0))
        //     {
        //         // if (LastPosition == Vector3.zero)
        //         // {
        //         //
        //         // }
        //         
        //         var playerPosition = new Vector3(PlayerData.XPosition, PlayerData.YPosition, 0);
        //
        //         if (Vector3.Distance(LastPosition + currentVelocity, playerPosition) < 0.04f ||
        //             LastPosition == Vector3.zero)
        //         {
        //             playerMove.SetPosition(playerPosition);
        //             
        //
        //         }
        //         else
        //         {
        //             playerMove.SetPosition(transform.position + currentVelocity * Time.deltaTime);
        //             Debug.Log("Bad package");
        //         }
        //         
        //         
        //         
        //         playerSize.SetSizeFromScore(PlayerData.Size);
        //
        //
        //         currentVelocity = (playerPosition - LastPosition).normalized * 0.04f;
        //
        //         LastPosition = playerPosition;
        //         NewPlayerDataAvailable.Reset();
        //         
        //         
        //
        //         // return;
        //     }
        //     
        //     
        // }
        
        

        public void ApplyUserData(UserData userData)
        {
            UserData = userData;
            spriteRenderer.color = userData.UserColor;
        }
    }
}
