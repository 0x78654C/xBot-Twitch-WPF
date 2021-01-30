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
using Core;
using System.Text.RegularExpressions;

namespace xBot_WPF
{
    /// <summary>
    /// Interaction logic for command.xaml
    /// </summary>
    public partial class command : Window
    {
        //declare global variables
        readonly static string comFile = Directory.GetCurrentDirectory() + @"\data\command.txt";

        StreamWriter sWriter;
        private static string cmd;
        private static string msg;
        private static string cmd_lst;
        private static string[] cmd_line;
        private static string[] cmd_lineA;
        private static string listCMD;
        //----------------------------------

        public command()
        {
            InitializeComponent();
            //loading command list in the textbox
            if (File.Exists(comFile))
            {
                string[] cList = File.ReadAllLines(comFile);
                foreach (var line in cList)
                {
                    if (line.Length > 0)
                    {
                        commandList.Items.Add(line);
                    }
                }
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
                cmd_lineA = File.ReadAllLines(comFile);
                cmd = nameTXT.Text;
                msg = contentTXT.Text;

                //Grab only comands list not the message 
                List<string> lst = new List<string>();
                foreach (var line in cmd_lineA)
                {
                    if (line.Length > 0)
                    {
                        string[] cmdS = line.Split(':');
                        lst.Add(cmdS[0]);
                    }
                }
                listCMD = string.Join("; ", lst);
                //------------------------------------------

                using (sWriter = new StreamWriter(comFile, append: true))
                {
                    //check if help cmd exists
                    if (!cmd.Contains("help") && !cmd.Contains("yt") && !cmd.Contains("weather") && !cmd.Contains("ss") && !cmd.Contains("gl"))
                    {
                        //check for separator oparator to not be present in our message
                        if (!msg.Contains(":"))
                        {
                            if (!cmd.Contains("!") && !cmd.Contains(":"))
                            {
                                //check if command already exists
                                if (!listCMD.Contains(cmd))
                                {
                                    //remove empty lines
                                    cmd_lst = Regex.Replace(cmd_lst, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
                                    sWriter.Write("!" + cmd + ":" + msg + Environment.NewLine);
                                    commandList.Items.Add("!" + cmd + ":" + msg);
                                    sWriter.Close();
                                    nameTXT.Clear();
                                    contentTXT.Clear();
                                    MessageBox.Show("Command: '" + cmd + "' with message: '" + msg + "' added to list");

                                }
                                else
                                {
                                    MessageBox.Show("The list already contains '" + cmd + "' command!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("The message should not contain the symbol ':' or '!' !"); ;
                            }
                        }
                        else
                        {
                            MessageBox.Show("The message should not contain the symbol ':' !"); ;
                        }
                    }
                    else
                    {
                        MessageBox.Show("You cannot add command '" + cmd + "' because is predifined!"); ;
                    }
                }
            }
            else
            {
                MessageBox.Show(comFile + " file not found!");
            }
        }

        /// <summary>
        /// Delete command from list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removerBTN_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(comFile))
            {
                if (commandList.SelectedItem.ToString().Length > 0)
                {
                    string[] cL = commandList.SelectedItem.ToString().Split(':');

                    cmd_line = File.ReadAllLines(comFile);
                    cmd_lst = File.ReadAllText(comFile);
                    cmd = nameTXT.Text;



                    foreach (var line in cmd_line)
                    {
                        //check if command already exists in line
                        if (line.Contains(cL[0]))
                        {

                            cmd_lst = cmd_lst.Replace(line, "");

                        }
                    }



                    using (sWriter = new StreamWriter(comFile))
                    {
                        cmd_lst = Regex.Replace(cmd_lst, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
                        sWriter.Write(cmd_lst);
                        sWriter.Close();
                        nameTXT.Clear();
                        MessageBox.Show("The command "+cL[0]+" was deleted!");

                    }

                    commandList.Items.Remove(commandList.SelectedItem);

                }
                else
                {
                    MessageBox.Show("You need to select a command from list!");
                }
            }
            else
            {
                MessageBox.Show(comFile + " file not found!");
            }
        }
    }
}
