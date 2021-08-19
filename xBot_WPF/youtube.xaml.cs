using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YouTubePlayList = xBot_WPF.YouTube.PlayList;

namespace xBot_WPF
{
    /// <summary>
    /// Interaction logic for youtube.xaml
    /// </summary>
    public partial class youtube : Window
    {
        // Declare path to youtube links file.
        private static string s_ytControl = "0";
        MatchCollection matches;
        readonly static string s_playListFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\playList.txt";
        private static string s_playLink;
        private static string s_ytTitle;
        //-----------------------------------------
        // Declare keyname.
        private static string s_keyName = "xBot";
        //----------------------------------------

        // Timer and counter declaration for play next vid function.
        private System.Windows.Forms.Timer timer1;
        private int counter = 0;
        //----------------------------------------

        // Declare variables for play requested song.
        List<string> listRequestedSongs = new List<string>();
        readonly static string s_playListRequest = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\playListRequest.txt";
        private System.Windows.Forms.Timer timer2;
        private static string s_ytRequest = string.Empty;
        //-------------------------------------

        public youtube()
        {
            InitializeComponent();

            // Load youtube link 
            s_playLink = Reg.regKey_Read(s_keyName, "YtUrl");
            //--------------------------

            // Load control number from registry
            s_ytControl = Reg.regKey_Read(s_keyName, "YTControl");
            //---------------------------

            // Set control number for window status
            Reg.regKey_WriteSubkey(s_keyName, "YtWin", "1");
            //---------------------------

            // Load control number for youtube request song
            s_ytRequest = Reg.regKey_Read(s_keyName, "ytRequest");
            //---------------------------

            // Set playReqCKB status
            if (s_ytRequest == "1")
            {
                playReqCKB.IsEnabled = true;
            }
            else
            {
                playReqCKB.IsEnabled = false;
            }
            //---------------------------

            // Load links from playslistfile in listbox
            YouTubePlayList.LoadPlayListLinks(s_playListFile, playList);
            //---------------------------

            // Start timer for play next song.
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(PlayNexTimer_Tick);
            timer1.Interval = 1000; // 1 second
            timer1.Stop();
            //---------------------------

            // Load song request in list.
            YouTubePlayList.LoadSongsInList(s_playListRequest, listRequestedSongs);
            //---------------------------
            
            // Start timer for play requested song.
            timer2 = new System.Windows.Forms.Timer();
            timer2.Tick += new EventHandler(LoadRequestedSongs);
            timer2.Interval = 60000; // 1 minute
            timer2.Start();
            //---------------------------
        }


        /// <summary>
        /// Youtube reload link on browser.
        /// </summary>
        /// <param name="youtubeUrl"></param>
        public void ReloadVideo(string youtubeUrl)
        {
            WebClient client = new WebClient();
            string titleParse;
            string html = "<html><head><center>";
            html += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
            html += "<iframe id='video' src= 'https://www.youtube.com/embed/{0}?rel=0&amp;&amp;showinfo=0;&controls=0;&autoplay=1' width='647' height='380' frameborder='0' allowfullscreen></iframe>";
            html += "</center></body></html>";
            try
            {
                this.ytBrowser.NavigateToString(string.Format(html, youtubeUrl.Split('=')[1]));
                Reg.regKey_WriteSubkey(s_keyName, "YTControl", "1");

                // Download youtube link source code.
                titleParse = client.DownloadString(youtubeUrl);

                // Grabing the title from source.
                int pFrom = titleParse.IndexOf("<title>") + "<title>".Length;
                int pTo = titleParse.LastIndexOf("</title>");

                // Store the title.
                s_ytTitle = titleParse.Substring(pFrom, pTo - pFrom);
                s_playLink = youtubeUrl;

                // Store youtube title in registry.
                Reg.regKey_WriteSubkey(s_keyName, "YtLink", s_ytTitle + ": " + s_playLink);

                // Store youtube link only in registry.
                Reg.regKey_WriteSubkey(s_keyName, "YtUrl", s_playLink);
            }
            catch
            {
                string html1 = "<html><head><center>";
                html1 += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
                html1 += "<iframe id='video' src= ' ' width='647' height='380' frameborder='0' allowfullscreen></iframe>";
                html1 += "</center></body></html>";
                this.ytBrowser.NavigateToString(html1);
                Reg.regKey_WriteSubkey(s_keyName, "YTControl", "0");
            }
        }

        /// <summary>
        /// Drag window function event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        /// <summary>
        /// Minimize window label button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiniMizeLBL_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Close window label button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseLBL_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            //reset window control key to 0 on colose
            Reg.regKey_WriteSubkey(s_keyName, "YtWin", "0");
            //---------------------------------------
        }


        /// <summary>
        /// Play button for youtube links
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayBTN_Click(object sender, RoutedEventArgs e)
        {
            // playNext();
            if (playBTN.Content.ToString() == "Play")
            {
                ReloadVideo(s_playLink);
                playBTN.Content = "Stop";
            }
            else
            {
                timer1.Stop();
                string html1 = "<html><head><center>";
                html1 += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
                html1 += "<iframe id='video' src= ' ' width='647' height='380' frameborder='0' allowfullscreen></iframe>";
                html1 += "</center></body></html>";
                this.ytBrowser.NavigateToString(html1);
                Reg.regKey_WriteSubkey(s_keyName, "YTControl", "0");
                playBTN.Content = "Play";
                if (playReqCKB.IsChecked == true)
                {
                    playReqCKB.IsChecked = false;
                    playNextCKB.IsChecked = false;
                }
                else
                {
                    playReqCKB.IsChecked = false;
                    playNextCKB.IsChecked = false;
                    playNextCKB.IsEnabled = true;
                    playReqCKB.IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// Clear controls for youtube player and stop play.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            string html1 = "<html><head><center>";
            html1 += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
            html1 += "<iframe id='video' src= ' ' width='647' height='380' frameborder='0' allowfullscreen></iframe>";
            html1 += "</center></body></html>";
            this.ytBrowser.NavigateToString(html1);
            Reg.regKey_WriteSubkey(s_keyName, "YTControl", "0");

            //Reset window control key to 0 on colose.
            Reg.regKey_WriteSubkey(s_keyName, "YtWin", "0");
            //-------------------------------------------
            timer1.Stop();
        }

        /// <summary>
        /// Add song from tb to list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void AddToListBTN(object sender, RoutedEventArgs e)
        {
            //we check if continas the '=' symbol for check and after cont it 
            matches = Regex.Matches(youtTubeLink.Text, "=");

            if (youtTubeLink.Text.Contains("youtube.") && matches.Count == 1)
            {
                if (!playList.Items.Contains(youtTubeLink.Text))
                {
                    playList.Items.Add(youtTubeLink.Text);
                    using (var sFile = new StreamWriter(s_playListFile, append: true))
                    {
                        sFile.WriteLine(youtTubeLink.Text);
                        sFile.Flush();
                        sFile.Close();
                    }
                    youtTubeLink.Clear();
                }
                else
                {
                    MessageBox.Show(youtTubeLink.Text + " already exists in playlist!");
                }
            }
            else
            {
                MessageBox.Show("You have typed a invalid YouTube link!");
            }
        }

        /// <summary>
        /// Play song on dboule click list value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ReloadVideo(playList.SelectedItem.ToString());
            //store title + youtube link in registry
            Reg.regKey_WriteSubkey(s_keyName, "YtLink", s_ytTitle + ": " + playList.SelectedItem.ToString());

            //store youtube link only in registry
            Reg.regKey_WriteSubkey(s_keyName, "YtUrl", playList.SelectedItem.ToString());
            playBTN.Content = "Stop";
        }

        /// <summary>
        /// Remove link from list .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemListBTN(object sender, RoutedEventArgs e)
        {
            string[] playlinks = File.ReadAllLines(s_playListFile);
            string playL = File.ReadAllText(s_playListFile);
            if (playList.SelectedIndex != -1)
            {
                foreach (var line in playlinks)
                {
                    if (line.Contains(playList.SelectedItem.ToString()))
                    {
                        playL = playL.Replace(line, "");
                    }
                }

                playList.Items.Remove(playList.SelectedItem);
                playL = Regex.Replace(playL, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
                using (var sW = new StreamWriter(s_playListFile))
                {
                    sW.Write(playL);
                    sW.Flush();
                    sW.Close();
                }
            }
            else
            {
                MessageBox.Show("You need to select a link from list!");
            }
        }

        /// <summary>
        /// Extract duration time of a YouTube video.
        /// </summary>
        /// <param name="url"></param>
        private static int YTVideoDuration(string url)
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
                string duration = Regex.Match(outs, @"\d+").Value;

                // Convet to int32.
                int yDuration = Int32.Parse(duration);

                // Convert from milliseconds to secdons.
                return yDuration / 1000;
            }
            catch
            {
                // We return 0 in case of live videos.
                return 0;
            }
        }


        /// <summary>
        /// Play next song from listbox.
        /// </summary>
        private void PlayNext()
        {
            try
            {
                if (s_playLink.Length > 0)
                {
                    int i = playList.Items.IndexOf(s_playLink);
                    int c = playList.Items.Count;
                    playList.SelectedIndex = i + 1;
                    if (playList.SelectedIndex > c - 1)
                    {
                        playList.SelectedIndex = 0;
                        playList.Focus();
                        ReloadVideo(playList.SelectedItem.ToString());
                        counter = YTVideoDuration(s_playLink);
                        Reg.regKey_WriteSubkey(s_keyName, "YtLink", s_ytTitle + ": " + s_playLink);

                        //store youtube link only in registry
                        Reg.regKey_WriteSubkey(s_keyName, "YtUrl", s_playLink);
                    }
                    else
                    {
                        playList.SelectedIndex = i + 1;
                        playList.Focus();
                        ReloadVideo(playList.SelectedItem.ToString());
                        counter = YTVideoDuration(s_playLink);
                        Reg.regKey_WriteSubkey(s_keyName, "YtLink", s_ytTitle + ": " + s_playLink);

                        //store youtube link only in registry
                        Reg.regKey_WriteSubkey(s_keyName, "YtUrl", s_playLink);
                    }
                }
            }
            catch (Exception e)
            {
                string date2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                CLog.LogWriteError("[" + date2 + "] YouTube Error: " + e.ToString() + Environment.NewLine);
            }
        }


        /// <summary>
        /// Decrement counter for play next song.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayNexTimer_Tick(object sender, EventArgs e)
        {
            counter--;
            if (counter == 0)
            {
                if (playReqCKB.IsChecked == true)
                {
                    //play requested songs
                    PlayRequestedSong();
                }
                else
                {
                    //play next song 
                    PlayNext();
                }
            }
        }

        /// <summary>
        /// Activate play next song.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayNextCKB_Checked(object sender, RoutedEventArgs e)
        {
            ReloadVideo(s_playLink);
            counter = YTVideoDuration(s_playLink);
            timer1.Start();
            playBTN.Content = "Stop";
            playReqCKB.IsChecked = false;
            playReqCKB.IsEnabled = false;
        }

        /// <summary>
        /// Deactivate play next song.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayNextCKB_Unchecked(object sender, RoutedEventArgs e)
        {
            timer1.Stop();
            string html1 = "<html><head><center>";
            html1 += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
            html1 += "<iframe id='video' src= ' ' width='647' height='380' frameborder='0' allowfullscreen></iframe>";
            html1 += "</center></body></html>";
            this.ytBrowser.NavigateToString(html1);
            Reg.regKey_WriteSubkey(s_keyName, "YTControl", "0");
            playBTN.Content = "Play";

            if (s_ytRequest == "1")
            {
                playReqCKB.IsEnabled = true;
            }
        }
        /// <summary>
        /// Open song reqeust lists.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SongRequestListOpen(object sender, RoutedEventArgs e)
        {
            yTsongRequest srq = new yTsongRequest();
            srq.Show();
        }

        /// <summary>
        /// Load songs from external file in list.
        /// </summary>
        private void LoadRequestedSongs(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines(s_playListRequest);
            listRequestedSongs.Clear();
            foreach (var line in lines)
            {
                if (line.Length > 0)
                {
                    listRequestedSongs.Add(line);
                }
            }
        }

        /// <summary>
        /// Plays requested songs from list.
        /// </summary>
        private void PlayRequestedSong()
        {
            try
            {
                // We count the songs in list
                int c = listRequestedSongs.Count;

                // Check if count is not null
                if (c > 0)
                {
                    // We decrement the count because list index start from 0
                    int i = c - 1;

                    // Get the song from list by index incrementor
                    string[] song = listRequestedSongs.ElementAt(i).Split('|');

                    // Play
                    ReloadVideo(song[1]);

                    // Get video duration for next song
                    counter = YTVideoDuration(song[1]);

                    // Store youtube title in registry
                    Reg.regKey_WriteSubkey(s_keyName, "YtLink", s_ytTitle + ": " + s_playLink);

                    // Store youtube link only in registry
                    Reg.regKey_WriteSubkey(s_keyName, "YtUrl", s_playLink);

                    // We removed played song 

                    listRequestedSongs.Remove(listRequestedSongs.ElementAt(i));
                    string playReqD = string.Join(Environment.NewLine, listRequestedSongs);

                    File.WriteAllText(s_playListRequest, playReqD);
                }
            }
            catch (Exception e)
            {
                string date2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                CLog.LogWriteError("[" + date2 + "] YouTube Error: " + e.ToString() + Environment.NewLine);
            }
        }

        /// <summary>
        /// We start the play next song from request list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaReqCKB_Checked(object sender, RoutedEventArgs e)
        {
            LoadRequestedSongs(sender, e);
            if (listRequestedSongs.Count > 0)
            {
                string[] song = listRequestedSongs.ElementAt(0).Split('|');
                ReloadVideo(song[1]);
                counter = YTVideoDuration(song[1]);
                listRequestedSongs.Remove(listRequestedSongs.ElementAt(0));

                string playReqD = string.Join(Environment.NewLine, listRequestedSongs);
                File.WriteAllText(s_playListRequest, playReqD);
                timer1.Start();
                playNextCKB.IsChecked = false;
                playNextCKB.IsEnabled = false;
                playBTN.Content = "Stop";
            }
            else
            {
                MessageBox.Show("No songs in requested list!");
                playReqCKB.IsChecked = false;
            }
        }
        /// <summary>
        ///  We stop the play next song from request list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayReqCKB_Unchecked(object sender, RoutedEventArgs e)
        {
            timer1.Stop();
            counter = 0;
            playNextCKB.IsEnabled = true;
            playBTN.Content = "Play";
        }
    }

}
