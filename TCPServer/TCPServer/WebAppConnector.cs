using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TCPServer.Entity;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using TCPServer.Helper;

namespace TCPServer
{
    /*  https://msdn.microsoft.com/en-us/library/zt39148a(v=vs.110).aspx 
     *  Maybe implement a service later on.
     */
    public class WebAppConnector
    {
        private string _clientAddress;
        private int _clientPort;

        private string _clientURI;

        public WebAppConnector(Int32 clientPort)
        {
            _clientAddress = "localhost";
            _clientPort = clientPort;

            _clientURI = "http://" + _clientAddress + ":" + clientPort;
        }

        public void Start()
        {
            
        }

        public async Task WebRequestPostTask(List<EntityFM112X> data)
        {
            await Task.Run(() =>
            {
                WebRequestPOST(data);
            });
        }

        public async Task<bool> CheckServerStatus()
        {
            return await Task.Run(() =>
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(_clientURI + "/api/");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    var result = httpResponse.GetResponseHeader("Content-Type");
                    return true;
                }
                catch (Exception ex)
                {
                    PrintHelper.Error(ex.Message);
                    LogUtility.WriteExceptionLog(ex);
                    return false;
                }
                
            });
        }

        private void WebRequestPOST(List<EntityFM112X> data)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(_clientURI + "/api/records/" + data[0].IMEI + "/");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                foreach (EntityFM112X value in data)
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = JsonConvert.SerializeObject(value);

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
                PrintHelper.Error(ex.Message);
                LogUtility.WriteExceptionLog(ex);
                return;
            } 

            PrintHelper.PrintInformation("The data was sent correctly!");
        }

    }
}