using System;


namespace TCPServer.Helper
{
    public class TrafficUtility
    {
        private static decimal _totalReceived = 0;
        private static decimal _totalSent = 0;

        public static void AddReceivedTraffic(decimal v)
        {
            _totalReceived += v;
        }

        public static void AddSentTraffic(decimal v)
        {
            _totalSent += v;
        }

        public static void DisplayTotal()
        {
            Console.WriteLine("Traffic received: {0}/Bytes {1}/KB", _totalReceived, _totalReceived / 1024);
            Console.WriteLine("Traffic sent: {0}/Bytes {1}/KB", _totalSent, _totalSent / 1024);
        }
    }
}
