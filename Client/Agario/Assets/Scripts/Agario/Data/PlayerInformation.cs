using UnityEngine;

using Agario.Data;



public class PlayerInformation : MonoBehaviour
{
    public PlayerData PlayerData;
    public UserData UserData;



    public void SetData2dPosition(Vector3 position)
    {
        PlayerData.XPosition = position.x;
        PlayerData.YPosition = position.y;
    }
}
