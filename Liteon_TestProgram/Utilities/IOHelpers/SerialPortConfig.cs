using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.IOHelpers
{
    internal class SerialPortConfig
    {

        private string _portName = "COM0";

        private int _baudRate = 115200;

        private int _dataBits = 8;

        private Parity _parity = Parity.None;

        private StopBits _stopBits = StopBits.One;

        private bool _rtsEnable = false;

        private bool _dtrEnable = false;

        public string PortName
        { get { return _portName; } set { _portName = value; } }
        public int BaudRate
        { get { return _baudRate; } set { _baudRate = value; } }
        public int DataBits
        { get { return _dataBits; } set { _dataBits = value; } }
        public Parity Parity
        { get { return _parity; } set { _parity = value; } }
        public StopBits StopBits
        { get { return _stopBits; } set { _stopBits = value; } }
        public bool RtsEnable
        { get { return _rtsEnable; } set { _rtsEnable = value; } }
        public bool DtrEnable
        { get { return _dtrEnable; } set { _dtrEnable = value; } }

    }
}
