using Liteon_TestProgram.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.PEM
{

    internal class PEMHelper
    {

        private const string LibraryPath = "TestDll/TestDll_Old/MyPEM.dll";

        [DllImport(LibraryPath)]
        public static extern int InitPEM();
        [DllImport(LibraryPath)]
        public static extern int CreateBAT(string PathB);
        [DllImport(LibraryPath)]
        public static extern int Open_PEM(int S);


        public int Init_PEM()
        {
            return InitPEM();
        }

        public bool OpenPEM(bool OnOFF)
        {
            if (OnOFF)
            {
                Open_PEM(1);

            }
            else
            {
                Open_PEM(0);
            }
            Thread.Sleep(200);
            return OpenBat(".\\rescan.bat");
        }


        public bool OpenBat(string BatPath)
        {
            //             CreateBAT(BatPath);
            try
            {
                if (CreateBAT(BatPath) == 1)
                {
                    Thread.Sleep(100);
                    return true;
                }
                else
                    return false;
                //                 Process proc = Process.Start(BatPath);
                //                 if (proc != null)
                //                 {
                //                     proc.WaitForExit();
                //                     return true;
                //                 }
            }
            catch (Exception ex)
            {
                MessageBoxEX.Show(ex.ToString(), true);
                return false;
            }
            //return false;
        }


    }
}
