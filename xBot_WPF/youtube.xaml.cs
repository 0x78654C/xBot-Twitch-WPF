using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Text.RegularExpressions;
using Core;
using System.Net;

namespace xBot_WPF
{
    /// <summary>
    /// Interaction logic for youtube.xaml
    /// </summary>
    public partial class youtube : Window
    {

        //Declare path to youtube links file
        private static string ytControl = "0";
        MatchCollection matches;
        readonly static string playListFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\playList.txt";
        private static string playLink;
        private static string ytTitle;
        //-----------------------------------------

        //Declare keyname
        private static string keyName = "xBot";
        //----------------------------------------

        //Timer and counter declaration for play next vid function
        private System.Windows.Forms.Timer timer1;

        private int counter = 0;
        //----------------------------------------

        //Declare variables for play requested song
        List<string> ListRequestedSongs = new List<string>();
        readonly static string playListRequest = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\playListRequest.txt";
        private System.Windows.Forms.Timer timer2;
        private static string ytRequest = string.Empty;
        //-------------------------------------

        public youtube()
        {
            InitializeComponent();

            //load youtube link 
            playLink = Reg.regKey_Read(keyName, "YtUrl");
            //--------------------------

            //Load control number from registry
            ytControl = Reg.regKey_Read(keyName, "YTControl");
            //---------------------------

            //Set control number for window status
            Reg.regKey_WriteSubkey(keyName, "YtWin", "1");
            //---------------------------

            //Load control number for youtube request song
            ytRequest = Reg.regKey_Read(keyName, "ytRequest");
            //---------------------------

            //set playReqCKB status
            if (ytRequest == "1")
            {
                playReqCKB.IsEnabled = true;
            }
            else
            {
                playReqCKB.IsEnabled = false;
            }
            //---------------------------

            //load links from playslistfile in listbox
            string[] yLinks = File.ReadAllLines(playListFile);

            foreach (var line in yLinks)
            {
                if (line.Length > 0)
                {
                    playList.Items.Add(line) ;
                }
            }
            //---------------------------

            //start timer for play next song
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(playNexTimer_Tick);
            timer1.Interval = 1000; // 1 second
            timer1.Stop();
            //---------------------------

            //load song request in list
            string[] lines = File.ReadAllLines(playListRequest);
            foreach (var line in lines)
            {
                if (line.Length > 0)
                {
                   
                    ListRequestedSongs.Add(line);
                }
            }

            //---------------------------
            //start timer for play requested song
            timer2 = new System.Windows.Forms.Timer();
            timer2.Tick += new EventHandler(LoadRequestedSongs);
            timer2.Interval = 60000; // 1 minute
            timer2.Start();
            //---------------------------



        }

        /// <summary>
        /// youtube reload link on browser
        /// </summary>
        /// <param name="url"></param>
        public void reload_YT(string url)
        {
            WebClient client = new WebClient();
            string titleParse;
            string html = "<html><head><center>";
            html += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
            html += "<iframe id='video' src= 'https://www.youtube.com/embed/{0}?rel=0&amp;&amp;showinfo=0;&controls=0;&autoplay=1' width='647' height='380' frameborder='0' allowfullscreen></iframe>";
            html += "</center></body></html>";
            try
            {
                this.ytBrowser.NavigateToString(string.Format(html, url.Split('=')[1]));
                Reg.regKey_WriteSubkey(keyName, "YTControl", "1");
                


                //Download youtube link source code
                titleParse = client.DownloadString(url);

                //grabing the title from source
                int pFrom = titleParse.IndexOf("<title>") + "<title>".Length;
                int pTo = titleParse.LastIndexOf("</title>");

                //store the title
                ytTitle=titleParse.Substring(pFrom, pTo - pFrom);
                playLink = url;

                //store youtube title in registry
                Reg.regKey_WriteSubkey(keyName, "YtLink", ytTitle + ": " + playLink);

                //store youtube link only in registry
                Reg.regKey_WriteSubkey(keyName, "YtUrl", playLink);
            }
            catch
            {
                string html1 = "<html><head><center>";
                html1 += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
                html1 += "<iframe id='video' src= ' ' width='647' height='380' frameborder='0' allowfullscreen></iframe>";
                html1 += "</center></body></html>";
                this.ytBrowser.NavigateToString(html1);
                Reg.regKey_WriteSubkey(keyName, "YTControl", "0");

            }


        }
        /// <summary>
        /// Drag window function event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }


        /// <summary>
        /// minimize window label button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miniMizeLBL_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        /// <summary>
        /// Close window label button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeLBL_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            //reset window control key to 0 on colose
            Reg.regKey_WriteSubkey(keyName, "YtWin", "0");
            //---------------------------------------
        }


        /// <summary>
        /// play button for youtube links
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playBTN_Click(object sender, RoutedEventArgs e)
        {
           // playNext();
            if (playBTN.Content.ToString() == "Play")
            {
                reload_YT(playLink);
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
                Reg.regKey_WriteSubkey(keyName, "YTControl", "0");
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
        /// clear controls for youtube player and stop play
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
            Reg.regKey_WriteSubkey(keyName, "YTControl", "0");
            //reset window control key to 0 on colose
            Reg.regKey_WriteSubkey(keyName, "YtWin", "0");
            //-------------------------------------------
            timer1.Stop();
        }

        /// <summary>
        /// Add song from tb to list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void addToListBTN(object sender, RoutedEventArgs e)
        {
            //we check if continas the '=' symbol for check and after cont it 
            matches = Regex.Matches(youtTubeLink.Text, "=");

            if (youtTubeLink.Text.Contains("youtube.") && matches.Count == 1)
            {
                if (!playList.Items.Contains(youtTubeLink.Text))
                {
                    playList.Items.Add(youtTubeLink.Text);
                    using (var sFile = new StreamWriter(playListFile, append: true))
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
        /// Play song on dboule click list value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            reload_YT(playList.SelectedItem.ToString());
            //store title + youtube link in registry
            Reg.regKey_WriteSubkey(keyName, "YtLink",ytTitle+ ": " + playList.SelectedItem.ToString());

            //store youtube link only in registry
            Reg.regKey_WriteSubkey(keyName, "YtUrl",playList.SelectedItem.ToString());
            playBTN.Content = "Stop";
        }

        /// <summary>
        /// remove link from list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remListBTN(object sender, RoutedEventArgs e)
        {
            string[] playlinks = File.ReadAllLines(playListFile);
            string playL = File.ReadAllText(playListFile);
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
                playL= Regex.Replace(playL, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
                using(var sW = new StreamWriter(playListFile))
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
        /// Extract duration time of a YouTube video
        /// </summary>
        /// <param name="url"></param>
        private static int YTVideoDuration(string url)
        {
            try
            {
                string id = url.Split('=')[1];
                WebClient client = new WebClient();
                //Download youtube link source code
                string titleParse = client.DownloadString("https://www.youtube.com/watch?v=" + id);

                //grabing string containing elapsed time
                int pFrom = titleParse.IndexOf("approxDurationMs") + "approxDurationMs".Length;
                string outs = titleParse.Substring(pFrom, 23);

                //return digits only
                string duration = Regex.Match(outs, @"\d+").Value;

                //convet to int32
                int yDuration = Int32.Parse(duration);

                //convert from milliseconds to secdons
                return yDuration / 1000;
            }catch{

                //we return 0 in case of live videos
                return 0;
            }
        }

       
        /// <summary>
        /// Play next song from listbox
        /// </summary>
        private void playNext()
        {
            try
            {
                if (playLink.Length > 0)
                {
                    int i= playList.Items.IndexOf(playLink);
                    int c = playList.Items.Count;
                    playList.SelectedIndex = i + 1;
                    if (playList.SelectedIndex > c - 1)
                    {
                        playList.SelectedIndex = 0;
                        playList.Focus();
                        reload_YT(playList.SelectedItem.ToString());
                        counter = YTVideoDuration(playLink);
                        Reg.regKey_WriteSubkey(keyName, "YtLink", ytTitle + ": " + playLink);

                        //store youtube link only in registry
                        Reg.regKey_WriteSubkey(keyName, "YtUrl", playLink);
                    }
                    else
                    {
                        playList.SelectedIndex = i + 1;
                        playList.Focus();
                        reload_YT(playList.SelectedItem.ToString());
                        counter = YTVideoDuration(playLink);
                        Reg.regKey_WriteSubkey(keyName, "YtLink", ytTitle + ": " + playLink);

                        //store youtube link only in registry
                        Reg.regKey_WriteSubkey(keyName, "YtUrl", playLink);

                    }

                }

            }catch(Exception e)
            {
               string date2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                CLog.LogWriteError("[" + date2 + "] YouTube Error: " + e.ToString() + Environment.NewLine);
               

            }
        }


        /// <summary>
        /// Decrement counter for play next song 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playNexTimer_Tick(object sender, EventArgs e)
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
                    playNext();
                }
            }
        }

        /// <summary>
        /// Activate play next song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playNextCKB_Checked(object sender, RoutedEventArgs e)
        {

            reload_YT(playLink);
            counter = YTVideoDuration(playLink);
            timer1.Start();
            playBTN.Content = "Stop";
            playReqCKB.IsChecked = false;
            playReqCKB.IsEnabled = false;
        }

        /// <summary>
        /// Deactivate play next song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playNextCKB_Unchecked(object sender, RoutedEventArgs e)
        {
            timer1.Stop();
            string html1 = "<html><head><center>";
            html1 += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
            html1 += "<iframe id='video' src= ' ' width='647' height='380' frameborder='0' allowfullscreen></iframe>";
            html1 += "</center></body></html>";
            this.ytBrowser.NavigateToString(html1);
            Reg.regKey_WriteSubkey(keyName, "YTControl", "0");
            playBTN.Content = "Play";

            if (ytRequest == "1")
            {
                playReqCKB.IsEnabled = true;
            }
        }
        /// <summary>
        /// Open song reqeust lists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void songRq(object sender, RoutedEventArgs e)
        {
            yTsongRequest srq = new yTsongRequest();
            srq.Show();
        }

        /// <summary>
        /// Load songs from external file in list
        /// </summary>
        private void LoadRequestedSongs(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines(playListRequest);
            ListRequestedSongs.Clear();
            foreach(var line in lines)
            {
                if(line.Length > 0)
                {
                    ListRequestedSongs.Add(line);
                }
            }
        }


        /// <summary>
        /// Plays requested songs from list
        /// </summary>
        private void PlayRequestedSong()
        {

            try
            {
                //we count the songs in list
                int c = ListRequestedSongs.Count;


                //check if count is not null
                if (c > 0 )
                {
                    //we decrement the count because list index start from 0
                    int i = c - 1;

                    //get the song from list by index incrementor
                    string[] song = ListRequestedSongs.ElementAt(i).Split('|');

                    //play
                    reload_YT(song[1]);

                    //get video duration for next song
                    counter = YTVideoDuration(song[1]);

                    //store youtube title in registry
                    Reg.regKey_WriteSubkey(keyName, "YtLink", ytTitle + ": " + playLink);

                    //store youtube link only in registry
                    Reg.regKey_WriteSubkey(keyName, "YtUrl", playLink);

                    //we removed played song 
                
                    ListRequestedSongs.Remove(ListRequestedSongs.ElementAt(i));
                    string playReqD = string.Join(Environment.NewLine, ListRequestedSongs);
                   
                    File.WriteAllText(playListRequest, playReqD);
                }

            }
            catch (Exception e)
            {
                string date2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                CLog.LogWriteError("[" + date2 + "] YouTube Error: " + e.ToString() + Environment.NewLine);

            }
        }
        /// <summary>
        /// We start the play next song from request list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void plaReqCKB_Checked(object sender, RoutedEventArgs e)
        {
            LoadRequestedSongs(sender, e);
            if (ListRequestedSongs.Count > 0)
            {
                string[] song = ListRequestedSongs.ElementAt(0).Split('|');
                reload_YT(song[1]);
                counter = YTVideoDuration(song[1]);
                ListRequestedSongs.Remove(ListRequestedSongs.ElementAt(0));

                string playReqD = string.Join(Environment.NewLine, ListRequestedSongs);
                File.WriteAllText(playListRequest, playReqD);
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
        ///  We stop the play next song from request list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playReqCKB_Unchecked(object sender, RoutedEventArgs e)
        {

            timer1.Stop();
            counter = 0;
            playNextCKB.IsEnabled = true;
            playBTN.Content = "Play";
        }
    }

}
