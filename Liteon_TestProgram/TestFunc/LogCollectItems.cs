using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.TestFunc
{
    internal class LogCollectItems
    {
        public class TestConfiguration
        {
            // 通用配置
            public bool Standard { get; set; } = true;
            public bool Result { get; set; } = true;

            // WIFI 配置组
            public WifiConfig Wifi { get; set; } = new WifiConfig();
            public BTConfig Bluetooth { get; set; } = new BTConfig();
            public ICTConfig ICT { get; set; } = new ICTConfig();
        }

        public class WifiConfig
        {
            // 发射测试
            public bool EVM { get; set; } = true;
            public bool FreqError { get; set; } = true;
            public bool SymClkError { get; set; } = true;
            public bool Power { get; set; } = true;
            public bool LOLeakage { get; set; } = true;
            public bool SpectrumMask { get; set; } = true;

            // 接收测试
            public bool RxPower { get; set; } = true;
            public bool RxPer { get; set; } = true;
            public bool RxRssi { get; set; } = true;
        }

        public class BTConfig
        {
            // 发射测试
            public bool FreqOffset { get; set; } = true;
            public bool Power { get; set; } = true;
            public bool InitFreqErr { get; set; } = true;

            // 接收测试
            public bool RxPower { get; set; } = true;
            public bool RxPer { get; set; } = true;
        }

        public class ICTConfig
        {
            public bool Value { get; set; } = true;
            public bool Unit { get; set; } = true;
        }
    }
}
