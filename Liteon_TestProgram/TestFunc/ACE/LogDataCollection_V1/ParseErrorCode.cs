using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Liteon_TestProgram.Base.TestErrorCode;

namespace Liteon_TestProgram.TestFunc.ACE
{
    internal class ParseErrorCode
    {
        // 辅助方法，检查测试是否失败
        private static bool IsTestFailed(string line, string testKeyword)
        {
            //  Power               17.832 dBm          (20.5 ~ 15.5)       <-- pass
            return line.Contains(testKeyword) && line.Contains("<-- fail");
        }

        // 解析文件并设置错误代码
        public static ErrorCode ParseFileForErrorCodes(string filePath)
        {
            ErrorCode errorCode = ErrorCode.Err000; // 默认
            using (StreamReader file = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (IsTestFailed(line, "Freq Error"))
                    {
                        errorCode = ErrorCode.Err006;
                    }
                    else if (IsTestFailed(line, "Power") )
                    {
                        errorCode = ErrorCode.Err008;
                    }
                    else if (IsTestFailed(line, "Spectrum Mask"))
                    {
                        errorCode = ErrorCode.Err007;
                    }
                    else if (IsTestFailed(line, "FreqErr") || IsTestFailed(line, "Init Freq Err"))
                    {
                        errorCode = ErrorCode.Err006;
                    }
                    else if (IsTestFailed(line, "Calibration"))
                    {
                        errorCode = ErrorCode.Err004;
                    }
                    else if (IsTestFailed(line, "WT_INSERT_DUT"))
                    {
                        errorCode = ErrorCode.Err002;
                    }
                    else if (IsTestFailed(line, "PER"))
                    {
                        errorCode = ErrorCode.Err009;
                    }
                    else if (IsTestFailed(line, "BER"))
                    {
                        errorCode = ErrorCode.Err009;
                    }
                    else if (IsTestFailed(line, "SET_MAC_ADDRESS") || IsTestFailed(line, "SAVE_CAL_DATA"))
                    {
                        errorCode = ErrorCode.Err018;
                    }
                    else if (IsTestFailed(line, "Device is not connected"))
                    {
                        errorCode = ErrorCode.Err001;
                    }
                    else
                    {
                        errorCode = ErrorCode.Err000;
                    }
                    // 可以继续添加更多的else if来检查其他错误
                }
            }

            UIHandleHelper.ShowRunLog($"ErrorCode: {errorCode}", true);
            return errorCode;
        }



    }
}
