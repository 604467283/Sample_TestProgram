using Liteon_TestProgram.Forms;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Liteon_TestProgram
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            [DllImport("kernel32.dll")]
            static extern bool AllocConsole();

            [DllImport("kernel32.dll")]
            static extern bool FreeConsole();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            //Application.Run(new MainForm());

            if (args.Any(x => x == "--debug"))
            {
                AllocConsole();
            }


            Application.Run(new CaseForm());

        }
    }
}