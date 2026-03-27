using Liteon_TestProgram.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class TestCountHelper
    {
        public bool CheckTestCount(bool Badd)
        {
            try
            {

                String strFilePath = "D:\\TestCount\\TestCount.ini";
                if (!Directory.Exists("D:\\TestCount"))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo("D:\\TestCount");
                    directoryInfo.Create();
                }
                if (!File.Exists(strFilePath))
                {
                    String strContent = "[Config]\r\nTestAntennaTime=0\r\nTestAntennaTotal=10000\r\nTestCardTime=0\r\nTestCardTotal=10000";
                    File.WriteAllText(strFilePath, strContent);
                }
                StringBuilder ValTemp = new StringBuilder();
                IniHelper.GetIniStr("Config", "TestAntennaTime", "0", ValTemp, 10, strFilePath);
                int TestAntennaTime = int.Parse(ValTemp.ToString());
                IniHelper.GetIniStr("Config", "TestAntennaTotal", "10000", ValTemp, 10, strFilePath);
                int TestAntennaTotal = int.Parse(ValTemp.ToString());
                IniHelper.GetIniStr("Config", "TestCardTime", "0", ValTemp, 10, strFilePath);
                int TestCardTime = int.Parse(ValTemp.ToString());
                IniHelper.GetIniStr("Config", "TestCardTotal", "10000", ValTemp, 10, strFilePath);
                int TestCardTotal = int.Parse(ValTemp.ToString());
                if (Badd)
                {
                    TestAntennaTime++;
                    TestCardTime++;
                }
                IniHelper.WriteIniStr("Config", "TestAntennaTime", TestAntennaTime.ToString(), strFilePath);
                IniHelper.WriteIniStr("Config", "TestCardTime", TestCardTime.ToString(), strFilePath);
                if (TestAntennaTime >= TestAntennaTotal)
                {
                    MessageBoxEX.Show($"测试次数已经超过{TestAntennaTotal}次，请联系PTE人员更换测试针", true);
                    return false;
                }
                if (TestCardTime >= TestCardTotal)
                {
                    MessageBoxEX.Show($"测试次数已经超过{TestCardTotal}次，请联系PTE人员更换转接卡", true);
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                MessageBoxEX.Show(ex.Message, true);
                return false;
            }
            return true;
        }
    }
}
