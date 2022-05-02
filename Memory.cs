using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeChangeMonitor
{
    class Memory
    {
        const int PROCESS_VM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008;

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, long lpBaseAddress, [In, Out] byte[] lpBuffer, int nsize, out IntPtr lpNumberOfBytesRead);
        public static byte[] ReadBytes(Int32 pid, long basePtr, int size)
        {
            byte[] buffer = new byte[size];
            IntPtr processHandle = OpenProcess(PROCESS_VM_READ | PROCESS_VM_WRITE | PROCESS_VM_OPERATION, false, pid);
            ReadProcessMemory(processHandle, basePtr, buffer, size, out _);
            CloseHandle(processHandle);
            return buffer;
        }

        public static bool IsValid(long Address)
        {
            return (Address >= 0x10000 && Address < 0x000F000000000000);
        }
    }
}
