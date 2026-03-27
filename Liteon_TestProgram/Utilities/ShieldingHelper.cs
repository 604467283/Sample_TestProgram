using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class ShieldingHelper
    {
        SerialPort Com = new SerialPort();
        String shopen;
        bool OnOFFSh;

        public bool InitShieldingCom(String port)
        {
            try
            {
                String portsNames = "COM" + port;
                if (!Com.IsOpen)
                {
                    Com.PortName = portsNames;
                    Com.BaudRate = 9600;
                    Com.DataBits = 8;
                    Com.Parity = Parity.None;
                    Com.StopBits = StopBits.One;

                    Com.Open();
                    Com.DataReceived += new SerialDataReceivedEventHandler(this.Com_DataReceived);
                }

            }
            catch
            {
                return false;
            }
            return true;
        }


        void Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            byte[] readBuffer = new byte[Com.ReadBufferSize + 1];
            try
            {
                int count = Com.Read(readBuffer, 0, Com.ReadBufferSize);

                String SerialIn = System.Text.Encoding.ASCII.GetString(readBuffer, 0, count);
                shopen += SerialIn;
                if (shopen.IndexOf("ok") >= 0 || shopen.IndexOf("OK") >= 0)
                {
                    OnOFFSh = true;
                }
                else if (shopen.IndexOf("ready") >= 0 || shopen.IndexOf("REDAY") >= 0)
                {
                    OnOFFSh = true;
                }
                else
                {
                    OnOFFSh = false;
                }
                Thread.Sleep(500);
            }
            catch (Exception ex) 
            {
                UIHandleHelper.ShowRunLog($"Shielding recv error: {ex}");
            }
        }

        public bool OpenShieldingBox(int SleepTime)
        {
            OnOFFSh = false;
            shopen = "";
            Com.Write("open\r\n");
            int n = 0;
            while (OnOFFSh == false)
            {
                Thread.Sleep(1000);
                n++;
                if (n > SleepTime / 1000)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CloseShieldingBox(int SleepTime)
        {
            OnOFFSh = false;
            shopen = "";
            Com.Write("close\r\n");
            int n = 0;
            while (OnOFFSh == false)
            {
                Thread.Sleep(1000);
                n++;
                if (n > SleepTime / 1000)
                {
                    return false;
                }
            }

            return true;
        }



    }
}
