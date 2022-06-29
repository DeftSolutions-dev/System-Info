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
            string sData = "Unknown System";
            try
            {
                using (ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(@"root\CIMV2", " SELECT * FROM win32_operatingsystem"))
                {
                    foreach (ManagementObject tObj in mSearcher.Get())
                        sData = Convert.ToString(tObj["Name"]);
                    sData = sData.Split(new char[] { '|' })[0];
                    int iLen = sData.Split(new char[] { ' ' })[0].Length;
                    sData = sData.Substring(iLen).TrimStart().TrimEnd();
                }
            }
            catch 
            {
                return "Error";
            }
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
            }
            catch 
            {
                return "Error";
            } 
        }
        
        public static string ScreenMetrics() // Получение разрешение экрана
        {
            Rectangle bounds = System.Windows.Forms.Screen.GetBounds(Point.Empty);
            int width = bounds.Width;
            int height = bounds.Height;
            return width + "x" + height;
        }
        public static string GetHWID() // Получаем HWID
        {
            string HWID = string.Empty;
            HWID = Registry.LocalMachine.OpenSubKey(@"SYSTEM\\CurrentControlSet\\Control\\SystemInformation")
                    .GetValue("ComputerHardwareId")
                    .ToString(); 
            return HWID;
        }
        public static string GetDisk() // Получаем инфу об Disk
        {
            try
            {
                string GPU = string.Empty;
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject mObject in mSearcher.Get())
                {
                    for (int i = 0; i < 10; i++)
                    {
                            GPU = mObject["Name"] + $"\n     ID: {mObject["SerialNumber"]}";
                    }
                }
                return GPU;
            }
            catch
            {
                return "Error";
            }
        }
        public static string GetCPU() // Получение инфу процессора
        {
            try
            {
                string CPU = string.Empty;
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                foreach (ManagementObject mObject in mSearcher.Get())
                    CPU = mObject["Name"] + $" [{mObject["NumberOfCores"]}/{mObject["NumberOfLogicalProcessors"]}]" + $"\n     ID: {mObject["ProcessorId"]}";
                return CPU;
            }
            catch 
            {
                return "Error";
            }
        }
        public static string GetRAM() // Получаем инфу RAM
        {
            try
            {
                string RAM = string.Empty;
                int RamAmount = 0;
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementObject mObject in mSearcher.Get())
                {
                    double Bytes = Convert.ToDouble(mObject["TotalPhysicalMemory"]);
                    RamAmount = (int)(Bytes / 1048576) - 1; 
                    break;
                }
                ManagementObjectSearcher a = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject mObject in a.Get())
                { 
                    RAM = mObject["SerialNumber"].ToString();
                    break;
                }
                return RamAmount.ToString() + "MB" + $"\n     ID: {RAM}";
            }
            catch
            {
                return "Error";
            }
        } 
        public static string GetGPU() // Получаем инфу видеокарты
        {
            try
            {
                string GPU = string.Empty;
                int GPUAmount = 0;
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
                foreach (ManagementObject mObject in mSearcher.Get())
                {
                    double Bytes = Convert.ToDouble(mObject["AdapterRam"]);
                    GPUAmount = (int)(Bytes / 1048576) - 1;
                    GPU = mObject["Name"] + $" [{GPUAmount}MB]";
                } 
                return GPU;
            }
            catch 
            {
                return "Error";
            }
        }
        public static string GetSMBIOS() // Получаем инфу об материнки
        {
            try
            {
                string GPU = string.Empty; 
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                foreach (ManagementObject mObject in mSearcher.Get())
                { 
                    GPU = mObject["Product"] + $"\n     ID: {mObject["SerialNumber"]}";
                }
                return GPU;
            }
            catch 
            {
                return "Error";
            }
        }
        public static string GetMAC() // Получаем инфу об MAC Address
        {
            try
            {
                string MAC = string.Empty;
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter");
                foreach (ManagementObject mObject in mSearcher.Get())
                {
                    MAC = mObject["Name"] + $"\n     ID: {mObject["MACAddress"]}";
                }
                return MAC;
            }
            catch 
            {
                return "Error";
            }
        }
        public static string GetPhysicalMemory() // Получаем инфу об PhysicalMemory
        {
            try
            {
                string PhysicalMemory = string.Empty;
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
                foreach (ManagementObject mObject in mSearcher.Get())
                {
                    PhysicalMemory = mObject["PartNumber"].ToString();
                }
                return PhysicalMemory;
            }
            catch
            {
                return "Error";
            }
        }
    }
}
