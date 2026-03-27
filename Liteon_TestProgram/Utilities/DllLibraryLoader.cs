using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class DllLibraryLoader
    {
        // 目的：
        // 动态加载与卸载非托管 DLL
        // 当用DllImport属性导入一个DLL时，默认情况下，这个DLL会被加载到应用程序域中，
        // 而且一旦加载，通常在整个应用程序运行期间都不会被卸载。
        // 不过，用户可能遇到了需要动态加载和卸载的情况，比如插件系统，这时候可能需要手动管理DLL的生命周期


        // 导入 Win32 API  
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        private IntPtr _hModule;

        // 加载 DLL
        public void Load(string dllPath)
        {
            _hModule = LoadLibrary(dllPath);
            if (_hModule == IntPtr.Zero)
            {
                throw new DllNotFoundException($"无法加载 DLL: {dllPath}。错误代码: {Marshal.GetLastWin32Error()}");
            }
        }

        // 获取函数委托
        public T GetFunction<T>(string functionName) where T : Delegate
        {
            IntPtr procAddress = GetProcAddress(_hModule, functionName);
            if (procAddress == IntPtr.Zero)
            {
                throw new EntryPointNotFoundException($"未找到函数: {functionName}。错误代码: {Marshal.GetLastWin32Error()}");
            }
            return Marshal.GetDelegateForFunctionPointer<T>(procAddress);
        }

        // 卸载 DLL
        public void Unload()
        {
            if (_hModule != IntPtr.Zero)
            {
                FreeLibrary(_hModule);
                _hModule = IntPtr.Zero;
            }
        }
    }
}
