using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.InstrumentControl
{
    internal interface ITFPowerSupply
    {
        public bool SetVoltage(double voltage);

        public double GetVoltage();

        public bool SetCurrent(double current);

        public double GetCurrent();

        public bool PowerOn();

        public bool PowerOff();
    }
}
