using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class HttpHelper
    {
        public bool SendData(Socket Client, String SendMsg)
        {
            if (Client != null)
            {

                try
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(SendMsg);
                    Client.Send(buffer);
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            return false;
        }



    }
}
