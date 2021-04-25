using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LiveDataController : ControllerBase
    {
        const int PROCESS_WM_READ = 0x0010;

        const int XPOS_ADDRESS = 0x013FF828;

        const int YPOS_ADDRESS = 0x013FF82C;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess,
          int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [HttpGet]
        public string Get()
        {
            Process process = Process.GetProcessesByName("jcpicker")[0];
            IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);

            int bytesRead = 0;
            byte[] buffer = new byte[4];
            ReadProcessMemory((int)processHandle, XPOS_ADDRESS, buffer, buffer.Length, ref bytesRead);
            bytesRead = 0;
            int xPos = BitConverter.ToInt32(buffer, 0);
            ReadProcessMemory((int)processHandle, YPOS_ADDRESS, buffer, buffer.Length, ref bytesRead);
            bytesRead = 0;
            int yPos = BitConverter.ToInt32(buffer, 0);
            return "x, y Pos: " + xPos + ", " + yPos;
        }
    }
}
