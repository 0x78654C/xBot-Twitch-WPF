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
using Core;
using System.IO;

namespace xBot_WPF
{
    /// <summary>
    /// Interaction logic for botMSG.xaml
    /// </summary>
    public partial class botMSG : Window
    {
        //declare variables
        private static string BotMSGFile=Directory.GetCurrentDirectory()+ @"\data\bot_message.txt";
        //-----------------------------------------

        public botMSG()
        {
            InitializeComponent();
            //check message file and display on textbox
            if (File.Exists(BotMSGFile))
            {
                botMSGtxt.Text = File.ReadAllText(BotMSGFile);

            }
            else
            {
                MessageBox.Show("File "+BotMSGFile+" dose not exist!");
                this.Close();
            }
            //--------------------------------------------------------


        }

        /// <summary>
        /// Close label Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeLBL_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Minimize window stat button label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miniMizeLBL_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// Drag window function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        /// <summary>
        /// Save settings button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveBTNMSG_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(BotMSGFile))
            {
                File.WriteAllText(BotMSGFile, botMSGtxt.Text);
                MessageBox.Show("Message saved!"); 
                this.Close();
            }
            else 
            {
                MessageBox.Show("File " + BotMSGFile + " dose not exist!");
                this.Close();
            }
        }
    }
}
