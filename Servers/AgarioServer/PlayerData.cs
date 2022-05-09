namespace AgarioServer;

public class PlayerData
{
    public int PlayerId;
    public float Size = 0;
    public float XPosition;
    public float YPosition;

    public bool SameIdAs(PlayerData playerData)
    {
        return PlayerId == playerData.PlayerId;
    }

    public bool SamePositionAs(PlayerData playerData)
    {
        return XPosition == playerData.XPosition && YPosition == playerData.YPosition;
    }
}