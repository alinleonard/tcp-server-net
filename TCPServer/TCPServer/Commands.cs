using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class Commands
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
                    DEBUG = !DEBUG;
                    AsyncTCPServer.SetDebug(DEBUG);
                    break;
                case "clients":
                    AsyncTCPServer.PrintClients();
                    break;
                case "traffic":
                    Traffic.DisplayTotal();
                    break;
                case "ev":
                    Process.Start("eventvwr", "/c:Application");
                    break;
                default:
                    WriteHelp();
                    break;
            }
        }

        private static void WriteHelp()
        {
            Print.PrintLine();
            Print.PrintRow("HELP");
            Print.PrintLine();
            Print.PrintRow("exit -> Exit the application.");
            Print.PrintRow("debug -> Toggle Enable/Disable debug mode");
            Print.PrintRow("clients -> Display the number of clients currently connected to the server");
            Print.PrintRow("traffic -> Display the traffic sent and received since application started");
            Print.PrintRow("ev -> Open Event Viewer.");
            Print.PrintLine();
        }
    }
}
