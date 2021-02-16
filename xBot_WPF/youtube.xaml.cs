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

            //load links from playslistfile in listbox
            string[] yLinks = File.ReadAllLines(playListFile);
            foreach (var line in yLinks)
            {
                if (line.Length > 0)
                {
                    playList.Items.Add(line);
                }
            }
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
            html += "<iframe id='video' src= 'https://www.youtube.com/embed/{0}?rel=0&amp;&amp;showinfo=0;&autoplay=1' width='647' height='380' frameborder='0' allowfullscreen></iframe>";
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
            
            if (playBTN.Content.ToString() == "Play")
            {
                reload_YT(playLink);
                playBTN.Content = "Stop";
            }
            else
            {
                string html1 = "<html><head><center>";
                html1 += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
                html1 += "<iframe id='video' src= ' ' width='647' height='380' frameborder='0' allowfullscreen></iframe>";
                html1 += "</center></body></html>";
                this.ytBrowser.NavigateToString(html1);
                Reg.regKey_WriteSubkey(keyName, "YTControl", "0");
                playBTN.Content = "Play";
                
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
    }

}
