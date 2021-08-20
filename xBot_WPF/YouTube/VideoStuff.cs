using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace xBot_WPF.YouTube
{
    public class VideoStuff
    {
        /// <summary>
        /// Extract duration time of a YouTube video.
        /// Live stream will return 0.
        /// </summary>
        /// <param name="url">YouTube URL</param>
        public static int YouTubeVideoDuration(string url)
        {
            try
            {
                string id = url.Split('=')[1];
                WebClient client = new WebClient();
                // Download youtube link source code.
                string titleParse = client.DownloadString("https://www.youtube.com/watch?v=" + id);

                // Grabing string containing elapsed time.
                int pFrom = titleParse.IndexOf("approxDurationMs") + "approxDurationMs".Length;
                string outs = titleParse.Substring(pFrom, 23);

                // Return digits only.
                int duration =Int32.Parse(Regex.Match(outs, @"\d+").Value);

                // Convert from milliseconds to secdons.
                return duration / 1000;
            }
            catch
            {
                // We return 0 in case of live videos.
                return 0;
            }
        }
    }
}
