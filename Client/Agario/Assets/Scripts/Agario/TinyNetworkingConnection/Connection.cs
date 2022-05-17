using System.Net;
using TNC.Interfaces;

namespace TNC
{
    public class Connection
    {
        private enum ConnectionMode { Host, Client }
        
        // References to external required tools.
        private IOutputLogger outputLogger;
        private IJsonSerializer jsonSerializer;

        /// <summary>
        /// Holds if the connection is a host or client.
        /// </summary>
        private ConnectionMode connectionMode;

        public Connection(IOutputLogger outputLogger, IJsonSerializer jsonSerializer)
        {
            this.outputLogger = outputLogger;
            this.jsonSerializer = jsonSerializer;
        }



        /// <summary>
        /// Create and 
        /// </summary>
        /// <param name="targetIp"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private bool EstablishConnection(IPAddress targetIp, int port)
        {
            ConnectionManager connectionManager = new();
            
            
            
            return false;
        }
    }
}
