using System;
using System.Collections.Generic;
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
    [SerializeField] private RemotePlayerSpawner remotePlayerSpawner;

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
            try
            {
                string receivedJson = streamReader.ReadLine();
                
                loginHandler.ReturnMessage = receivedJson;
                
                var basePackage = JsonUtility.FromJson<NetworkPackage>(receivedJson);
                

                switch (basePackage.Id)
                {
                    case PackageType.UserData:
                        loginHandler.CompleteTcpLoginSequence(JsonUtility.FromJson<NetworkPackage<UserData>>(receivedJson));
                        break;
                    case PackageType.NewUsers:
                        remotePlayerSpawner.SpawnRemotes(JsonUtility.FromJson<NetworkPackage<UserData[]>>(receivedJson).Value);
                        break;
                }
            }
            catch (SocketException)
            {
                // The connection was lost. So stop the loop.
                break;
            }
        }
    }


    private void OnDestroy()
    {
        SendPackage(new NetworkPackage(PackageType.Disconnect));
        
        streamReader?.Close();
        streamReader = null;
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
