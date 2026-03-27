using Rockmong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.GPIOHelpers.RockMong
{
    internal class RockMongSimpleGpioController : IDisposable
    {
        private readonly int _deviceSerialNumber;
        private bool _initialized = false;
        private const int TotalPins = 32;

        /// <summary>
        /// Pull：上拉下拉电阻。0，无。1，使能内部上拉。2，使能内部下拉 
        /// </summary>
        /// <param name="iPull"></param>
        /// <param name="serialNumber"></param>
        /// <exception cref="Exception"></exception>
        public RockMongSimpleGpioController(int iPull, int? serialNumber = null)
        {
            //1.   //扫描 USB 设备，获取设备序列号列表。 
           //2.   //返回值如果大于 0，代表获取到设备的个数。如果等于 0，代表未插入设备。如果小于 0，代表发生错误
            int[] devices = ScanDevices();
            if (devices.Length == 0) throw new Exception("未找到任何设备");
            _deviceSerialNumber = serialNumber ?? devices[0];
            InitializeAllPins(iPull);
            _initialized = true;
        }

        public bool ReadPin(int pin)
        {
            CheckInitialized();
            ValidatePinNumber(pin);

            var result = ReadPinsInternal(new[] { pin });
            return result[pin] != 0;  // 修正为使用pin参数而不是pins变量
        }

        //public Dictionary<int, bool> ReadMultiplePins(int[] pins)
        //{
        //    CheckInitialized();
        //    if (pins == null || pins.Length == 0)
        //        throw new ArgumentException("引脚数组不能为空");

        //    foreach (var pin in pins) ValidatePinNumber(pin);

        //    var result = ReadPinsInternal(pins);
        //    return pins.ToDictionary(p => p, p => result[p] != 0);
        //}


        /// <summary>
        /// 读取多个引脚状态
        /// </summary>
        /// <param name="pins">要读取的引脚号数组</param>
        /// <returns>bool数组，每个元素对应输入引脚的状态(true=高电平，false=低电平)</returns>
        public bool[] ReadMultiplePins(int[] pins)
        {
            CheckInitialized();
            if (pins == null || pins.Length == 0)
                throw new ArgumentException("引脚数组不能为空");

            foreach (var pin in pins) ValidatePinNumber(pin);

            var result = ReadPinsInternal(pins);
            return pins.Select(p => result[p] != 0).ToArray();
        }




        public bool[] ReadAllPins()
        {
            CheckInitialized();

            var readTx = new io.IO_Read_TxStruct[TotalPins];
            for (int i = 0; i < TotalPins; i++)
            {
                readTx[i] = new io.IO_Read_TxStruct { Pin = (byte)i };
            }

            var readRx = new io.IO_Read_RxStruct[TotalPins];
            int ret = io.IO_ReadMultiPin(_deviceSerialNumber, readTx, readRx, TotalPins);

            if (ret < 0) throw new Exception($"读取错误: {ret}");

            return readRx.Select(r => r.PinState != 0).ToArray();
        }

        public int DeviceSerialNumber => _deviceSerialNumber;

        public void Dispose() => _initialized = false;



        // ========== 私有方法 ==========

        private int[] ScanDevices()
        {
            int[] serialNumbers = new int[16];
            int count = usb_device.UsbDevice_Scan(serialNumbers);
            return count <= 0 ? Array.Empty<int>() : serialNumbers[0..count];
        }

        /// <summary>
        //Pull：上拉下拉电阻。0，无。1，使能内部上拉。2，使能内部下拉 
        /// </summary>
        /// <param name="iPull"></param>
        /// <exception cref="Exception"></exception>
        private void InitializeAllPins(int iPull)
        {
            /*
                //Pin：引脚编号。0，P0. 1, P1... 
                //Mode：输入输出模式。0，输入。1，输出。2，开漏 
                //Pull：上拉下拉电阻。0，无。1，使能内部上拉。2，使能内部下拉 
             */
            var initTx = new io.IO_Init_TxStruct[TotalPins];
            for (int i = 0; i < TotalPins; i++)
            {
                initTx[i] = new io.IO_Init_TxStruct
                {
                    Pin = (byte)i,
                    Mode = 0,
                    Pull = (byte)iPull
                };
            }

            var initRx = new io.IO_Init_RxStruct[TotalPins];
            int ret = io.IO_InitMultiPin(_deviceSerialNumber, initTx, initRx, TotalPins);
            if (ret < 0) throw new Exception($"初始化错误: {ret}");
        }

        private Dictionary<int, int> ReadPinsInternal(int[] pins)
        {
            var readTx = new io.IO_Read_TxStruct[pins.Length];
            for (int i = 0; i < pins.Length; i++)
            {
                readTx[i] = new io.IO_Read_TxStruct { Pin = (byte)pins[i] };
            }

            var readRx = new io.IO_Read_RxStruct[pins.Length];
            int ret = io.IO_ReadMultiPin(_deviceSerialNumber, readTx, readRx, pins.Length);
            if (ret < 0) throw new Exception($"读取错误: {ret}");

            return pins.ToDictionary(
                p => p,
                p => (int)readRx[Array.IndexOf(pins, p)].PinState
            );
        }

        private void ValidatePinNumber(int pin)
        {
            if (pin < 0 || pin >= TotalPins)
                throw new ArgumentOutOfRangeException(nameof(pin), $"引脚号必须在0-{TotalPins - 1}之间");
        }

        private void CheckInitialized()
        {
            if (!_initialized) throw new Exception("GPIO控制器未初始化");
        }
    }



}
