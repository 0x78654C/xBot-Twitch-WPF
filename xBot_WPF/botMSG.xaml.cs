using Core;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace xBot_WPF
{
    /// <summary>
    /// Interaction logic for botMSG.xaml
    /// </summary>
    public partial class botMSG : Window
    {
        //declare variables
        private static string keyName = "xBot";
        private static string botMSGControl;
        readonly static string randomListFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\random_msg.txt";
        private static string randomC;

        //-----------------------------------------

        public botMSG()
        {
            InitializeComponent();
            //Load variable value from registry and display it
            botMSGtxt.Text = Reg.regKey_Read(keyName, "StartMessage");
            rTimetxt.Text = Reg.regKey_Read(keyName, "rTime");
            //--------------------------------------------------------


            //Load checkbox control for start message
            botMSGControl = Reg.regKey_Read(keyName, "botMSGControl");
            if (botMSGControl == "1")
            {
                botMsgCKB.IsChecked = true;
                botMsgCKB.Content = "Display Bot Message on Start: ON";
            }
            else
            {
                botMsgCKB.IsChecked = false;
                botMsgCKB.Content = "Display Bot Message on Start: OFF";
            }
            //--------------------------------------------------------

            //Load checkbox control for random messages
            randomC = Reg.regKey_Read(keyName, "randomC");
            if (randomC == "1")
            {
                randMsgCKB.IsChecked = true;
                randMsgCKB.Content = "Display Random Message : ON";
            }
            else
            {
                randMsgCKB.IsChecked = false;
                randMsgCKB.Content = "Display Random Message : OFF";
            }
            //--------------------------------------------------------

            //loading random list in the listbox for display
            if (File.Exists(randomListFile))
            {
                string[] cList = File.ReadAllLines(randomListFile);
                foreach (var line in cList)
                {
                    if (line.Length > 0)
                    {
                        randomList.Items.Add(line);
                    }
                }
            }
            else
            {
                MessageBox.Show(randomListFile + " file not found!");
            }
            //-----------------------------------------
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
            Reg.regKey_WriteSubkey(keyName, "StartMessage", botMSGtxt.Text);
            MessageBox.Show("Message saved!");
            this.Close();
        }
        /// <summary>
        /// Save control key to 1 if checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void botMsgCKB_Checked(object sender, RoutedEventArgs e)
        {
            botMsgCKB.Content = "Display Bot Message on Start: ON";
            Reg.regKey_WriteSubkey(keyName, "botMSGControl", "1");
        }

        /// <summary>
        /// Save control key to 0 if unchecked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void botMsgCKB_Unchecked(object sender, RoutedEventArgs e)
        {
            botMsgCKB.Content = "Display Bot Message on Start: OFF";
            Reg.regKey_WriteSubkey(keyName, "botMSGControl", "0");
        }

        /// <summary>
        /// Display selected item from listbox in textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void randomList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (randomList.SelectedIndex != -1)
            {
                randomMSGtxt.Text = randomList.SelectedItem.ToString();
            }
        }

        /// <summary>
        /// Set to 0 key control for random message display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void randMsgCKB_Unchecked(object sender, RoutedEventArgs e)
        {
            randMsgCKB.Content = "Display Random Message : OFF";
            Reg.regKey_WriteSubkey(keyName, "randomC", "0");
        }

        /// <summary>
        /// Set to 1 key control for random message display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void randMsgCKB_Checked(object sender, RoutedEventArgs e)
        {
            randMsgCKB.Content = "Display Random Message : ON";
            Reg.regKey_WriteSubkey(keyName, "randomC", "1");
        }

        /// <summary>
        /// Delat message from listbox and randomListFile path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteBTNMSG_Click(object sender, RoutedEventArgs e)
        {
            if (randomList.SelectedIndex != -1 && randomList.SelectedItem.ToString().Contains(randomMSGtxt.Text))
            {
                string rL = randomList.SelectedItem.ToString();
                string[] rList = File.ReadAllLines(randomListFile);
                string rRead = File.ReadAllText(randomListFile);

                foreach (var line in rList)
                {
                    if (line.StartsWith(rL))
                    {
                        rRead = rRead.Replace(line, "");
                    }
                }

                using (var sWriter = new StreamWriter(randomListFile))
                {
                    rRead = Regex.Replace(rRead, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);//remove empty lines
                    sWriter.Write(rRead);
                    sWriter.Close();
                    MessageBox.Show("Message '" + rL + "' deleted!");

                }
                randomList.Items.Remove(randomList.SelectedItem);

                randomMSGtxt.Clear();
            }
            else
            {
                MessageBox.Show("You must select first an item from the listbox to delete!");
            }
        }

        /// <summary>
        /// Store message in listbox and randomListFile path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBTNMSG_Click(object sender, RoutedEventArgs e)
        {
            if (randomMSGtxt.Text.Length > 0)
            {

                randomList.Items.Add(randomMSGtxt.Text);
                File.AppendAllText(randomListFile, randomMSGtxt.Text + Environment.NewLine);
                MessageBox.Show("Message '" + randomMSGtxt.Text + "' added!");
                randomMSGtxt.Clear();
            }
            else
            {
                MessageBox.Show("You must type a message to add for random list!");
            }
        }

        /// <summary>
        /// Store the display time interval(minutes) in registry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rTimeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (rTimetxt.Text.Length > 0)
            {

                Reg.regKey_WriteSubkey(keyName, "rTime", rTimetxt.Text);
                MessageBox.Show("Display interval time is set to " + rTimetxt.Text + " minutes!");
            }
            else
            {
                MessageBox.Show("The Display Time text box must not be empty!");
            }
        }
    }
}
