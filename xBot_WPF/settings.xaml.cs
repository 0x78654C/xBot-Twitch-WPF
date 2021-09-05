using Core;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ModeratorManagement = xBot_WPF.Settings.ModeratorManagement;

namespace xBot_WPF
{
    /// <summary>
    /// Interaction logic for settings.xaml
    /// </summary>
    public partial class settings : Window
    {
        // Declare global variables
        private readonly static string s_keyName = "xBot";
        private static string s_twithcUserName;
        private static string s_streamKey;
        private static string s_weatherKey;
        private static string s_apiKey;
        private static string s_joinedKey;
        private static string s_weatherUnits;
        private static string s_youtubeRequest;
        readonly static string s_modFile = Directory.GetCurrentDirectory() + @"\data\mods.txt";
        //---------------------------------------------------------

        public settings()
        {
            InitializeComponent();

            ReadRegistryKeyVariables();
            LoadSettings();
            ModeratorManagement.LoadModerators(s_modFile,modsListV);
        }

        // Load data from Registry.
        private void ReadRegistryKeyVariables()
        {
            s_weatherUnits = Reg.regKey_Read(s_keyName, "weatherUnits");
            s_twithcUserName = Reg.regKey_Read(s_keyName, "UserName");
            s_streamKey = Reg.regKey_Read(s_keyName, "StreamKey");
            if (s_streamKey != "")
            {
                try
                {

                    s_streamKey = Encryption._decryptData(Reg.regKey_Read(s_keyName, "StreamKey"));
                }
                catch (Exception e)
                {
                    CLog.LogWriteError("Settigns - decrypt oAuth Key: " + e.ToString());
                }
            }

            s_weatherKey = Reg.regKey_Read(s_keyName, "WeatherMSG");
            s_apiKey = Reg.regKey_Read(s_keyName, "WeatherAPIKey");
            s_joinedKey = Reg.regKey_Read(s_keyName, "BotMSG");
            s_youtubeRequest = Reg.regKey_Read(s_keyName, "ytRequest");

            streamOauthKeyTXT.Password = s_streamKey;
            userNameTXT.Text = s_twithcUserName;
            weatherAPIKeyTXT.Password = Encryption._decryptData(s_apiKey);
        }

        // Load settings and apply.
        private void LoadSettings()
        {

            //load yt song request setting
            if (s_youtubeRequest == "1")
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

            if (s_weatherKey == "1")
            {
                weatherCKB.Content = "Activate Weather Command: ON";
                weatherCKB.IsChecked = true;
                weaherUnits.IsEnabled = true;
            }
            else if (s_weatherKey == "0")
            {
                weatherCKB.Content = "Activate Weather Command: OFF";
                weatherCKB.IsChecked = false;
                weaherUnits.IsEnabled = false;
            }
            else if (s_weatherKey.Length <= 0)
            {
                weatherCKB.IsChecked = false;
                weatherCKB.IsEnabled = false;
            }
            //----------------------------------------


            //load joiend checkbox msg ar deactivate

            if (s_joinedKey == "1")
            {
                JoinedUserCheckBox.Content = "Display user joiend chat message: ON";
                JoinedUserCheckBox.IsChecked = true;
            }
            else if (s_joinedKey == "0")
            {
                JoinedUserCheckBox.Content = "Display user joiend chat message: OFF";
                JoinedUserCheckBox.IsChecked = false;
            }
            else if (s_joinedKey.Length <= 0)
            {
                JoinedUserCheckBox.IsChecked = false;
                JoinedUserCheckBox.IsEnabled = false;
            }
            //----------------------------------------

            //load weather units key control
            if (s_weatherUnits == "1")
            {
                weaherUnits.SelectedIndex = 0;
            }
            else
            {
                weaherUnits.SelectedIndex = 1;
            }
            //----------------------------------------

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
            Reg.regKey_WriteSubkey(s_keyName, "UserName", userNameTXT.Text);

            //sotre oauth key in regkey
            if (streamOauthKeyTXT.Password != "")
            {
                Reg.regKey_WriteSubkey(s_keyName, "StreamKey", Encryption._encryptData(streamOauthKeyTXT.Password));
            }
            else
            {

                Reg.regKey_WriteSubkey(s_keyName, "StreamKey", streamOauthKeyTXT.Password);
            }

            //sotre weather API key in regkey
            if (weatherAPIKeyTXT.Password != "")
            {
                Reg.regKey_WriteSubkey(s_keyName, "WeatherAPIKey", Encryption._encryptData(weatherAPIKeyTXT.Password));
            }
            else
            {
                Reg.regKey_WriteSubkey(s_keyName, "WeatherAPIKey", weatherAPIKeyTXT.Password);
            }
            MessageBox.Show("Settings saved!");
            this.Close();
        }

        #region saveing weather keycontrol for main window check 
        private void weatherCKB_Checked(object sender, RoutedEventArgs e)
        {
            //Store the activation command control number
            Reg.regKey_WriteSubkey(s_keyName, "WeatherMSG", "1");

            //change the label content to on
            weatherCKB.Content = "Activate Weather Command: ON";
            weaherUnits.IsEnabled = true;
        }

        private void weatherCKB_Unchecked(object sender, RoutedEventArgs e)
        {
            //store the deactivation command control number
            Reg.regKey_WriteSubkey(s_keyName, "WeatherMSG", "0");

            //change the label content to off
            weatherCKB.Content = "Activate Weather Command: OFF";
            weaherUnits.IsEnabled = false;
        }
        #endregion

        #region saveing user joined keycontrol for main window check 
        private void JoinedUserCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //store the OnJoined event bot display message control number for activation
            Reg.regKey_WriteSubkey(s_keyName, "BotMSG", "1");

            //change the label content to on
            JoinedUserCheckBox.Content = "Display user joiend chat message: ON";
        }

        private void JoinedUserCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //store the OnJoined event bot display message control number for deactivation
            Reg.regKey_WriteSubkey(s_keyName, "BotMSG", "0");

            //change the label content to on
            JoinedUserCheckBox.Content = "Display user joiend chat message: OFF";
        }
        #endregion

        #region saveing yt song request settings
        private void SongRequestCheckBox_Checked(object sender, RoutedEventArgs e)
        {

            Reg.regKey_WriteSubkey(s_keyName, "ytRequest", "1");
            songReqCKB.Content = "Activate YT Song Rquest: ON";
        }

        private void SongRequestCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

            Reg.regKey_WriteSubkey(s_keyName, "ytRequest", "0");
            songReqCKB.Content = "Activate YT Song Rquest: OFF";
        }
        #endregion
        /// <summary>
        /// Write key control by unit case
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeaherUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (weaherUnits.SelectedItem.ToString().Contains("Celsius"))
            {
                Reg.regKey_WriteSubkey(s_keyName, "weatherUnits", "1");
            }
            else
            {
                Reg.regKey_WriteSubkey(s_keyName, "weatherUnits", "0");
            }
        }

        /// <summary>
        /// Add mods in external list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void AddModeratorButton_Click(object sender, RoutedEventArgs e)
        {
            ModeratorManagement.AddModeratorToList(s_modFile, modNameTXT, modsListV);
        }

        /// <summary>
        /// Remove mod from external list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteModeratorButton_Click(object sender, RoutedEventArgs e)
        {
            ModeratorManagement.RemoveSelectedModerator(s_modFile, modsListV);
        }
    }
}

