using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.InstrumentControl
{
    internal class GWINSTEK_PSS3203 : VisaDeviceBase, ITFPowerSupply
    {
        /*
         *IDN?			显示仪器版本
        :CHAN1:PROT:VOLT?	获取仪器最大电压
        :OUTP:STAT?		是否输出
        :CHAN1:PROT:CURR?	
        :CHAN1:VOLT?		获取设定电压
        :CHAN1:CURR?		获取设定电流
        :CHAN1:MEAS:VOLT?	获取输出电压
        :CHAN1:MEAS:CURR?	获取输出电流
        :SYST:ERR?		获取错误
        :CHAN1:CURR 2.58	                设定电流
        :CHAN1:VOLT 3.2		设定电压
        :OUTP:STAT 0		0不输出，1输出
         * */
        public override void OnConfiguration(VisaConfigurationBuilder configurationBuilders)
        {
            
        }

        public string str_DeviceAddr { get; set; }

        public GWINSTEK_PSS3203(string str_Addr) : base(str_Addr)
        {
            str_Addr = str_DeviceAddr;
        }



        public double GetCurrent()
        {
            string ret = Query(":CHAN1:MEAS:CURR?\r\n");
            if (!double.TryParse(ret, out double value))
            {
                UIHandleHelper.ShowRunLog("|FAIL|PowerSupply|PSS3203| get current failed!", true);
                return double.NaN;
            }
            return value;
        }

        public double GetVoltage()
        {
            string ret = Query(":CHAN1:MEAS:VOLT?\r\n");
            if (!double.TryParse(ret, out double value))
            {
                UIHandleHelper.ShowRunLog("|FAIL|PowerSupply|PSS3203| get voltage failed!", true);
                return double.NaN;
            }

            return value;
        }


        public bool PowerOff()
        {
            try
            {
                Write(":OUTP:STAT 0\r\n");
                UIHandleHelper.ShowRunLog("|INFO|PowerSupply|PSS3203| power off!");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                UIHandleHelper.ShowRunLog("|ERROR|PowerSupply|PSS3203| send power off failed!", true);
                return false;
            }
        }

        public bool PowerOn()
        {
            try
            {
                Write(":OUTP:STAT 1\r\n");
                UIHandleHelper.ShowRunLog("|INFO|PowerSupply|PSS3203| power on!");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                UIHandleHelper.ShowRunLog("|ERROR|PowerSupply|PSS3203| power on failed!", true);
                return false;
            }
        }

        public bool SetCurrent(double value)
        {
            try
            {
                Write($":CHAN1:CURR {value}\r\n");
                UIHandleHelper.ShowRunLog($"|INFO|PowerSupply|PSS3203| set current to {value}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                UIHandleHelper.ShowRunLog("|ERROR|PowerSupply|PSS3203| set current failed!", true);
                return false;
            }
        }

        public bool SetVoltage(double value)
        {
            try
            {
                if (value < 0)
                {
                    UIHandleHelper.ShowRunLog($"|FAIL|PowerSupply|PSS3203| can not set voltage to {value}", true);
                    return false;
                }

                if (value <= 33)
                {
                    Write($":CHAN1:VOLT {value}\r\n");
                }
                else
                {
                    UIHandleHelper.ShowRunLog("|INFO|PowerSupply|PSS3203| The maximum set voltage is 33V");
                    return false;
                }

                UIHandleHelper.ShowRunLog($"|INFO|PowerSupply|PSS3203| set voltage to {value}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                UIHandleHelper.ShowRunLog("|ERROR|PowerSupply|PSS3203| set voltage failed!", true);
                return false;
            }
        }
    }
}
