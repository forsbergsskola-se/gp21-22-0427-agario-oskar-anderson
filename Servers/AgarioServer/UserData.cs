namespace AgarioServer;

/// <summary>
/// Holds information about a user.
/// </summary>
public class UserData
{
    public string UserName { get; private set; }
    public Color UserColor { get; private set; }
    public sbyte id;



    public UserData(string userName, Color userColor, sbyte id)
    {
        this.id = id;
        UserName = userName;
        UserColor = userColor;
    }
}

/// <summary>
/// Local struct to mimic the unity Color class.
/// </summary>
public struct Color
{
    public float r;
    public float g;
    public float b;
    public float a;

    public Color(float r, float g, float b, float a = 1)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }
}