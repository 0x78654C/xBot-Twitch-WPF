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

namespace xBot_WPF
{
    /// <summary>
    /// Interaction logic for youtube.xaml
    /// </summary>
    public partial class youtube : Window
    {

        //Declare path to youtube link file
        readonly static string ytFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\youtube_link.txt";

        readonly static string ytControl = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\yt_control.txt";
        
        //-----------------------------------------
        public youtube()
        {
            InitializeComponent();
            //load youtube link 
            youtTubeLink.Text = File.ReadAllText(ytFile);
            //--------------------------
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
                using (var sWriter = new StreamWriter(ytControl))
                {
                    sWriter.Write("1");
                }
            }
            catch
            {
                string html1 = "<html><head><center>";
                html1 += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
                html1 += "<iframe id='video' src= ' ' width='647' height='380' frameborder='0' allowfullscreen></iframe>";
                html1 += "</center></body></html>";
                this.ytBrowser.NavigateToString(html1);
                using (var sWriter = new StreamWriter(ytControl))
                {
                    sWriter.Write("0");
                }
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
                reload_YT(youtTubeLink.Text);
                using (var sWriter = new StreamWriter(ytFile))
                {
                    sWriter.Write(youtTubeLink.Text);
                    sWriter.Close();
                }
                playBTN.Content = "Stop";
            }
            else
            {
                string html1 = "<html><head><center>";
                html1 += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
                html1 += "<iframe id='video' src= ' ' width='647' height='380' frameborder='0' allowfullscreen></iframe>";
                html1 += "</center></body></html>";
                this.ytBrowser.NavigateToString(html1);
                using (var sWriter = new StreamWriter(ytControl))
                {
                    sWriter.Write("0");
                }
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
                using (var sWriter = new StreamWriter(ytControl))
            {
                sWriter.Write("0");
            }
        }
    }
}
