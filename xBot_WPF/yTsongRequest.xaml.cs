using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace xBot_WPF
{
    /// <summary>
    /// Interaction logic for yTsongRequest.xaml
    /// </summary>
    public partial class yTsongRequest : Window
    {
        //delclare the path to playlist with requested songs from viewers
        readonly static string playListRequest = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\playListRequest.txt";
        WebClient wClient;
        //-------------------------------
        public yTsongRequest()
        {
            InitializeComponent();

            //loading the youtube links to file

            if (File.Exists(playListRequest))
            {
                int index = 0;
                string[] requestList = File.ReadAllLines(playListRequest);
                wClient = new WebClient();
                foreach (var line in requestList)
                {
                    if (line.Length > 0)
                    {
                        index++;
                        string[] s = line.Split('|');
                        //Download youtube link source code
                        string titleParse = wClient.DownloadString(s[1]);

                        //grabing the title from source
                        int pFrom = titleParse.IndexOf("<title>") + "<title>".Length;
                        int pTo = titleParse.LastIndexOf("</title>");

                        //store the title
                        string ytTitle = titleParse.Substring(pFrom, pTo - pFrom);
                        rPlayList.Items.Add(index.ToString() + " | " + ytTitle + " | " + s[1]);
                    }
                }
                index = 0;
            }

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
        /// Drag window function event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

    }
}
