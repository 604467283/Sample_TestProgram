using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.InstrumentControl
{
    internal class VisaConfigurationBuilder
    {
        private readonly VisaDeviceBase _base;

        public VisaConfigurationBuilder(VisaDeviceBase @base)
        {
            _base = @base;
        }

        public VisaConfigurationBuilder SetTerminationCharacterEnable(bool enable = true)
        {
            if (_base.Session is not null)
            {
                _base.Session.TerminationCharacterEnabled = enable;
            }
            return this;
        }

        public VisaConfigurationBuilder SetTerminationCharacter(char character)
        {
            if (_base.Session is not null)
            {
                _base.Session.TerminationCharacter = (byte)character;
            }
            return this;
        }

        public VisaConfigurationBuilder SetTimeout(int millisecond)
        {
            if (_base.Session is not null)
            {
                _base.Session.TimeoutMilliseconds = millisecond;
            }
            return this;
        }

        public VisaConfigurationBuilder SetBufferSize(int size)
        {
            Array.Resize(ref _base.readBuffer, size);
            return this;
        }


    }
}
