using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        private static string[] predefinedCMD = { "time", "help", "yt", "weather", "ss", "gl", "playlist", "rsong", "showrequest", "8ball", "!update", "!create", "!delete" };
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

                //Check for empty command text

                if (cmd.Length > 0)
                {

                    //check for predefined commands exists
                    if (!predefinedCMD.Contains(cmd))
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
                                    File.AppendAllText(comFile, "!" + cmd + ":" + msg + Environment.NewLine);
                                    commandList.Items.Add("!" + cmd + ":" + msg);
                                    nameTXT.Clear();
                                    contentTXT.Clear();
                                    MessageBox.Show("Command: '" + cmd + "' with message: '" + msg + "' added to list");

                                }
                                else
                                {
                                    if (commandList.SelectedIndex != -1 && commandList.SelectedItem.ToString().Contains(cmd))
                                    {
                                        string[] cL = commandList.SelectedItem.ToString().Split(':');
                                        foreach (var line in cmd_lineA)
                                        {
                                            //check if command already exists in line
                                            if (line.Contains(cL[0]))
                                            {
                                                cmd_lst = cmd_lst.Replace(line, "!" + cmd + ":" + msg);
                                            }
                                        }

                                        cmd_lst = Regex.Replace(cmd_lst, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);//remove empty lines
                                        File.WriteAllText(comFile, cmd_lst);


                                        commandList.Items.Remove(commandList.SelectedItem);
                                        commandList.Items.Add("!" + cmd + ":" + msg);
                                        contentTXT.Clear();
                                        nameTXT.Clear();
                                        MessageBox.Show("The command '!" + cmd + "' was updated with message: '" + msg + "'");
                                    }


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
                else
                {
                    MessageBox.Show("You must type/select a command");
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
                if (commandList.SelectedIndex != -1)
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
                        cmd_lst = Regex.Replace(cmd_lst, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);//remove empty lines
                        sWriter.Write(cmd_lst);
                        sWriter.Close();
                        nameTXT.Clear();
                        contentTXT.Clear();
                        MessageBox.Show("The command " + cL[0] + " was deleted!");

                    }
                    //we remove item from list
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

        /// <summary>
        /// We update the command name/message textbox with the command name/message from selected command item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commandList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (commandList.SelectedIndex != -1)
            {
                string sItem = commandList.SelectedItem.ToString();
                string[] fItem = sItem.Split('!');
                string[] lItem = fItem[1].Split(':');
                nameTXT.Text = lItem[0]; //we grab the command name
                contentTXT.Text = lItem[1]; //we grab the message content
            }
        }

    }
}
