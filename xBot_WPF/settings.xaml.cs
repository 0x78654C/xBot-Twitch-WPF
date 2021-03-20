using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

namespace xBot_WPF
{
    /// <summary>
    /// Interaction logic for settings.xaml
    /// </summary>
    public partial class settings : Window
    {
        //Declare global variables
        private static string keyName = "xBot";
        private static string t_userName;
        private static string t_streamKey;
        private static string weatherKey;
        private static string apiKey;
        private static string joinedKey;
        private static string weatherUnits;
        private static string ytRequest;
        readonly static string modFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\mods.txt";
        //---------------------------------------------------------

        public settings()
        {
            InitializeComponent();
            //Load and display username, streamkey and dark mode control from registry
            weatherUnits = Reg.regKey_Read(keyName, "weatherUnits");
            t_userName = Reg.regKey_Read(keyName, "UserName");
            t_streamKey = Reg.regKey_Read(keyName, "StreamKey");
            if (t_streamKey != "")
            {
                try
                {

                    t_streamKey = Encryption._decryptData(Reg.regKey_Read(keyName, "StreamKey"));
                }
                catch(Exception e)
                {
                    CLog.LogWriteError("Settigns - decrypt oAuth Key: " + e.ToString());
                }
            }


            weatherKey = Reg.regKey_Read(keyName, "WeatherMSG");
            apiKey = Reg.regKey_Read(keyName, "WeatherAPIKey");
            joinedKey = Reg.regKey_Read(keyName, "BotMSG");
            ytRequest = Reg.regKey_Read(keyName, "ytRequest");

            streamOauthKeyTXT.Password = t_streamKey;
            userNameTXT.Text = t_userName;
            weatherAPIKeyTXT.Password = Encryption._decryptData(apiKey);
            //---------------------------------

            //load yt song request setting
            if (ytRequest == "1")
            {
                songReqCKB.Content = "Activate YT Song Rquest: ON";
                songReqCKB.IsChecked = true;
            }
            else
            {
                songReqCKB.Content = "Activate YT Song Rquest: OFF";
                songReqCKB.IsChecked = false; 
            }
            //---------------------------------
            
            //load weaheter checkbox msg ar deactivate

            if (weatherKey == "1")
            {
                weatherCKB.Content = "Activate Weather Command: ON";
                weatherCKB.IsChecked = true;
                weaherUnits.IsEnabled = true;
            }
            else if (weatherKey == "0")
            {
                weatherCKB.Content = "Activate Weather Command: OFF";
                weatherCKB.IsChecked = false;
                weaherUnits.IsEnabled = false;
            }
            else if (weatherKey.Length <= 0)
            {
                weatherCKB.IsChecked = false;
                weatherCKB.IsEnabled = false;
            }
            //----------------------------------------


            //load joiend checkbox msg ar deactivate

            if (joinedKey == "1")
            {
                joinedCKB.Content = "Display user joiend chat message: ON";
                joinedCKB.IsChecked = true;
            }
            else if (joinedKey == "0")
            {
                joinedCKB.Content = "Display user joiend chat message: OFF";
                joinedCKB.IsChecked = false;
            }
            else if (joinedKey.Length <= 0)
            {
                joinedCKB.IsChecked = false;
                joinedCKB.IsEnabled = false;
            }
            //----------------------------------------

            //load weather units key control
            if (weatherUnits == "1")
            {
                weaherUnits.SelectedIndex = 0;              
            }
            else
            {
                weaherUnits.SelectedIndex = 1;              
            }
            //----------------------------------------

            //Load mods list in listview
            if (File.Exists(modFile))
            {

                //we add every mod from external file to listview
                string[] bList = File.ReadAllLines(modFile);
                foreach (var line in bList)
                {
                    if (line.Length > 0)
                    {
                        modsListV.Items.Add(line);
                    }
                }
            }
            else
            {
                MessageBox.Show("File " + modFile+ " dose not exist!");
            }
            //-----------------------------------------

        }
        /// <summary>
        /// Drag window with mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        /// <summary>
        /// Close lable button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeLBL_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Minimize lable button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miniMizeLBL_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        // helper to hide watermark hint in password field
        private void passwordChanged_oAuth(object sender, RoutedEventArgs e)
        {
            //todo future work
        }
        /// <summary>
        /// Save settings button    
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveBTN_Click(object sender, RoutedEventArgs e)
        {
            //sotre username in regkey
            Reg.regKey_WriteSubkey(keyName, "UserName", userNameTXT.Text);

            //sotre oauth key in regkey
            if (streamOauthKeyTXT.Password != "")
            {
                Reg.regKey_WriteSubkey(keyName, "StreamKey", Encryption._encryptData(streamOauthKeyTXT.Password));
            }
            else
            {
                
                Reg.regKey_WriteSubkey(keyName, "StreamKey", streamOauthKeyTXT.Password);
            }

            //sotre weather API key in regkey
            if (weatherAPIKeyTXT.Password != "")
            {
                Reg.regKey_WriteSubkey(keyName, "WeatherAPIKey", Encryption._encryptData(weatherAPIKeyTXT.Password));
            }
            else
            {
                Reg.regKey_WriteSubkey(keyName, "WeatherAPIKey", weatherAPIKeyTXT.Password);
            }
            MessageBox.Show("Settings saved!");
            this.Close();
        }

        #region saveing weather keycontrol for main window check 
        private void weatherCKB_Checked(object sender, RoutedEventArgs e)
        {
            //Store the activation command control number
            Reg.regKey_WriteSubkey(keyName, "WeatherMSG", "1");

            //change the label content to on
            weatherCKB.Content = "Activate Weather Command: ON";
            weaherUnits.IsEnabled = true;
        }

        private void weatherCKB_Unchecked(object sender, RoutedEventArgs e)
        {
            //store the deactivation command control number
            Reg.regKey_WriteSubkey(keyName, "WeatherMSG", "0");

            //change the label content to off
            weatherCKB.Content = "Activate Weather Command: OFF";
            weaherUnits.IsEnabled = false ;
        }
        #endregion

        #region saveing user joined keycontrol for main window check 
        private void joinedCKB_Checked(object sender, RoutedEventArgs e)
        {
            //store the OnJoined event bot display message control number for activation
            Reg.regKey_WriteSubkey(keyName, "BotMSG", "1");

            //change the label content to on
            joinedCKB.Content = "Display user joiend chat message: ON";
        }

        private void joinedCKB_Unchecked(object sender, RoutedEventArgs e)
        {
            //store the OnJoined event bot display message control number for deactivation
            Reg.regKey_WriteSubkey(keyName, "BotMSG", "0");

            //change the label content to on
            joinedCKB.Content = "Display user joiend chat message: OFF";
        }
        #endregion

        #region saveing yt song request settings
        private void songReqCKB_Checked(object sender, RoutedEventArgs e)
        {
        
            Reg.regKey_WriteSubkey(keyName, "ytRequest", "1");
            songReqCKB.Content = "Activate YT Song Rquest: ON";
        }

        private void songReqCKB_Unchecked(object sender, RoutedEventArgs e)
        {
            
            Reg.regKey_WriteSubkey(keyName, "ytRequest", "0");
            songReqCKB.Content = "Activate YT Song Rquest: OFF";
        }
        #endregion
        /// <summary>
        /// Write key control by unit case
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weaherUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (weaherUnits.SelectedItem.ToString().Contains("Celsius")) 
            {
                Reg.regKey_WriteSubkey(keyName, "weatherUnits", "1");
            }
            else
            {
                Reg.regKey_WriteSubkey(keyName, "weatherUnits", "0");
            }
        }

        /// <summary>
        /// Add mods in external list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void addModBTN_Click(object sender, RoutedEventArgs e)
        {

            if (File.Exists(modFile))
            {

                if (modNameTXT.Text.Length > 0)
                {

                    //   badWrodTXT.Text = Regex.Replace(badWrodTXT.Text, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline); //remove new line and emty line

                    File.AppendAllText(modFile, modNameTXT.Text + Environment.NewLine);
                    modsListV.Items.Add(modNameTXT.Text);
                    modNameTXT.Clear();

                }
                else
                {
                    MessageBox.Show("You must fill the empy text box!");
                }
            }
            else
            {
                MessageBox.Show("File " +modFile + " dose not exist!");
            }
        }

        /// <summary>
        /// Remove mod from external list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delModBTN_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(modFile))
            {
                string line;
                string bFile = File.ReadAllText(modFile);
                if (modsListV.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a Moderator username for delete!");
                }
                else
                {

                    using (var sr = new StringReader(modsListV.SelectedItem.ToString()))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            bFile = bFile.Replace(line, "");
                        }

                        modsListV.Items.Remove(modsListV.SelectedItem.ToString());
                        bFile = Regex.Replace(bFile, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);//remove empty lines
                        File.WriteAllText(modFile, bFile);

                    }
                }
            }
            else
            {
                MessageBox.Show("File " + modFile + " dose not exist!");
            }
        }
    }
}

