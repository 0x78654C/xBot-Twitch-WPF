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
    /*Weather Forecast display using openweathermap.org API 
     Type of display: Imperial and Metric
     */

    public class WeatherForecast
    {
        private static readonly HttpClient s_ClientH = new HttpClient();

        /// <summary>
        /// weather api check and return the output parssed
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static string WeatherForecastData(string cityName, string apiKey, string weatherUnits)
        {

            string _date = DateTime.Now.ToString("yyyy_MM_dd");
            string date2;
            string errFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\log\errors\" + _date + "_log.txt";
            string outs;
            try
            {

                if (apiKey.Length > 0) // we check the lenght
                {
                    //Open weather map API link with celsius 
                    // TODO: will decide if I put switch for ferenhait
                    string html = @"https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";
                    if (weatherUnits == "1")
                    {
                        html = @"https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";
                    }
                    else
                    {
                        html = @"https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=imperial";
                    }

                    HttpResponseMessage response = s_ClientH.GetAsync(string.Format(html, cityName, Encryption._decryptData(apiKey))).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    string l = "";
                    string line = "";
                    //parssing the oudtput
                    responseBody = responseBody.Replace(",", Environment.NewLine);
                    responseBody = responseBody.Replace("\"", "");
                    responseBody = responseBody.Replace("}", "");
                    responseBody = responseBody.Replace("{", "");
                    responseBody = responseBody.Replace("wind:", "");
                    responseBody = responseBody.Replace("main:", "");
                    //---------------------------------
                    using (var sr = new StringReader(responseBody))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            //we check only for what we need, like: temp, feel, humidity, wind speed
                            if (line.Contains("temp") || line.Contains("feel") || line.Contains("humidity") || line.Contains("speed"))
                            {
                                l += line + Environment.NewLine;
                            }
                        }
                    }
                    outs = l;
                    //renaming output parts
                    outs = outs.Replace("temp:", " Temperature: ");
                    outs = outs.Replace("feels_like:", " Feels Like: ");
                    outs = outs.Replace("temp_min:", " Minim Temperature: ");
                    outs = outs.Replace("temp_max:", " Maxim Temperature: ");
                    outs = outs.Replace("humidity:", " Humidity: ");
                    outs = outs.Replace("speed:", " Wind Speed: ");
                    //---------------------------------
                }
                else
                {
                    //we print the issue on the log viewer console
                    outs = "No openweathermap.org API Key saved! Please check";
                }
            }
            catch (Exception e)
            {
                //In case of error we output this in console.
                outs = "Please check city name!";

                //save the entire error to file
                date2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                if (File.Exists(errFile))
                {
                    string rErrorFile = File.ReadAllText(errFile);

                    if (!rErrorFile.Contains("[" + date2 + "] Weather error: "))
                    {
                        CLog.LogWriteError("[" + date2 + "] Weather error: " + e.ToString() + Environment.NewLine);
                    }
                }
                else
                {
                    File.WriteAllText(errFile, "");
                    string rErrorFile = File.ReadAllText(errFile);

                    if (!rErrorFile.Contains("[" + date2 + "] Weather error: "))
                    {
                        CLog.LogWriteError("[" + date2 + "] Weather error: " + e.ToString() + Environment.NewLine);
                    }
                }
                //--------------------------------
            }
            //print the final weather forecast
            return outs;
        }
    }
}
