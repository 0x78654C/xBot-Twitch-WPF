using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Core
{
    /// <summary>
    /// Log system class for events 
    /// </summary>
    public class CLog
    {
        //Declare log path file variable and StreamWriter
        private static string s_LogPathFile;
        private static StreamWriter s_LWriter;
        private static string s_Date;
        //----------------------

        /// <summary>
        /// Log writer function
        /// </summary>
        /// <param name="data">String data to writhe in log file</param>
        public static void LogWrite(string data)
        {
            //grabbin the current date
            s_Date = DateTime.Now.ToString("yyyy_MM_dd");

            //declaring path using date
            s_LogPathFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\log\" + s_Date + "_log.txt";

            //Appending log data to file with string 
            try
            {
                using (s_LWriter = new StreamWriter(s_LogPathFile, append: true))
                {
                    s_LWriter.Write(data + Environment.NewLine);
                    s_LWriter.Close();
                }
            }
            catch
            {
                //TODO: future checks
            }
        }

        /// <summary>
        /// Write errors to log folder
        /// </summary>
        /// <param name="data">String data to write in error log files.</param>

        public static void LogWriteError(string data)
        {
            //grabbin the current date
            s_Date = DateTime.Now.ToString("yyyy_MM_dd");

            //declaring path using date
            s_LogPathFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\log\errors\" + s_Date + "_log.txt";

            //Appending log data to file with string 
            try
            {
                using (s_LWriter = new StreamWriter(s_LogPathFile, append: true))
                {
                    s_LWriter.Write(data + Environment.NewLine);
                    s_LWriter.Close();
                }
            }
            catch
            {
                //TODO: future checks
            }
        }
    }
}
