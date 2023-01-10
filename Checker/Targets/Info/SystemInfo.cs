using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Management;

namespace Checker
{
    class SystemInfo
    { 
        public static string username = Environment.UserName; 
        public static string compname = Environment.MachineName;
        public static string GetSystemVersion() // Получение версии виндовс
        {
            return GetWindowsVersionName() + " " + GetBitVersion();
        }
        public static string GetWindowsVersionName()// Версия виндовс
        {
            var sData = "Unknown System";
            try
            {
                using (var mSearcher = new ManagementObjectSearcher(@"root\CIMV2", " SELECT * FROM win32_operatingsystem"))
                {
                    foreach (var tObj in mSearcher.Get())
                        sData = Convert.ToString(tObj["Name"]);
                    sData = sData.Split(new char[] { '|' })[0];
                    var iLen = sData.Split(new char[] { ' ' })[0].Length;
                    sData = sData.Substring(iLen).TrimStart().TrimEnd();
                }
            }catch {return "Error"; } 
            return sData;
        }
        private static string GetBitVersion() // Получение битности
        {
            try
            {
                if (Registry.LocalMachine.OpenSubKey(@"HARDWARE\Description\System\CentralProcessor\0")
                    .GetValue("Identifier")
                    .ToString()
                    .Contains("x86"))
                    return "(32 Bit)";
                else
                    return "(64 Bit)";
            }catch {return "Error"; } 
        }
        
        public static string ScreenMetrics() // Получение разрешение экрана
        {
            var bounds = System.Windows.Forms.Screen.GetBounds(Point.Empty);
            var width = bounds.Width;
            var height = bounds.Height;
            return width + "x" + height;
        }
        public static string GetHWID() // Получаем HWID
        {
            var HWID = string.Empty;
            HWID = Registry.LocalMachine.OpenSubKey(@"SYSTEM\\CurrentControlSet\\Control\\SystemInformation")
                    .GetValue("ComputerHardwareId")
                    .ToString(); 
            return HWID;
        }
        public static string GetDisk() // Получаем инфу об Disk
        {
            try
            {
                var GPU = string.Empty;
                var mSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (var mObject in mSearcher.Get())
                {
                    for (int i = 0; i < 10; i++) 
                         GPU = mObject["Name"] + $"\n     ID: {mObject["SerialNumber"]}"; 
                }
                return GPU;
            }catch {return "Error"; } 
        }
        public static string GetCPU() // Получение инфу процессора
        {
            try
            {
                var CPU = string.Empty;
                var mSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                foreach (var mObject in mSearcher.Get())
                    CPU = mObject["Name"] + $" [{mObject["NumberOfCores"]}/{mObject["NumberOfLogicalProcessors"]}]" + $"\n     ID: {mObject["ProcessorId"]}";
                return CPU;
            }catch {return "Error"; } 
        }
        public static string GetRAM() // Получаем инфу RAM
        {
            try
            {
                var RAM = string.Empty;
                var RamAmount = 0;
                var mSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
                foreach (var mObject in mSearcher.Get())
                {
                    var Bytes = Convert.ToDouble(mObject["TotalPhysicalMemory"]);
                    RamAmount = (int)(Bytes / 1048576) - 1; 
                    break;
                }
                var a = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                foreach (var mObject in a.Get())
                { 
                    RAM = mObject["SerialNumber"].ToString();
                    break;
                }
                return RamAmount.ToString() + "MB" + $"\n     ID: {RAM}";
            }catch {return "Error"; } 
        } 
        public static string GetGPU() // Получаем инфу видеокарты
        {
            try
            {
                var GPU = string.Empty;
                var GPUAmount = 0;
                var mSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
                foreach (var mObject in mSearcher.Get())
                {
                    var Bytes = Convert.ToDouble(mObject["AdapterRam"]);
                    GPUAmount = (int)(Bytes / 1048576) - 1;
                    GPU = mObject["Name"] + $" [{GPUAmount}MB]";
                } 
                return GPU;
            }catch {return "Error"; }  
        }
        public static string GetSMBIOS() // Получаем инфу об материнки
        {
            try
            {
                var GPU = string.Empty; 
                var mSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                foreach (var mObject in mSearcher.Get())
                { 
                    GPU = mObject["Product"] + $"\n     ID: {mObject["SerialNumber"]}";
                }
                return GPU;
            }catch {return "Error"; } 
        }
        public static string GetMAC() // Получаем инфу об MAC Address
        {
            try
            {
                var MAC = string.Empty;
                var mSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter");
                foreach (var mObject in mSearcher.Get())
                {
                    MAC = mObject["Name"] + $"\n     ID: {mObject["MACAddress"]}";
                }
                return MAC;
            }catch {return "Error"; }  
        }
        public static string GetPhysicalMemory() // Получаем инфу об PhysicalMemory
        {
            try
            {
                var PhysicalMemory = string.Empty;
                var mSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
                foreach (var mObject in mSearcher.Get()) 
                    PhysicalMemory = mObject["PartNumber"].ToString(); 
                return PhysicalMemory;
            }catch {return "Error"; } 
        }
    }
}
