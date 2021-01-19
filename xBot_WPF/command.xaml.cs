using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Core;

namespace xBot_WPF
{
    /// <summary>
    /// Interaction logic for command.xaml
    /// </summary>
    public partial class command : Window
    {
        readonly static string comFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\command.txt";
        StreamWriter sWriter;
        private static string cmd;
        private static string msg;
        private static string cmd_lst;

        public command()
        {
            InitializeComponent();
            //loading command list in the textbox
            if (File.Exists(comFile))
            {
                listTXT.Text = File.ReadAllText(comFile);
            }
            else
            {
                MessageBox.Show(comFile + " file not found!");
            }
            //-----------------------------------------
        }

        /// <summary>
        /// Drag window funciton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        /// <summary>
        /// minimize label button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miniMizeLBL_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// close label button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeLBL_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// add command button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void adderBTN_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(comFile))
            {
                cmd_lst = File.ReadAllText(comFile);
                cmd = nameTXT.Text;
                msg = contentTXT.Text;
                using (sWriter = new StreamWriter(comFile, append: true))
                {
                    //check if help cmd exists
                    if (!cmd.Contains("help") || !cmd.Contains("yt") || !cmd.Contains("weather") || !cmd.Contains("ss"))
                    {
                        //check for separator oparator to not be present in our message
                        if (!msg.Contains(":"))
                        {
                            //check if command already exists
                            if (!cmd_lst.Contains(cmd))
                            {
                                sWriter.Write(cmd + ":" + msg + Environment.NewLine);
                                sWriter.Close();
                                nameTXT.Clear();
                                contentTXT.Clear();
                                MessageBox.Show("Command: " + cmd + " with message: " + msg + " added to list");
                                listTXT.Text = File.ReadAllText(comFile);
                            }
                            else
                            {
                                MessageBox.Show("The list already contains " + cmd + " command!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("The message should not contain the character ':' !"); ;
                        }
                    }
                    else
                    {
                        MessageBox.Show("You cannot add command " + msg + " because is predifined!"); ;
                    }
                }
            }
            else
            {
                MessageBox.Show(comFile + " file not found!");
            }
        }
        /// <summary>
        /// scroll to end on command list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listTXT_TextChanged(object sender, TextChangedEventArgs e)
        {
            listTXT.ScrollToEnd();
        }
    }
}
