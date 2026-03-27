using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.TestFunc.Litepoint
{
    internal class RFTestResult
    {


        public string Frequency { get; set; }
        public string DataRate { get; set; }
        public string Bandwidth { get; set; }
        public string Antenna { get; set; }



        public WifiMetrics_Tx Metrics_Tx { get; set; }
        public WifiMetrics_Rx Metrics_Rx { get; set; }
        public BtMetrics_Tx Metrics_BtTx { get; set; }
        public BtMetrics_Rx Metrics_BtRx { get; set; }

        public class WifiMetrics_Tx
        {
            public string EVM { get; set; }
            public string EVM_Range { get; set; }
            public string EVM_Result { get; set; }

            public string FreqError { get; set; }
            public string FreqError_Range { get; set; }
            public string FreqError_Result { get; set; }

            public string SymClkError { get; set; }
            public string SymClkError_Range { get; set; }
            public string SymClkError_Result { get; set; }

            public string Power { get; set; }
            public string Power_Range { get; set; }
            public string Power_Result { get; set; }

            public string LOLeakage { get; set; }
            public string LOLeakage_Range { get; set; }
            public string LOLeakage_Result { get; set; }

            public string TestTime { get; set; }
        }

        public class WifiMetrics_Rx
        {
            public string PER { get; set; }
            public string PER_Range { get; set; }
            public string PER_Result { get; set; }


            public string PER_Power { get; set; }


            public string RSSI { get; set; }
            public string RSSI_Range { get; set; }
            public string RSSI_Result { get; set; }


            public string TestTime { get; set; }
        }


        public class BtMetrics_Tx
        {
            public string FreqOffset { get; set; }
            public string FreqOffset_Range { get; set; }
            public string FreqOffset_Result { get; set; }


            public string Power { get; set; }
            public string Power_Range { get; set; }
            public string Power_Result { get; set; }


            public string TestTime { get; set; }
        }


        public class BtMetrics_Rx
        {
            public string PER { get; set; }
            public string PER_Range { get; set; }
            public string PER_Result { get; set; }


            public string PER_Power { get; set; }

            public string TestTime { get; set; }
        }


    }
}
