using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class ANSIHandler
    {
        private enum ANSI_STATE
        {
            NORMAL,
            ESCAPE,
            ANSI_SEQUENCE,
        }

        private ANSI_STATE _state = ANSI_STATE.NORMAL;

        public char? StripANSI(char singleChar)
        {
            switch (_state)
            {
                case ANSI_STATE.NORMAL:
                    if (singleChar == '\x1B')
                    {
                        _state = ANSI_STATE.ESCAPE;
                    }
                    else
                    {
                        return singleChar;
                    }
                    break;
                case ANSI_STATE.ESCAPE:
                    if (singleChar == '[')
                    {
                        _state = ANSI_STATE.ANSI_SEQUENCE;
                    }
                    else if (singleChar == '\x1B')
                    {
                        //continue;
                    }
                    else
                    {
                        _state = ANSI_STATE.NORMAL;
                    }
                    break;
                case ANSI_STATE.ANSI_SEQUENCE:
                    if (char.IsLetter(singleChar))
                    {
                        _state = ANSI_STATE.NORMAL;
                    }
                    break;
            }

            return null;
        }
    }
}
