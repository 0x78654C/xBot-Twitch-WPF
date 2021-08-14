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
        private static string logPathFile;
        static StreamWriter lWriter;
        static string _date;
        //----------------------

        /// <summary>
        /// Log writer function
        /// </summary>
        /// <param name="data">String data to writhe in log file</param>
        public static void LogWrite(string data)
        {
            //grabbin the current date
            _date = DateTime.Now.ToString("yyyy_MM_dd");

            //declaring path using date
            logPathFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\log\" + _date + "_log.txt";

            //Appending log data to file with string 
            try
            {
                using (lWriter = new StreamWriter(logPathFile, append: true))
                {
                    lWriter.Write(data + Environment.NewLine);
                    lWriter.Close();
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
            _date = DateTime.Now.ToString("yyyy_MM_dd");

            //declaring path using date
            logPathFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\log\errors\" + _date + "_log.txt";

            //Appending log data to file with string 
            try
            {
                using (lWriter = new StreamWriter(logPathFile, append: true))
                {
                    lWriter.Write(data + Environment.NewLine);
                    lWriter.Close();
                }
            }
            catch
            {
                //TODO: future checks
            }
        }
    }
}
