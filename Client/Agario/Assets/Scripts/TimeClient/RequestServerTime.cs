using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class RequestServerTime : MonoBehaviour
{
    [SerializeField] private string ipAddress;
    [SerializeField] private int port;

    [SerializeField] private UpdateTimeText updateTimeText;
    private UpdateTimeText.SetTime setTime;

    public void Start()
    {
        setTime = updateTimeText.SetText;
    }

    public void SendRequest()
    {
        TcpClient timeClient = new TcpClient(ipAddress, port);

        var stream = timeClient.GetStream();

        var buffer = new byte[22];

        stream.Read(buffer, 0, 22);
        
        setTime(Encoding.ASCII.GetString(buffer));
    }
}
