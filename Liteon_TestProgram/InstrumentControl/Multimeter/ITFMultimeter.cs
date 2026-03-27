using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.InstrumentControl
{
    internal interface ITFMultimeter
    {

        public virtual bool SetToFrequencyMode() => throw new NotImplementedException();

        public virtual double GetFrequencyValue() => throw new NotImplementedException();

        public virtual bool SetToVoltageMode() => throw new NotImplementedException();

        public virtual double GetVoltageValue() => throw new NotImplementedException();

        public virtual bool SetToCurrentMode(string range = "") => throw new NotImplementedException();

        public virtual double GetCurrentValue() => throw new NotImplementedException();

        public virtual bool SetToResistanceMode() => throw new NotImplementedException();

        public virtual double GetResistanceValue() => throw new NotImplementedException();


    }
}
