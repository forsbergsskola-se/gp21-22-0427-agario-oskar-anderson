using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

using Agario.Data;
using Agario.Networking;



public class TcpConnection : MonoBehaviour
{
    private TcpClient tcpClient;
    private StreamWriter streamWriter;
    private StreamReader streamReader;

    [SerializeField] LoginHandler loginHandler;
    
    public void SetupTcpConnection(string ipAddress, int port)
    {
        tcpClient = new TcpClient(ipAddress, port);
        
        streamWriter = new StreamWriter(tcpClient.GetStream());
        streamReader = new StreamReader(tcpClient.GetStream());

        new Thread(TcpListener).Start();

    }
    
    
    
    public void TcpListener()
    {
        while (true)
        {
            string receivedJson = streamReader.ReadLine();
            var basePackage = JsonUtility.FromJson<NetworkPackage>(receivedJson);
            
            loginHandler.ReturnMessage = receivedJson;
            

            switch (basePackage.Id)
            {
                case (int)NetworkProtocol.RequestType.UserData:
                    loginHandler.CompleteLoginSequence(JsonUtility.FromJson<NetworkPackage<UserData>>(receivedJson));
                    break;
            }
        }
    }
    
        
    
    /// <summary>
    /// Sends the provided networkPackage to the connected server.
    /// </summary>
    /// <param name="networkPackage"></param>
    public void SendPackage(NetworkPackage networkPackage)
    {
        string jsonMessage = JsonUtility.ToJson(networkPackage);
        streamWriter.WriteLine(jsonMessage);
        streamWriter.Flush();
    }
}
