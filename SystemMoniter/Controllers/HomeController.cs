using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SystemMoniter.Models;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace SystemMoniter.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var proc = Process.GetCurrentProcess();
            var allproc = Process.GetProcesses();
            double allCPU = 0;
            int threadsCount = 0;
            Int64 handlesCount = 0;

            foreach (var process in allproc)
            {
                try
                {
                    allCPU += process.TotalProcessorTime.TotalMilliseconds;
                }
                catch (Exception)
                {
                    
                }
            }

            foreach (var process in allproc)
            {
                threadsCount += process.Threads.Count;
                handlesCount += process.HandleCount;
            }

            //var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            //cpuCounter.NextValue();

            //var uptime = new PerformanceCounter("System", "System Up Time");
            //uptime.NextValue();       //Call this an extra time before reading its value
            //var time = TimeSpan.FromSeconds(uptime.NextValue());

            Int64 phav = PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
            Int64 tot = PerformanceInfo.GetTotalMemoryInMiB();

            DriveInfo[] drives = DriveInfo.GetDrives();
            long HHD = 0;
            long usedHHD = 0;

            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    HHD += drive.TotalSize;
                    usedHHD += drive.TotalFreeSpace;
                }
            }

            AdminPanelGInfoViewModel Info = new AdminPanelGInfoViewModel();
            Info.CPuLoad = allCPU ;
            Info.systemUptime = proc.UserProcessorTime.TotalMilliseconds;
            Info.ThreadsCount = threadsCount;
            Info.ProcessCount = allproc.Count();
            Info.HandlesCount = handlesCount;

            Info.TotalPhyemory = tot / 1000;
            Info.MemoryUsage = (tot - phav) / 1000;
            Info.MemoryUsedPer = (long)(Info.MemoryUsage / Info.TotalPhyemory * 100);

            Info.HDDTotalSpace = HHD / 1000000000;
            Info.HDDUsage = (HHD - usedHHD) / 1000000000;
            Info.HDDUsedPer = (long)((float)Info.HDDUsage / (float)Info.HDDTotalSpace * 100);

            return View(Info);
        }


        public static class PerformanceInfo
        {
            [DllImport("psapi.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetPerformanceInfo([Out] out PerformanceInformation PerformanceInformation, [In] int Size);

            [StructLayout(LayoutKind.Sequential)]
            public struct PerformanceInformation
            {
                public int Size;
                public IntPtr CommitTotal;
                public IntPtr CommitLimit;
                public IntPtr CommitPeak;
                public IntPtr PhysicalTotal;
                public IntPtr PhysicalAvailable;
                public IntPtr SystemCache;
                public IntPtr KernelTotal;
                public IntPtr KernelPaged;
                public IntPtr KernelNonPaged;
                public IntPtr PageSize;
                public int HandlesCount;
                public int ProcessCount;
                public int ThreadCount;
            }

            public static Int64 GetPhysicalAvailableMemoryInMiB()
            {
                PerformanceInformation pi = new PerformanceInformation();
                if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
                {
                    return Convert.ToInt64((pi.PhysicalAvailable.ToInt64() * pi.PageSize.ToInt64() / 1048576));
                }
                else
                {
                    return -1;
                }

            }

            public static Int64 GetTotalMemoryInMiB()
            {
                PerformanceInformation pi = new PerformanceInformation();
                if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
                {
                    return Convert.ToInt64((pi.PhysicalTotal.ToInt64() * pi.PageSize.ToInt64() / 1048576));
                }
                else
                {
                    return -1;
                }

            }

        }
     
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
