using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class RequestServerTime : MonoBehaviour
{
    [SerializeField] private string ipAddress;
    [SerializeField] private int port;
    
    public void SendRequest()
    {
        TcpClient timeClient = new TcpClient(ipAddress, port);

        var stream = timeClient.GetStream();

        var buffer = new byte[22];

        stream.Read(buffer, 0, 22);

        var resultString = Encoding.ASCII.GetString(buffer);

        Debug.Log(resultString);
    }
}
