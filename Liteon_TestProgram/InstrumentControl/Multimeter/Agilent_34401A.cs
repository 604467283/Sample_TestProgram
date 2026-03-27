using Liteon_TestProgram.Forms;
using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.InstrumentControl
{
    internal class Agilent_34401A : VisaDeviceBase, ITFMultimeter
    {
        public override void OnConfiguration(VisaConfigurationBuilder configurationBuilders)
        {
            configurationBuilders.SetTerminationCharacterEnable(true);
        }

        public string str_DeviceAddr { get;  set; }

        public Agilent_34401A(string str_Addr): base(str_Addr)
        {
            str_Addr = str_DeviceAddr;
        }

        double ITFMultimeter.GetFrequencyValue()
        {
            try
            {
                string result = Query("MEAS:FREQ?\n");

                if (string.IsNullOrEmpty(result))
                {
                    UIHandleHelper.ShowRunLog("|FAIL|Multimeter|34401A| query frequency has failed!", true);
                    return double.NaN;
                }
                result = result.Replace("\\n", "").Replace("\\r", "").Trim();
                if (!double.TryParse(result, out double value))
                {
                    UIHandleHelper.ShowRunLog($"|ERROR|Multimeter|34401A| parse frequency value has failed! device return :'{result}'", true);
                    return double.NaN;
                }
                return value;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|ERROR|Multimeter|34401A| try get frequency has failed! cause: {ex.Message}", true);
                return double.NaN;
            }
        }

        bool ITFMultimeter.SetToFrequencyMode()
        {
            try
            {
                Write("CONF:FREQ \n");
                UIHandleHelper.ShowRunLog($"|INFO|Multimeter|34401A| set mode to frequency");
                return true;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|ERROR|Multimeter|34401A| try set frequency mode has failed;cause: " + ex.Message, true);
                return false;
            }
        }
        double ITFMultimeter.GetCurrentValue()
        {
            try
            {
                Write("SAMP:COUN 1\n");
                string result = Query("READ?\n");

                if (string.IsNullOrEmpty(result))
                {
                    UIHandleHelper.ShowRunLog($"|FAIL|Multimeter|34401A| query current has failed!", true);
                    return double.NaN;
                }
                result = result.Replace("\\n", "").Replace("\\r", "").Trim();
                if (!double.TryParse(result, out double value))
                {
                    UIHandleHelper.ShowRunLog($"|FAIL|Multimeter|34401A| parse current value has failed! device return :'{result}'", true);
                    return double.NaN;
                }
                return value;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|ERROR|Multimeter|34401A| try get current has failed! cause: {ex.Message}", true);
                return double.NaN;
            }
        }

        bool ITFMultimeter.SetToCurrentMode(string range)
        {
            try
            {
                if (range == "")
                    Write("CONF:CURR:DC\n");
                else
                    Write($"CONF:CURR:DC {range}\n");

                UIHandleHelper.ShowRunLog($"|INFO|Multimeter|34401A| set mode to current");
                return true;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|ERROR|Multimeter|34401A| try set current mode has failed; cause: {ex.Message}", true);
                return false;
            }
        }


        double ITFMultimeter.GetResistanceValue()
        {
            try
            {
                string result = Query("MEAS:RES?\n");

                if (string.IsNullOrEmpty(result))
                {
                    UIHandleHelper.ShowRunLog($"|FAIL|Multimeter|34401A| query RESistance has failed!", true);
                    return double.NaN;
                }
                result = result.Replace("\\n", "").Replace("\\r", "").Trim();
                if (!double.TryParse(result, out double value))
                {
                    UIHandleHelper.ShowRunLog($"|FAIL|Multimeter|34401A|  parse RESistance value has failed! device return :'{result}'", true);
                    return double.NaN;
                }
                return value;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|ERROR|Multimeter|34401A| try get RESistance has failed! cause:  {ex.Message}", true);
                return double.NaN;
            }
        }

        bool ITFMultimeter.SetToResistanceMode()
        {
            try
            {
                Write("CONF:RES\n");
                UIHandleHelper.ShowRunLog($"|INFO|Multimeter|34401A| set mode to RESistance");
                return true;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|ERROR|Multimeter|34401A| try set RESistance mode has failed;cause:  {ex.Message}", true);
                return false;
            }
        }

        bool ITFMultimeter.SetToVoltageMode()
        {
            try
            {
                Write("CONF:VOLT\n");
                UIHandleHelper.ShowRunLog($"|INFO|Multimeter|34401A| set mode to voltage");
                return true;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|ERROR|Multimeter|34401A| try set voltage mode has failed;cause: {ex.Message}", true);
                return false;
            }
        }

        double ITFMultimeter.GetVoltageValue()
        {
            try
            {
                string result = Query("MEAS:VOLT?\n");

                if (string.IsNullOrEmpty(result))
                {
                    UIHandleHelper.ShowRunLog($"|FAIL|Multimeter|34401A| query voltage has failed!", true);
                    return double.NaN;
                }
                result = result.Replace("\\n", "").Replace("\\r", "").Trim();
                if (!double.TryParse(result, out double value))
                {
                    UIHandleHelper.ShowRunLog($"|FAIL|Multimeter|34401A| parse voltage value has failed! device return :'{result}'", true);
                    return double.NaN;
                }
                return value;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"|ERROR|Multimeter|34401A| try get voltage has failed! cause: {ex.Message}", true);
                return double.NaN;
            }
        }




    }
}
