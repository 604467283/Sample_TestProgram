using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class AdbProcess : CustomProcess
    {
        public AdbProcess(string programPath) : base(programPath)
        {
        }

        public bool WaitForDevice(int timeout = 999999)
        {
            return RunCommandLine("wait-for-device", out string output, timeout);
        }

        public string[] ListDevices()
        {
            string tag = "List of devices attached\r\n";
            var result = RunCommandLine("devices");
            int index = result.LastIndexOf(tag);
            result = result.Substring(index + tag.Length, result.Length - index - tag.Length).Trim();
            if (string.IsNullOrEmpty(result))
            {
                return Array.Empty<string>();
            }
            string[] devices = result.Split("\r\n");
            return devices;
        }

    }
}
