using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AtelierRyza2Trainer.Core
{
    /// <summary>
    /// Manages memory reading and writing for the Atelier Ryza 2 process.
    /// Provides low-level access to game memory using Win32 API.
    /// </summary>
    public class MemoryManager : IDisposable
    {
        private IntPtr _processHandle;
        private readonly string _processName;
        private bool _disposed = false;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        private const uint PROCESS_VM_READ = 0x0010;
        private const uint PROCESS_VM_WRITE = 0x0020;
        private const uint PROCESS_VM_OPERATION = 0x0008;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryManager"/> class.
        /// </summary>
        /// <param name="processName">The name of the game process (without extension).</param>
        /// <exception cref="Exception">Thrown when the process cannot be found or opened.</exception>
        public MemoryManager(string processName)
        {
            _processName = processName;
            var processes = Process.GetProcessesByName(_processName);
            if (processes.Length == 0)
            {
                throw new Exception($"Process '{_processName}' not found. Make sure the game is running.");
            }

            var process = processes[0];
            _processHandle = OpenProcess(PROCESS_VM_READ | PROCESS_VM_WRITE | PROCESS_VM_OPERATION, false, process.Id);
            if (_processHandle == IntPtr.Zero)
            {
                throw new Exception("Failed to open process. Try running the trainer as administrator.");
            }
        }

        /// <summary>
        /// Reads memory from the game process at the specified address.
        /// </summary>
        /// <param name="address">The memory address to read from.</param>
        /// <param name="size">The number of bytes to read.</param>
        /// <returns>A byte array containing the read data.</returns>
        public byte[] ReadMemory(IntPtr address, int size)
        {
            byte[] buffer = new byte[size];
            if (!ReadProcessMemory(_processHandle, address, buffer, size, out int bytesRead))
            {
                throw new Exception($"Failed to read memory at 0x{address.ToString("X")}. Error code: {Marshal.GetLastWin32Error()}");
            }
            return buffer;
        }

        /// <summary>
        /// Writes memory to the game process at the specified address.
        /// </summary>
        /// <param name="address">The memory address to write to.</param>
        /// <param name="data">The byte array to write.</param>
        public void WriteMemory(IntPtr address, byte[] data)
        {
            if (!WriteProcessMemory(_processHandle, address, data, data.Length, out int bytesWritten))
            {
                throw new Exception($"Failed to write memory at 0x{address.ToString("X")}. Error code: {Marshal.GetLastWin32Error()}");
            }
        }

        /// <summary>
        /// Disposes the memory manager by closing the process handle.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed && _processHandle != IntPtr.Zero)
            {
                CloseHandle(_processHandle);
                _processHandle = IntPtr.Zero;
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        ~MemoryManager()
        {
            Dispose();
        }
    }
}
