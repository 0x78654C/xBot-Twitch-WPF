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
                catch
                {
                    // MessageBox.Show(e.ToString());
                }
            }


            weatherKey = Reg.regKey_Read(keyName, "WeatherMSG");
            apiKey = Reg.regKey_Read(keyName, "WeatherAPIKey");
            joinedKey = Reg.regKey_Read(keyName, "BotMSG");

            streamOauthKeyTXT.Password = t_streamKey;
            userNameTXT.Text = t_userName;
            weatherAPIKeyTXT.Password = Encryption._decryptData(apiKey);
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
    }
}

