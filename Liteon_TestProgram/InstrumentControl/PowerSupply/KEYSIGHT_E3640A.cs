using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.InstrumentControl
{
    internal class KEYSIGHT_E3640A :VisaDeviceBase, ITFPowerSupply
    {
        public string str_DeviceAddr { get; set; }

        public KEYSIGHT_E3640A(string str_Addr) : base(str_Addr)
        {
            str_Addr = str_DeviceAddr;
        }

        public double GetCurrent()
        {
            string ret = Query("MEAS:CURR?\r\n");
            if (!double.TryParse(ret, out double value))
            {
                UIHandleHelper.ShowRunLog("|FAIL|PowerSupply|E3640A| get current failed!", true);
                return double.NaN;
            }
            return value;
        }

        public double GetVoltage()
        {
            string ret = Query("MEAS:VOLT?\r\n");
            if (!double.TryParse(ret, out double value))
            {
                UIHandleHelper.ShowRunLog("|FAIL|PowerSupply|E3640A| get voltage failed!", true);
                return double.NaN;
            }

            return value;
        }

        public override void OnConfiguration(VisaConfigurationBuilder configurationBuilders)
        {
        }

        public bool PowerOff()
        {
            try
            {
                Write("OUTP OFF\r\n");
                UIHandleHelper.ShowRunLog("|INFO|PowerSupply|E3640A| power off!");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                UIHandleHelper.ShowRunLog("|ERROR|PowerSupply|E3640A| send power off failed!", true);
                return false;
            }
        }

        public bool PowerOn()
        {
            try
            {
                Write("OUTP ON\r\n");
                UIHandleHelper.ShowRunLog("|INFO|PowerSupply|E3640A| power on!");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                UIHandleHelper.ShowRunLog("|ERROR|PowerSupply|E3640A| power on failed!", true);
                return false;
            }
        }

        public bool SetCurrent(double value)
        {
            try
            {
                Write($"CURR {value}\r\n");
                UIHandleHelper.ShowRunLog($"|INFO|PowerSupply|E3640A| set current to {value}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                UIHandleHelper.ShowRunLog("|ERROR|PowerSupply|E3640A| set current failed!", true);
                return false;
            }
        }

        public bool SetVoltage(double value)
        {
            try
            {
                if (value < 0)
                {
                    UIHandleHelper.ShowRunLog($"|FAIL|PowerSupply|E3640A| can not set voltage to {value}", true);
                    return false;
                }

                if (value <= 8)
                {
                    Write($"VOLT {value}\r\n");

                }
                else
                {
                    //Write($"VOLT 3\r\n");
                    Write($"VOLT: RANG P20V\r\n");
                    Write($"VOLT {value}\r\n");
                }

                UIHandleHelper.ShowRunLog($"|INFO|PowerSupply|E3640A| set voltage to {value}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                UIHandleHelper.ShowRunLog("|ERROR|PowerSupply|E3640A| set voltage failed!", true);
                return false;
            }
        }


    }
}
