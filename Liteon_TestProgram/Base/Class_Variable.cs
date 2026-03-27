using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Base
{
    public static class Class_Variable
    {
        public struct struct_Test_Variable
        {
            public bool bNeedCreateSFCFile;
            public string str_LogDataCollectionType;

            /// <summary>
            /// //截取后的长度
            /// </summary>
            public int iMACLength;                  //截取后的长度
            /// <summary>
            /// //截取后的长度
            /// </summary>
            public int iBDLength;
            /// <summary>
            /// //截取后的长度
            /// </summary>
            public int iSNLength;
            /// <summary>
            /// //截取的开始位(包含)
            /// </summary>
            public int iMAC_ExtractStartPosition;  //截取的开始位
            /// <summary>
            /// //截取的开始位(包含)
            /// </summary>
            public int iBD_ExtractStartPosition;
            /// <summary>
            /// //截取的开始位(包含)
            /// </summary>
            public int iSN_ExtractStartPosition;
            /// <summary>
            /// //原始长度
            /// </summary>
            public int iMAC_OriginalLength;  //原始长度
            /// <summary>
            /// //原始长度
            /// </summary>
            public int iBD_OriginalLength;
            /// <summary>
            /// //原始长度
            /// </summary>
            public int iSN_OriginalLength;

            public string str_ErrorCode;
            public List<float> floatSampleIniPowerList;
            public float floatSamplePowerRange;
            public string PC_HardDeskSN;
            public string PC_MainboardSN;
        }

        public struct struct_NormalINI_Variable
        {
            //[Mac_Six]
            public bool stru_b_CheckMacSix_Switch;
            public string stru_str_CheckMacID1;      
            public string stru_str_CheckMacID2;      
            public string stru_str_CheckMacID3;


            //[ShieldingBox]
            public int stru_i_ShieldingBoxCOM;
            public int stru_i_SleepCycleWhenOpen;
            public int stru_i_SleepCycleWhenClose;


            //[Path]
            public int stru_i_FreeSpaceLimit;
            public string stru_str_SFCFilePath;
            public string stru_str_LogFilePath;

            //[TCP_IP]
            public int stru_i_TestCompNum;
            public string stru_str_RobotClientHostIP;
            public string stru_str_RobotClientHostPort;

            //[Multi_DUT]
            public int stru_i_SelectMode;
            public bool stru_b_Multi_Switch;
            public string stru_str_Multi_Server_IP;
            public string stru_str_Multi_Server_Port;

            //[PEM]
            public bool stru_b_IsOpenPEM;
            public bool stru_b_IsOpenDUT;
            public string stru_str_DeviceName1;
            public string stru_str_DeviceName2;

            //[TesterPort]
            public string stru_str_TesterPort;

            //[ComPort_1]
            public string stru_str_PortName_1;
            public int stru_i_BaudRate_1;
            public int stru_i_DataBits_1;         
            public StopBits stru_StopBits_1;
            public Parity stru_Parity_1;
            public bool stru_b_RtsEnable_1;
            public bool stru_b_DtrEnable_1;

            //[ComPort_2]
            public string stru_str_PortName_2;
            public int stru_i_BaudRate_2;
            public int stru_i_DataBits_2;
            public StopBits stru_StopBits_2;
            public Parity stru_Parity_2;
            public bool stru_b_RtsEnable_2;
            public bool stru_b_DtrEnable_2;

            //[ComPort_3]
            public string stru_str_PortName_3;
            public int stru_i_BaudRate_3;
            public int stru_i_DataBits_3;
            public StopBits stru_StopBits_3;
            public Parity stru_Parity_3;
            public bool stru_b_RtsEnable_3;
            public bool stru_b_DtrEnable_3;

            //[ComPort_4]
            public string stru_str_PortName_4;
            public int stru_i_BaudRate_4;
            public int stru_i_DataBits_4;
            public StopBits stru_StopBits_4;
            public Parity stru_Parity_4;
            public bool stru_b_RtsEnable_4;
            public bool stru_b_DtrEnable_4;

            //[ComPort_5]
            public string stru_str_PortName_5;
            public int stru_i_BaudRate_5;
            public int stru_i_DataBits_5;
            public StopBits stru_StopBits_5;
            public Parity stru_Parity_5;
            public bool stru_b_RtsEnable_5;
            public bool stru_b_DtrEnable_5;

            //[ComPort_6]
            public string stru_str_PortName_6;
            public int stru_i_BaudRate_6;
            public int stru_i_DataBits_6;
            public StopBits stru_StopBits_6;
            public Parity stru_Parity_6;
            public bool stru_b_RtsEnable_6;
            public bool stru_b_DtrEnable_6;

            //[InstrumentControl]
            public string stru_str_InstrumentName_1;
            public string stru_str_InstrumentName_2;
            public string stru_str_InstrumentName_3;
            public string stru_str_InstrumentAddr_1;
            public string stru_str_InstrumentAddr_2;
            public string stru_str_InstrumentAddr_3;

            //[Mode]
            public bool stru_b_DebugMode;

            //[ServerLog]
            public bool stru_b_LogServerSwitch;
            public string stru_str_LogServerUser;
            public string stru_str_LogServerPassword;
            public string stru_str_LogServerPath;

            //[FTP]
            public bool stru_b_FTPSwitch;
            public string stru_str_FTP_IP;
            public int stru_i_FTP_Port;
            public string stru_str_FTP_User;
            public string stru_str_FTP_Password;
            public string stru_str_FTP_Path;

            //TestTimes
            public int stru_i_TestTimes_Fail;
            public int stru_i_TestTimes_Pass;


        }

        public struct struct_EncryptINI_Variable
        {
            //[Model]
            public string stru_str_ProjectName;
            public string stru_str_CaseVersion;
            public string stru_str_SFCNumber;


            //[MD5_INFO]
            public string stru_str_MD5_INFO;

        }

        public struct struct_Barcode_Variable
        {
            public string stru_str_sRevDUTSN;
            public string stru_str_sRevDUTMac;
            public string stru_str_sRevDUTBD;
        }

        public enum MacBD_Relation
        {
            None,
            OnlyMac,
            PlusOne,
            Separate,
        }

        public enum SN_Relation
        {
            None,
            SN, 
        }



    }
}
