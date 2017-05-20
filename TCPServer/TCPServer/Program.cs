using System;
using System.Threading.Tasks;
using TCPServer.Helper;

namespace TCPServer
{
    class Program
    {
        public static bool debug = false;

        AsyncTCPServer server = null;
        public static WebAppConnector connector = null;

        static void Main(string[] args)
        {
            Program instance = new Program();
            
            instance.StartTCPIPserver(Properties.Settings.Default.TCP_SERVER_PORT);
            instance.StartWebAppConnectorServer(Properties.Settings.Default.CONNECTOR_SERVER_PORT);

            while (true)
            {
                var read = Console.ReadLine();
                CommandsHelper.WriteCommand(read);
            }
        }

        private void StartTCPIPserver(int port)
        {
            Task.Run(() =>
            {
                server = new AsyncTCPServer(port);
                server.Start();
            });
        }

        private void StartWebAppConnectorServer(int port)
        {
            Task.Run(() =>
            {
                connector = new WebAppConnector(port);
                connector.Start();
            });
        }
    }
}
