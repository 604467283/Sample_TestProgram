using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class MacHelper
    {
        char[] strID;

        public String BTMacSet(String strMAC)
        {
            strID = strMAC.ToCharArray();
            incadd(strMAC.Length - 1);
            String BTMAC = new String(strID);
            return BTMAC;

        }
        public void incadd(int w)
        {
            if (w < 0)
                return;

            if ('F' == strID[w])
            {
                strID[w] = '0';
                incadd(w - 1);
            }

            else if ('9' == strID[w])
            {
                strID[w] = 'A';
            }
            else
            {
                strID[w]++;
            }
        }
    }
}
