using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TCPServer.Helper;
using TCPServer.Protocol;

namespace TCPServer
{
    public class AsyncTCPServer
    {
        #region init
        TcpListener server = null;
        FM112X fm112x = null;

        private int _port;

        public static int _clients = 0;
        public static List<TcpClient> clients = new List<TcpClient>();
        public static List<TcpClient> history = new List<TcpClient>();
        #endregion

        public AsyncTCPServer(int port)
        {
            _port = port;

            fm112x = new FM112X();

            fm112x.IMEI += Fm112x_IMEI;
            fm112x.AVLdataArray += Fm112x_AVLdataArray;
            fm112x.Error += Fm112x_Error;
        }

        #region events
        private void Fm112x_Error(object sender, string e)
        {
            PrintHelper.Error(e);
            LogUtility.WriteErrorLog(e);
        }

        private async void Fm112x_AVLdataArray(object sender, EventArgsAVLdataArray entry)
        {
            NetworkStream s = entry.client.GetStream();

            WriteStream(s, fm112x.ServerAcknowledgeReceivedData);
          
            await Program.connector.WebRequestPostTask(entry.data);
            
            PrintHelper.PrintRow(entry.client.Client.ProtocolType.ToString(), entry.client.Client.RemoteEndPoint.ToString(), fm112x.ServerAcknowledgeReceivedData.Length.ToString(), "SENDING DATA");
            TrafficUtility.AddSentTraffic(fm112x.ServerAcknowledgeReceivedData.Length);
        }

        private void Fm112x_IMEI(object sender, TcpClient c)
        {
            NetworkStream s = c.GetStream();

            WriteStream(s, fm112x.ByteAcceptData);

            PrintHelper.PrintRow(c.Client.ProtocolType.ToString(), c.Client.RemoteEndPoint.ToString(), fm112x.ByteAcceptData.Length.ToString(), "SENDING DATA");
            TrafficUtility.AddSentTraffic(fm112x.ByteAcceptData.Length);
        }
        #endregion

        #region tcp
        public void Start()
        {
            try
            {
                Console.WriteLine("TCP Server is starting...");

                server = new TcpListener(IPAddress.Any, _port);
                server.Start();
                _clients = 0;

                Console.WriteLine("TCP Server is running!");
                Console.WriteLine("TCP Listening on port: " + _port);
                PrintHelper.PrintLine();
                PrintHelper.PrintRow("Protocol", "Ip", "Bandwith (Bytes)", "Status");

                while (true)
                {
                    // waiting for connection
                    TcpClient client = server.AcceptTcpClient();
                    Task.Run(() =>
                    {
                        clients.Add(client);
                        history.Add(client);
                        _clients++;
                        HandleConnectionAsync(client);
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void HandleConnectionAsync(TcpClient client)
        {
            PrintHelper.PrintLine();
            PrintHelper.PrintRow(client.Client.ProtocolType.ToString(), client.Client.RemoteEndPoint.ToString(), client.Client.Available.ToString(), "ESTABLISHED");

            try
            {
                byte[] bytes = new Byte[1024]; // 1 KB buffer
                //string data = null;
                int i;
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    //client established correctly 
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var data = BitConverter.ToString(bytes, 0, i).Replace("-", string.Empty);

                        PrintHelper.PrintRow(client.Client.ProtocolType.ToString(), client.Client.RemoteEndPoint.ToString(), i.ToString(), "RECEIVING DATA");
                        TrafficUtility.AddReceivedTraffic(i);

                        if (Program.debug)
                            PrintHelper.PrintDump(data);

                        fm112x.Parse(client, data);    
                    }

                    if (i == 0)
                        break;
                }

                PrintHelper.PrintRow(client.Client.ProtocolType.ToString(), client.Client.RemoteEndPoint.ToString(), client.Client.Available.ToString(), "CLOSED");
                _clients--;
                clients.Remove(client);
            }
            catch (Exception ex)
            {
                PrintHelper.Error(ex.Message);
                LogUtility.WriteErrorLog(string.Format("Message:\n{0}\nStackTrace:\n{1}", ex.Message, ex.StackTrace));
            }
        }

        public void WriteStream(NetworkStream stream, byte[] bytes)
        {
            if (stream != null)
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }
        #endregion
    }

}
