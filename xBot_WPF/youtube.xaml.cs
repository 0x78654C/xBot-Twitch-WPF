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

namespace xBot_WPF
{
    /// <summary>
    /// Interaction logic for youtube.xaml
    /// </summary>
    public partial class youtube : Window
    {

        //Declare path to youtube link file
        private static string ytControl = "0";
        MatchCollection matches;
        //-----------------------------------------

        //Declare keyname
        private static string keyName = "xBot";
        //----------------------------------------

        public youtube()
        {
            InitializeComponent();

            //load youtube link 
            youtTubeLink.Text = Reg.regKey_Read(keyName, "YtLink");
            //--------------------------

            //Load control number from registry
            ytControl = Reg.regKey_Read(keyName, "YTControl");
            //---------------------------

            //Set control number for window status
            Reg.regKey_WriteSubkey(keyName, "YtWin", "1");
            //---------------------------

        }

        /// <summary>
        /// youtube reload link on browser
        /// </summary>
        /// <param name="url"></param>
        public void reload_YT(string url)
        {
            string html = "<html><head><center>";
            html += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
            html += "<iframe id='video' src= 'https://www.youtube.com/embed/{0}?rel=0&amp;&amp;showinfo=0;&autoplay=1' width='647' height='380' frameborder='0' allowfullscreen></iframe>";
            html += "</center></body></html>";
            try
            {
                this.ytBrowser.NavigateToString(string.Format(html, url.Split('=')[1]));
                Reg.regKey_WriteSubkey(keyName, "YTControl", "1");
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
                //we check if continas the '=' symbol for check and after cont it 
                matches = Regex.Matches(youtTubeLink.Text, "=");

                if (youtTubeLink.Text.Contains("youtube.") && matches.Count == 1)
                {
                    reload_YT(youtTubeLink.Text);
                    Reg.regKey_WriteSubkey(keyName, "YtLink", youtTubeLink.Text);
                    playBTN.Content = "Stop";
                }
                else
                {
                    MessageBox.Show("You have typed a invalid YouTube link!");
                }
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
    }
}
