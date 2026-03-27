using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Liteon_TestProgram.Base.TestErrorCode;

namespace Liteon_TestProgram.TestFunc.iTest
{
    internal class ParseErrorCode
    {
        // 辅助方法，检查测试是否失败
        private static bool IsTestFailed(string line, string testKeyword)
        {
            //   TX_[0] 65.0M MaskErr      : 6.44   %    (   0.00 .................X    5.12) FAIL
            return line.Contains(testKeyword) && line.Contains("FAIL");
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
                    if (IsTestFailed(line, "FreqErr"))
                    {
                        errorCode = ErrorCode.Err006;
                    }
                    else if (IsTestFailed(line, "Power[") )
                    {
                        errorCode = ErrorCode.Err008;
                    }
                    else if (IsTestFailed(line, "MaskErr"))
                    {
                        errorCode = ErrorCode.Err007;
                    }
                    else if (IsTestFailed(line, "FreqErr") || IsTestFailed(line, "Init Freq Err"))
                    {
                        errorCode = ErrorCode.Err006;
                    }
                    else if (IsTestFailed(line, "_CAL_"))
                    {
                        errorCode = ErrorCode.Err004;
                    }
                    else if (IsTestFailed(line, "WT_INSERT_DUT"))
                    {
                        errorCode = ErrorCode.Err002;
                    }
                    else if (IsTestFailed(line.Replace(" ", ""), "Power:"))
                    {
                        errorCode = ErrorCode.Err014;
                    }
                    else if (IsTestFailed(line, "BT_RX_BER"))
                    {
                        errorCode = ErrorCode.Err015;
                    }
                    else if (IsTestFailed(line, "BT_RX_PER"))
                    {
                        errorCode = ErrorCode.Err016;
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
