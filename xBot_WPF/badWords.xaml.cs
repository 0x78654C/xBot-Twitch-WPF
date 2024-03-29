﻿using Core;
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
    /// Interaction logic for badWords.xaml
    /// </summary>
    public partial class badWords : Window
    {
        //Declare variables
        readonly static string badWordDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\badwords.txt";
        readonly static string keyName = "xBot";
        private static string banActivate;
        private static string date;
        //------------------------------------------
        public badWords()
        {
            InitializeComponent();

            //read data from registry
            banTimeTXT.Text = Reg.regKey_Read(keyName, "WordBanTime");
            banActivate = Reg.regKey_Read(keyName, "BadWord");
            //---------------------------------------

            //Load bad words list in listview
            if (File.Exists(badWordDir))
            {

                //we add every word from external file to listbox
                string[] bList = File.ReadAllLines(badWordDir);
                foreach (var line in bList)
                {
                    if (line.Length > 0)
                    {
                        wordList.Items.Add(line);
                    }
                }
            }
            else
            {
                MessageBox.Show("File " + badWordDir + " dose not exist!");
                this.Close();
            }
            //-----------------------------------------

            //check if bad word ban is activated and modify the settings
            if (banActivate == "1")
            {
                banWordCKB.IsChecked = true;
                banUserCKB.IsChecked = false;
                banUserCKB.IsEnabled = false;
                banTimeTXT.IsEnabled = true;
                banWordCKB.Content = "Activate Chat Ban: ON";
                banUserCKB.Content = "Activate User Ban: OFF";

            }
            else if (banActivate == "2")
            {
                banWordCKB.IsChecked = false;
                banUserCKB.IsChecked = true;
                banWordCKB.IsEnabled = false;
                banTimeTXT.IsEnabled = false;
                banWordCKB.Content = "Activate Chat Ban: OFF";
                banUserCKB.Content = "Activate Ban User: ON";
            }
            else
            {
                banWordCKB.IsChecked = false;
                banUserCKB.IsChecked = false;
                banTimeTXT.IsEnabled = true;
                banWordCKB.Content = "Activate Chat Ban: OFF";
                banUserCKB.Content = "Activate User Ban: OFF";
            }
            //---------------------------------------------
        }

        /// <summary>
        /// Minimize label button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miniMizeLBL_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Close label button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeLBL_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Save bad words in file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void adderBTN_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(badWordDir))
            {

                if (badWrodTXT.Text.Length > 0)
                {

                    badWrodTXT.Text = Regex.Replace(badWrodTXT.Text, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline); //remove new line and emty line
                    string line;
                    using (var sr = new StringReader(badWrodTXT.Text))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            File.AppendAllText(badWordDir, line + Environment.NewLine);
                            wordList.Items.Add(line);
                            badWrodTXT.Clear();
                        }

                    }
                }
                else
                {
                    MessageBox.Show("You must fill the empy text box!");
                }
            }
            else
            {
                MessageBox.Show("File " + badWordDir + " dose not exist!");
                this.Close();
            }
        }

        /// <summary>
        /// Drag window on mouse down event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        /// <summary>
        /// Write 1 for chat ban activation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void banWordCKB_Checked(object sender, RoutedEventArgs e)
        {
            banWordCKB.Content = "Activate Chat Ban: ON";
            Reg.regKey_WriteSubkey(keyName, "BadWord", "1");
            date = DateTime.Now.ToString("yyyy MM dd HH:mm:ss");
            CLog.LogWrite("[" + date + "]" + "Bad words check activated!");
            banUserCKB.IsEnabled = false;
            banUserCKB.IsChecked = false;
        }

        /// <summary>
        /// Write 0  for chat ban deactivation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void banWordCKB_Unchecked(object sender, RoutedEventArgs e)
        {
            banWordCKB.Content = "Activate Chat Ban: OFF";
            Reg.regKey_WriteSubkey(keyName, "BadWord", "0");
            date = DateTime.Now.ToString("yyyy MM dd HH:mm:ss");
            CLog.LogWrite("[" + date + "]" + "Bad words check deactivated!");
            banUserCKB.IsChecked = false;
            banUserCKB.IsEnabled = true;
        }
        /// <summary>
        /// Write automaticly the ban time in registry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void banTimeTXT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (banTimeTXT.Text.Length > 0)
            {
                Reg.regKey_WriteSubkey(keyName, "WordBanTime", banTimeTXT.Text);
            }
        }

        /// <summary>
        /// Only number function for ban textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// remove bad word function button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removerBTN_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(badWordDir))
            {
                string line;
                string bFile = File.ReadAllText(badWordDir);
                if (wordList.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a word for delete!");
                }
                else
                {

                    using (var sr = new StringReader(wordList.SelectedItem.ToString()))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            bFile = bFile.Replace(line, "");
                        }

                        wordList.Items.Remove(wordList.SelectedItem.ToString());
                        bFile = Regex.Replace(bFile, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);//remove empty lines
                        File.WriteAllText(badWordDir, bFile);

                    }
                }
            }
            else
            {
                MessageBox.Show("File " + badWordDir + " dose not exist!");
                this.Close();
            }
        }

        /// <summary>
        /// Writes 2 in registry for ban user activation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void banUserCKB_Checked(object sender, RoutedEventArgs e)
        {
            banUserCKB.Content = "Activate User Ban: ON";
            Reg.regKey_WriteSubkey(keyName, "BadWord", "2");
            date = DateTime.Now.ToString("yyyy MM dd HH:mm:ss");
            CLog.LogWrite("[" + date + "]" + "Bad words/spam check activated!");
            banWordCKB.IsEnabled = false;
            banWordCKB.IsChecked = false;
            banTimeTXT.IsEnabled = false;
        }

        /// <summary>
        /// Writes 0 in registry for ban user deactivation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void banUserCKB_Unchecked(object sender, RoutedEventArgs e)
        {
            banUserCKB.Content = "Activate User Ban: OFF";
            Reg.regKey_WriteSubkey(keyName, "BadWord", "0");
            date = DateTime.Now.ToString("yyyy MM dd HH:mm:ss");
            CLog.LogWrite("[" + date + "]" + "Bad words/spam check deactivated!");
            banWordCKB.IsChecked = false;
            banWordCKB.IsEnabled = true;
            banTimeTXT.IsEnabled = true;
        }
    }
}
