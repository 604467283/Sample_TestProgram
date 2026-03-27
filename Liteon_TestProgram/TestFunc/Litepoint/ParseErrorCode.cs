using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Liteon_TestProgram.Base.TestErrorCode;

namespace Liteon_TestProgram.TestFunc.Litepoint
{
    internal class ParseErrorCode
    {
        // 辅助方法，检查测试是否失败
        private static bool IsTestFailed(string line, string testKeyword)
        {
            return line.Contains(testKeyword) && line.Contains("--- [Failed]");
        }

        // 解析文件并设置错误代码
        public static ErrorCode ParseFileForErrorCodes(string filePath)
        {
            ErrorCode errorCode = ErrorCode.Err000; // 默认无错误
            using (StreamReader file = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (IsTestFailed(line, "FREQ_ERROR_AVG"))
                    {
                        errorCode = ErrorCode.Err006;
                    }
                    else if (IsTestFailed(line, "POWER_AVG_DBM") || IsTestFailed(line, "POWER_AVERAGE_DBM"))
                    {
                        errorCode = ErrorCode.Err008;
                    }
                    else if (IsTestFailed(line, "MASK_MARGIN"))
                    {
                        errorCode = ErrorCode.Err007;
                    }
                    else if (IsTestFailed(line, "TX_CALIBRATION"))
                    {
                        errorCode = ErrorCode.Err004;
                    }
                    else if (IsTestFailed(line, "INSERT_DUT") || IsTestFailed(line, "INITIALIZE_DUT"))
                    {
                        errorCode = ErrorCode.Err002;
                    }
                    else if (IsTestFailed(line, "BER"))
                    {
                        errorCode = ErrorCode.Err015;
                    }
                    else if (IsTestFailed(line, "PER"))
                    {
                        errorCode = ErrorCode.Err009;
                    }
                    else if (IsTestFailed(line, "WRITE_MAC_ADDRESS") || IsTestFailed(line, "WRITE_BD_ADDRESS") || IsTestFailed(line, "FINALIZE_EEPROM"))
                    {
                        errorCode = ErrorCode.Err018;
                    }
                    else if (IsTestFailed(line, "Device is not connected"))
                    {
                        errorCode = ErrorCode.Err001;
                    }

                    // 可以继续添加更多的else if来检查其他错误
                }
            }

            UIHandleHelper.ShowRunLog($"ErrorCode: {errorCode}", true);
            return errorCode;
        }



    }
}
