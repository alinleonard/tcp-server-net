using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Helper
{
    public class CommandsHelper
    {
        public static bool DEBUG = false;

        public static void WriteCommand(string c)
        {
            switch (c)
            {
                case "exit":
                    Environment.Exit(0);
                    break;
                case "debug":
                    Program.debug = !Program.debug;
                    Console.WriteLine("Set debug: {0}", Program.debug);
                    break;
                case "clients":
                    Console.WriteLine("Clients connected: {0}", AsyncTCPServer.clients.Count);
                    break;
                case "clientsinfo":
                    if(AsyncTCPServer.clients.Count != 0)
                        foreach(TcpClient i in AsyncTCPServer.clients)
                            PrintHelper.PrintRow(i.Client.ProtocolType.ToString(), i.Client.RemoteEndPoint.ToString(), i.Client.Available.ToString());
                    else
                        Console.WriteLine("No clients connected");
                    break;
                case "clientshistory":
                    if(AsyncTCPServer.history.Count != 0)
                        foreach(TcpClient i in AsyncTCPServer.history)
                            PrintHelper.PrintRow(i.Client.ProtocolType.ToString(), i.Client.RemoteEndPoint.ToString(), i.Client.Available.ToString());
                    else
                        Console.WriteLine("No history");
                    break;
                case "clearclientshistory":
                    Console.WriteLine("Cleared {0} clients history", AsyncTCPServer.history.Count);
                    AsyncTCPServer.history.Clear();
                    break;
                case "traffic":
                    TrafficUtility.DisplayTotal();
                    break;
                case "ev":
                    Process.Start("eventvwr", "/c:Application");
                    break;
                case "checktcpconnection":
                    CheckTCPConnection();
                    break;
                case "checkwrconnector":
                    CheckWebRequestConnector();
                    break;
                default:
                    WriteHelp();
                    break;
            }
        }

        private async static void CheckWebRequestConnector()
        {
            try
            {
                bool result = await Program.connector.CheckServerStatus();
                if(result)
                    Console.WriteLine("Connector online");
                else
                    Console.WriteLine("Connector offline");
            }
            catch (Exception ex)
            {
                PrintHelper.Error(ex.Message);
                LogUtility.WriteExceptionLog(ex);
            }
        }

        private static void CheckTCPConnection()
        {
            try
            {
                Console.WriteLine("Setting connection....");
                TcpClient client = new TcpClient();
                client.Connect("localhost", Properties.Settings.Default.TCP_SERVER_PORT);
                Console.WriteLine("Connected");
                client.Close();
                Console.WriteLine("Connection closed");
            }
            catch(Exception ex)
            {
                PrintHelper.Error(ex.Message);
                LogUtility.WriteExceptionLog(ex);
            }
        }

        private static void WriteHelp()
        {
            PrintHelper.PrintLine();
            PrintHelper.PrintRow("HELP");
            PrintHelper.PrintLine();
            PrintHelper.PrintDump("exit -> Exit the application.");
            PrintHelper.PrintDump("debug -> Toggle Enable/Disable debug mode");
            PrintHelper.PrintDump("clients -> Display the number of clients currently connected to the server");
            PrintHelper.PrintDump("clientsinfo -> Display extended information about the connected clients on the server.");
            PrintHelper.PrintDump("clientshistory -> Display the history of the connected clients since the starding of the server");
            PrintHelper.PrintDump("clearclientshistory -> Clear the history of the connected clients (perfomance recommendation)");
            PrintHelper.PrintDump("traffic -> Display the traffic sent and received since application started");
            PrintHelper.PrintDump("checktcpconnection -> Check if the tcp connection work by creating a client and connection to the server");
            PrintHelper.PrintDump("checkwrconnector -> Check if the connector works with the web request method");
            PrintHelper.PrintDump("ev -> Open Event Viewer.");
            PrintHelper.PrintLine();
        }
    }
}
