using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Ini;

namespace Core
{
    public class Process_e
    {
        /// <summary>
        /// Checking if specific process window is opened.
        /// </summary>
        /// <param name="WindowTitle"></param>
        /// <returns></returns>
        /// 

        /// <summary>
        /// Check an open Process window titile
        /// </summary>
        /// <param name="WindowTitle"></param>
        /// <returns>bool</returns>
        public static string CheckProcessWinTitle(string WindowTitle)
        {
            string result = "";

            Process[] processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                if (process.MainWindowTitle.Contains(WindowTitle))
                {
                    result = "1";
                    return result;
                }

            }

            return result;
        }


        /// <summary>
        /// Open pricess kill function
        /// </summary>
        /// <param name="ProcessName"></param>
        public static void process_kill(string ProcessName)
        {
            Process[] processes = Process.GetProcessesByName(ProcessName);
            foreach (var process in processes)
            {
                    process.Kill();
            }

        } 
    }
}