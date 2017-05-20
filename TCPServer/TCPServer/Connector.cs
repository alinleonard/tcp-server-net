using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TCPServer.Entity;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace TCPServer
{
    public class Connector
    {
        private string _clientAddress;
        private int _clientPort;

        private string _clientURI;

        public Connector(string clientAddress, Int32 clientPort)
        {
            _clientAddress = clientAddress;
            _clientPort = clientPort;

            _clientURI = "http://" + clientAddress + ":" + clientPort;
        }

        public async Task WebRequestPostTask(List<EntityFM112X> data)
        {
            await Task.Run(() =>
            {
                WebRequestPOST(data);
            });
        }

        private void WebRequestPOST(List<EntityFM112X> data)
        {
            try
            {
                foreach (EntityFM112X entity in data)
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(_clientURI + "/api/records/" + data[0].IMEI + "/");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = JsonConvert.SerializeObject(entity);

                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Print.PrintError(ex.Message);
                Log.WriteErrorLog(string.Format("Message:\n{0}\nStackTrace:\n{1}", ex.Message, ex.StackTrace));
                return;
            } 

            Print.PrintInformation("Data sent correctly!");
        }

    }
}