using Masuit.Tools.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// 硬件监测工具
    /// </summary>
    public static class NGHardwareMonitorUtil
    {
        /// <summary>
        /// 硬件监测
        /// </summary>
        /// <returns>硬件监测信息</returns>
        public static Object StartMonitoring()
        {

            int cpuCount = 0;// 获取CPU核心数
            float cpuLoad = 0;// 获取CPU占用率
            double cpuTemperature = 0;// 获取CPU温度
            List<DiskInfo> diskInfo = null;// 获取磁盘每个分区可用空间
            string localUsedIp = "";// 获取本机当前正在使用的IP地址
            RamInfo ramInfo = null;// 获取内存信息
            List<CpuInfo> CpuInfo = null;

            bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            if (isLinux)
            {
                cpuCount = Environment.ProcessorCount;
                //TODO: Linux 硬件监测待完善
            }
            else
            {
                cpuCount = SystemInfo.GetCpuCount();// 获取CPU核心数
                cpuLoad = SystemInfo.CpuLoad;// 获取CPU占用率
                cpuTemperature = SystemInfo.GetCPUTemperature();// 获取CPU温度
                diskInfo = SystemInfo.GetDiskInfo();// 获取磁盘每个分区可用空间
                localUsedIp = SystemInfo.GetLocalUsedIP().ToString();// 获取本机当前正在使用的IP地址
                ramInfo = SystemInfo.GetRamInfo();// 获取内存信息
                CpuInfo = SystemInfo.GetCpuInfo();// 获取CPU信息
            }

            return new
            {
                CpuCount = cpuCount,
                CpuLoad = cpuLoad,
                CpuTemperature = cpuTemperature,
                CpuInfo = CpuInfo,
                LogicalDrives = Environment.GetLogicalDrives(),
                DiskInfo = diskInfo?.DistinctBy(t => t.SerialNumber),
                LocalUsedIP = localUsedIp,
                RamInfo = ramInfo,
                OSArchitecture = System.Enum.GetName(typeof(Architecture), RuntimeInformation.OSArchitecture),
                OSDescription = RuntimeInformation.OSDescription,
                ProcessArchitecture = System.Enum.GetName(typeof(Architecture), RuntimeInformation.ProcessArchitecture),
                FrameworkDescription = RuntimeInformation.FrameworkDescription,
                Windows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                Is64BitProcess = Environment.Is64BitProcess,
                OSVersion = Environment.OSVersion,
                CpuCore = Environment.ProcessorCount,
                HostName = Environment.MachineName
            };
        }
    }
}
