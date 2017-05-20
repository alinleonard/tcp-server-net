using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCPServer.Entity;

namespace TCPServer.Protocol
{
    public class FM112X
    {
        #region config
        public readonly byte[] ByteAcceptData = { 01 };
        public readonly byte[] ByteDeclineData = { 00 };

        public readonly byte[] ServerAcknowledgeReceivedData = { 00000002 };

        int IMEIByteLength = 34;
        #endregion

        #region initialize
        private List<EntityFM112X> _records;
        private EntityFM112X _EntityFM112X;
        #endregion

        #region events
        public event EventHandler<TcpClient> IMEI;
        public event EventHandler<EventArgsAVLdataArray> AVLdataArray;
        public event EventHandler<string> Error;

        protected virtual void OnIMEI(TcpClient client)
        {
            (IMEI as EventHandler<TcpClient>)?.Invoke(this, client);
        }

        protected virtual void OnAVLdataArray(EventArgsAVLdataArray entry)
        {
            (AVLdataArray as EventHandler<EventArgsAVLdataArray>)?.Invoke(this, entry);
        }

        protected virtual void OnError(string message)
        {
            (Error as EventHandler<string>)?.Invoke(this, message);
        }
        #endregion

        public FM112X()
        {
            _records = new List<EntityFM112X>();
            _EntityFM112X = new EntityFM112X();
        }

        public void Parse(TcpClient c,string d)
        {
            if (IfIMEI(d))
            {
                ExtractIMEI(d);
                OnIMEI(c);
            }
                
            if (IfAVLdataArray(d))
            {
                ExtractAVLdataArray(d);
                OnAVLdataArray(new EventArgsAVLdataArray { client = c, data = _records });
                PrintAVLdataArrayResults();
            }
        }

        #region print
        private void PrintAVLdataArrayResults()
        {
            foreach (EntityFM112X result in _records)
            {
                Console.WriteLine();
                Console.WriteLine("IMEI: {0}", result.IMEI);
                Console.WriteLine("Timespan: {0}", result.TimeStamp);
                Console.WriteLine("Longitude: {0}", result.Longitude);
                Console.WriteLine("Latitude: {0}", result.Latitude);
                Console.WriteLine("Altitude: {0}", result.Altitude);
                Console.WriteLine("Speed: {0}", result.Speed);
                Console.WriteLine("Priority: {0}", result.Priority);
                Console.WriteLine();
            }
        }
        #endregion

        #region checking 
        private bool IfIMEI(string d)
        {
            if(d.StartsWith("000F") && d.Length == IMEIByteLength && d.Length % 2 == 0)
                return true;
            else
                return false;
        }

        private bool IfAVLdataArray(string d)
        {
            int avlarrayLength = (d.Length - 24) / 2;

            if (!d.StartsWith("00000000"))
                return false;
            if ((int)ConvertHexToDecimal(d.Substring(8, 8)) != avlarrayLength)
                return false;

            return true;
        }
        #endregion

        #region data decoding
        private void ExtractIMEI(string d)
        {
            string IMEI = ConvertHexToString(d);
            IMEI = IMEI.Trim();
            IMEI = IMEI.Replace("\0\u000f", "");

            _EntityFM112X.IMEI = IMEI;
        }

        private void ExtractAVLdataArray(string d)
        {
            int NumberOfData = (int)ConvertHexToDecimal(d.Substring(18, 2));

            for (int i = 1; i <= (NumberOfData/2); i++)
            {
                if (i == 1)
                {
                    _EntityFM112X.TimeStamp = ConvertHexToDecimal(d.Substring(20, 16));
                    _EntityFM112X.Priority = (int)ConvertHexToDecimal(d.Substring(36, 2));
                    _EntityFM112X.Longitude = ConvertHexToDecimal(d.Substring(38, 8));
                    _EntityFM112X.Latitude = ConvertHexToDecimal(d.Substring(46, 8));
                    _EntityFM112X.Altitude = (int)ConvertHexToDecimal(d.Substring(54, 4));
                    _EntityFM112X.Angle = (int)ConvertHexToDecimal(d.Substring(58, 4));
                    _EntityFM112X.VisibleSattelites = (int)ConvertHexToDecimal(d.Substring(62, 2));
                    _EntityFM112X.Speed = (int)ConvertHexToDecimal(d.Substring(64, 4));
                }
                else
                {
                    int index = (60 * i) + 20;
                    _EntityFM112X.TimeStamp = ConvertHexToDecimal(d.Substring(index, 16));
                    _EntityFM112X.Priority = (int)ConvertHexToDecimal(d.Substring(index + 16, 2));
                    _EntityFM112X.Longitude = ConvertHexToDecimal(d.Substring(index + 18, 8));
                    _EntityFM112X.Latitude = ConvertHexToDecimal(d.Substring(index + 26, 8));
                    _EntityFM112X.Altitude = (int)ConvertHexToDecimal(d.Substring(index + 34, 4));
                    _EntityFM112X.Angle = (int)ConvertHexToDecimal(d.Substring(index + 38, 4));
                    _EntityFM112X.VisibleSattelites = (int)ConvertHexToDecimal(d.Substring(index + 42, 2));
                    _EntityFM112X.Speed = (int)ConvertHexToDecimal(d.Substring(index + 44, 4));
                }

                _records.Add(_EntityFM112X);
            }
        }
        #endregion

        #region Internal   
        private static string ConvertHexToString(string HexValue)
        {
            string StrValue = "";
            while (HexValue.Length > 0)
            {
                StrValue += System.Convert.ToChar(System.Convert.ToUInt32(HexValue.Substring(0, 2), 16)).ToString();
                HexValue = HexValue.Substring(2, HexValue.Length - 2);
            }
            return StrValue;
        }

        private static long ConvertHexToDecimal(string HexValue)
        {
            HexValue = HexValue.Trim();
            long value = Convert.ToInt64(HexValue, 16);
            return value;
        }
        #endregion
    }

    public class EventArgsAVLdataArray
    {
        public TcpClient client { get; set; }
        public List<EntityFM112X> data { get; set; }
    }
}
