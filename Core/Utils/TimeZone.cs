using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public class TimeZone
    {
        private static readonly HttpClient s_ClientH = new HttpClient();

        /// <summary>
        /// TimeZone extract using http://worldtimeapi.org API
        /// </summary>
        /// <param name="Region">Continent</param>
        /// <param name="CityName">City Name</param>
        /// <returns>string</returns>
        public static string TimeZoneData(string Region, string CityName)
        {
            string _date = DateTime.Now.ToString("yyyy_MM_dd");
            string date2 = string.Empty;
            string errFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\log\errors\" + _date + "_log.txt";
            string outs = string.Empty;
            string html = @"http://worldtimeapi.org/api/timezone/{0}/{1}";
            try
            {

                HttpResponseMessage response = s_ClientH.GetAsync(string.Format(html, Region, CityName)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;

                //parsing the https output 
                string[] oS = responseBody.Split(',');
                string[] oY = oS[2].Split('.');
                oY[0] = oY[0].Replace("\"datetime\":\"", "Date: ");
                oY[0] = oY[0].Replace("T", " Hour: ");
                //-----------------

                //return the final data
                outs = oY[0];
            }
            catch (Exception e)
            {
                outs = "Please check City name or Continent name. Only Cities avaible on http://worldtimeapi.org are displyed!";
                //save the entire error to file
                date2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                if (File.Exists(errFile))
                {
                    string rErrorFile = File.ReadAllText(errFile);

                    if (!rErrorFile.Contains("[" + date2 + "] TimeZone error: "))
                    {
                        CLog.LogWriteError("[" + date2 + "] TimeZone error: " + e.ToString() + Environment.NewLine);
                    }
                }
                else
                {
                    File.WriteAllText(errFile, "");
                    string rErrorFile = File.ReadAllText(errFile);

                    if (!rErrorFile.Contains("[" + date2 + "] TimeZone error: "))
                    {
                        CLog.LogWriteError("[" + date2 + "] TimeZone error: " + e.ToString() + Environment.NewLine);
                    }
                }
                //--------------------------------
            }
            return outs;
        }
    }
}
