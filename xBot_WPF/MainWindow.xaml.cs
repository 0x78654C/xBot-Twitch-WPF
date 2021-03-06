﻿using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Users;
using TwitchLib.Api.V5.Models.Subscriptions;
using System.IO;
using System.Reflection;
using Core;
using Microsoft.Win32;
using System.Threading;
using System.Net.Http;
using System.ComponentModel;
using System.Net;

namespace xBot_WPF
{
    /*
       Description: This a simple Twitch bot created with passion and fun entirely live on stream.
       Is based on https://github.com/TwitchLib/TwitchLib 


       This app is distributed under the MIT License.
       Copyright © 2021 0x78654C. All rights reserved.

       THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
       IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
       FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL THE
       AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
       LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
       OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
       SOFTWARE.
     */
    
    public partial class MainWindow : Window
    {
     
        //declare twitch client variable
        TwitchClient client = new TwitchClient();
        //------------------------------------------------

        //data and log directory declare
        readonly static string dataDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data";
        readonly static string logDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\log";
        readonly static string logErrorDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\log\errors";
        //------------------------------------------------


        //Declare keyname and subkey
        readonly static string keyName = "xBot";
        private static string weatherKey = "0";
        private static string botMSGKey = "0";
        private static string botMSGControl = "0";
        private static string weatherUnits = "0";
        private static string menuStatus = "0";
        //------------------------------------------------

        //declare twitch credential info
        private static string t_userName;
        private static string t_streamKey;
        //-------------------------------------------------


        //command file variables
        readonly static string comDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\command.txt";
        private static string[] comandList;
        //-------------------------------------------------


        //bad word lists path declaration and variable
        readonly static string badWordDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\badwords.txt";
        private static string[] badWordList;
        private static string bWord = "0";
        int timeBan = 0;
        //-------------------------------------------------

        //date variable declar
        private static string date;
        //-------------------------------------------------

        //declare path to bot message
        private static string StartMessage;
        //-------------------------------------------------

        //Media player declaration
        readonly static string playListFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\playList.txt";
        private static string YtLink;
        private static string ytControl = "0";
        private static string YtWin = "0";
        private static string ytRequest = "0";
        readonly static string playListRequest = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\playListRequest.txt";
        List<string> cList = new List<string>();
        List<string> pList = new List<string>();
        List<string> qList = new List<string>();
        //-------------------------------------------------


        //declare weather variables
        private static string apiKey = string.Empty;
        private static string weatheCond = string.Empty;
        static readonly HttpClient clientH = new HttpClient();
        //--------------------------------------------------

        //declare the bot forms variables
        settings sT;
        botMSG bM;
        command cmD;
        badWords bW;
        youtube yT;
        about aB;
        //--------------------------------

        //Define the background worker for bot start and random message
        BackgroundWorker worker;
        //--------------------------------

        //Declare mutex variable for startup instance check
        Mutex myMutex;
        //---------------------------------


        //declare timer for load icons and variables read value
        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        //--------------------------------

        int Viewers = 0;
        int SubsCount = 0;

        //declare variables for random message system
        readonly static string randomListFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\random_msg.txt";
        private static string randomC;
        System.Windows.Threading.DispatcherTimer dispatcherTimerR;
        string[] rand_list;
        private static string rTime;
        Random r = new Random();
        //--------------------------------

        //declare variables for mod section
        readonly static string modFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\mods.txt";
        string[] mods = null;
        //--------------------------------

        //declare variables for Magic 8ball game
        readonly static string ballAnswer = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\8ball_answers.txt";
        Random r8 = new Random();
        //--------------------------------


        public MainWindow()
        {
            InitializeComponent();

            //aplication startup instance check
            Application_Startup();
            //---------------------------------------------
            //Bot start message
            this.Dispatcher.Invoke(() =>
            {
                logViewRTB.Document.Blocks.Clear();
            });
            logWrite("Welcome to xBot! The Twitch bot that was entirely build on live stream :D");
            //----------------------------------------------


            //data directory check and create if not exists
            if (!File.Exists(modFile))
            {
              File.Create(modFile);
            }
            //------------------------------------------------

            //data directory check and create if not exists
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }
            //------------------------------------------------


            //log/error directory check and create if not exists
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);

            }

            if (!Directory.Exists(logErrorDirectory))
            {
                Directory.CreateDirectory(logErrorDirectory);
            }
            //------------------------------------------------

            //check if badwords file exits and if not we recreate
            if (!File.Exists(badWordDir))
            {
                File.WriteAllText(badWordDir, "");
            }
            //---------------------------------------------------

            //check if comands file exits and if not we recreate
            if (!File.Exists(comDirectory))
            {
                File.WriteAllText(comDirectory, "");
            }
            //---------------------------------------------------


            //check if youtube playlist file exits and if not we recreate
            if (!File.Exists(playListFile))
            {
                File.WriteAllText(playListFile, "");
            }
            //---------------------------------------------------

            //check if youtube playlist file exits and if not we recreate
            if (!File.Exists(playListRequest))
            {
                File.WriteAllText(playListRequest, "");
            }
            //---------------------------------------------------

            //check if random list file exits and if not we recreate
            if (!File.Exists(randomListFile))
            {
                File.WriteAllText(randomListFile, "");
            }
            //---------------------------------------------------

            //8Ball system file autocreate
            if (!File.Exists(ballAnswer))
            {
                File.Create(ballAnswer);
            }
            //------------------------------------------------

     
            //Checking if reg keys and subkeys exist and if not we recreate

            if (Reg.regKey_Read(keyName, "UserName") == "")
            {
                Reg.regKey_CreateKey(keyName, "UserName", "");
            }

            if (Reg.regKey_Read(keyName, "StreamKey") == "")
            {
                Reg.regKey_CreateKey(keyName, "StreamKey", "");
            }


            if (Reg.regKey_Read(keyName, "WeatherAPIKey") == "")
            {
                Reg.regKey_CreateKey(keyName, "WeatherAPIKey", "");
            }

            if (Reg.regKey_Read(keyName, "WeatherMSG") == "")
            {
                Reg.regKey_CreateKey(keyName, "WeatherMSG", "0");
            }

            if (Reg.regKey_Read(keyName, "BotMSG") == "")
            {
                Reg.regKey_CreateKey(keyName, "BotMSG", "0");
            }

            if (Reg.regKey_Read(keyName, "BadWord") == "")
            {
                Reg.regKey_CreateKey(keyName, "BadWord", "0");
            }

            if (Reg.regKey_Read(keyName, "WordBanTime") == "")
            {
                Reg.regKey_CreateKey(keyName, "WordBanTime", "0");
            }

            if (Reg.regKey_Read(keyName, "YTControl") == "")
            {
                Reg.regKey_CreateKey(keyName, "YTControl", "0");
            }

            if (Reg.regKey_Read(keyName, "YtWin") == "")
            {
                Reg.regKey_CreateKey(keyName, "YtWin", "0");
            }

            if (Reg.regKey_Read(keyName, "StartMessage") == "")
            {
                Reg.regKey_CreateKey(keyName, "StartMessage", " ");
            }

            if (Reg.regKey_Read(keyName, "YtLink") == "")
            {
                Reg.regKey_CreateKey(keyName, "YtLink", " ");
            }

            if (Reg.regKey_Read(keyName, "YtUrl") == "")
            {
                Reg.regKey_CreateKey(keyName, "YtUrl", " ");
            }

            if (Reg.regKey_Read(keyName, "botMSGControl") == "")
            {
                Reg.regKey_CreateKey(keyName, "botMSGControl", "0");
            }

            if (Reg.regKey_Read(keyName, "weatherUnits") == "")
            {
                Reg.regKey_CreateKey(keyName, "weatherUnits", "0");
            }

            if (Reg.regKey_Read(keyName, "Menu") == "")
            {
                Reg.regKey_CreateKey(keyName, "Menu", "0");
            }

            if (Reg.regKey_Read(keyName, "randomC") == "")
            {
                Reg.regKey_CreateKey(keyName, "randomC", "0");
            }

            if (Reg.regKey_Read(keyName, "rTime") == "")
            {
                Reg.regKey_CreateKey(keyName, "rTime", "0");
            }

            if (Reg.regKey_Read(keyName, "ytRequest") == "")
            {
                Reg.regKey_CreateKey(keyName, "ytRequest", "0");
            }
            
            //-----------------------------------------


            #region Load and display username, streamkey and dark mode control from registry

            t_userName = Reg.regKey_Read(keyName, "UserName");

            try
            {

                t_streamKey = Encryption._decryptData(Reg.regKey_Read(keyName, "StreamKey"));
            }
            catch (Exception x)
            {
                CLog.LogWriteError("oAuth decrypt error: " + x.ToString());
                t_streamKey = "error_key";
            }


            botMSGKey = Reg.regKey_Read(keyName, "BotMSG");
            timeBan = Int32.Parse(Reg.regKey_Read(keyName, "WordBanTime"));
            bWord = Reg.regKey_Read(keyName, "BadWord");
            apiKey = Reg.regKey_Read(keyName, "WeatherAPIKey");
            ytControl = Reg.regKey_Read(keyName, "YTControl");
            weatherKey = Reg.regKey_Read(keyName, "WeatherMSG");
            YtWin = Reg.regKey_Read(keyName, "YtWin");
            StartMessage = Reg.regKey_Read(keyName, "StartMessage");
            YtLink = Reg.regKey_Read(keyName, "YtLink");
            botMSGControl = Reg.regKey_Read(keyName, "botMSGControl");
            weatherUnits = Reg.regKey_Read(keyName, "weatherUnits");
            menuStatus = Reg.regKey_Read(keyName, "Menu");
            randomC = Reg.regKey_Read(keyName, "randomC");
            rTime = Reg.regKey_Read(keyName, "rTime");
            ytRequest = Reg.regKey_Read(keyName, "ytRequest");
            #endregion


            //Menu state check and apply
            if (menuStatus == "1")
            {
                GridMenu.Width = 199;
                btnOpenMenu.Visibility = Visibility.Collapsed;
                btnCloseMenu.Visibility = Visibility.Visible;
                startBotBTN.Visibility = Visibility.Visible;
                logViewRTB.Margin = new Thickness(199, 50, 0, 0);

            }
            else
            {
                GridMenu.Width = 50;
                logViewRTB.Margin = new Thickness(50, 50, 0, 0);
                btnOpenMenu.Visibility = Visibility.Visible;
                btnCloseMenu.Visibility = Visibility.Collapsed;
                startBotBTN.Visibility = Visibility.Hidden;

            }
            //---------------------------------


            //staus check timer declaration
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += StatusLoadIcon;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            //----------------------------------

            //timer declaration for random message send
            dispatcherTimerR = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimerR.Tick += randomMessage;
            if (Convert.ToInt32(rTime) > 0)
            {
                // logWrite(Convert.ToInt32(rTime).ToString());
                dispatcherTimerR.Interval = new TimeSpan(0, Convert.ToInt32(rTime), 0);
            }
            else
            {
                dispatcherTimerR.Interval = new TimeSpan(0, 10, 0);
            }
            dispatcherTimerR.Stop();
            //----------------------------------

            //reset Youtube Controler/request song and window on bot start in case of cras
            Reg.regKey_WriteSubkey(keyName, "YTControl", "0");
            Reg.regKey_WriteSubkey(keyName, "YtWin", "0");
           // Reg.regKey_WriteSubkey(keyName, "ytRequest", "0");
            //-----------------------------------

            //load connection icon
            this.Dispatcher.Invoke(() =>
            {
                statIMG.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/red_dot.png"));
            });
            //---------------------------------

            //load playlist/ and qlist and titles for song request command
            playListLoad();
            queueListLoad();
            //---------------------------------

            //read mods from file
            if (File.Exists(modFile))
            {
                mods = File.ReadAllLines(modFile);
            }
            //-----------------------------------

        }


        /// <summary>
        /// Check aplication start instace and close if is already opened
        /// </summary>
        private void Application_Startup()
        {
            bool aIsNewInstance = false;
            myMutex = new Mutex(true, "xBot", out aIsNewInstance);
            if (!aIsNewInstance)
            {
                System.Windows.Forms.MessageBox.Show("xBot is already running...");
                App.Current.Shutdown();
            }
        }



        /// <summary>
        /// Log writer
        /// </summary>
        /// <param name="data"></param>
        private void logWrite(string data)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (!string.IsNullOrEmpty(data))
                {
                    //outs += data + Environment.NewLine;
                    logViewRTB.Document.Blocks.Add(new Paragraph(new Run(data)));
                    logViewRTB.ScrollToEnd();
                }
            });

        }


        /// <summary>
        /// Bot Start function
        /// </summary>
        public void BotStart(object sender, DoWorkEventArgs e)
        {

            this.Dispatcher.Invoke(() =>
            {
                statIMG.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/orange_dot.png"));

                logViewRTB.Document.Blocks.Clear();
            });

            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            logWrite("[" + date + "] xBot connecting to " + t_userName + " channel....");

            ConnectionCredentials credentials = new ConnectionCredentials(t_userName, t_streamKey, null, true);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, t_userName);
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnNewSubscriber += Client_OnNewSubscriber;
            client.OnConnected += Client_OnConnected;
            client.OnUserJoined += Client_OnUserJoinedArgs;
            client.OnUserLeft += Client_OnUserLeftArgs;
            client.OnRaidNotification += Client_OnRaidNotificationArgs;
            client.OnBeingHosted += Client_OnBeingHosted; //enabled for test only
            client.AutoReListenOnException = true;
            client.Connect();
        
            //we check if bot is connected and display the log info
            if (client.IsConnected)
            {
                logWrite("[" + date + "] xBot Connected to " + t_userName + " channel !");
                Thread.Sleep(1);
                this.Dispatcher.Invoke(() =>
                {
                    statIMG.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/green_dot.png"));

                    startBotBTN.Content = "STOP";
                });

            }

        }

        /// <summary>
        /// Bot Stop function
        /// </summary>
        private void BotStop()
        {
            if (client.IsConnected)
            {
                client.Disconnect();
                dispatcherTimer.Stop();
                dispatcherTimerR.Stop();
                this.Dispatcher.Invoke(() =>
                {
                    statIMG.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/red_dot.png"));
                });
            }
            if (!client.IsConnected)
            {
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                logWrite("[" + date + "] xBot Disconncted!");
                CLog.LogWrite("[" + date + "] xBot Disconncted!");
                startBotBTN.Content = "START";

                //reset viewers/new subs counter
                Viewers = 0;
                viewersLbL.Content = "0";

                SubsCount = 0;
                subsLbL.Content = "0";
                //---------------------
            }
        }


        #region Client events

        /* //Disabled for future use
        private void Client_OnLog(object sender, OnLogArgs e)
        {
            //  CLog.LogWrite($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
            
            ///For future work if necesary
            
        }
        */


        private void Client_OnRaidNotificationArgs(object sender, OnRaidNotificationArgs e)
        {
            client.SendMessage(e.Channel, "We are RAIDED by " + e.RaidNotification.DisplayName + ". Shoutout for @" + e.RaidNotification.DisplayName + " which is also a streamer! https://twitch.tv/" + e.RaidNotification.DisplayName);
            logWrite("[BOT] We are raided by " + e.RaidNotification.DisplayName);
            CLog.LogWrite("[BOT] We are raided by " + e.RaidNotification.DisplayName);
        }

  
          //disbaled untill we get respons from TwitchLib Devs
          //enabled for test only
          private void Client_OnBeingHosted(object sender, OnBeingHostedArgs e)
          {

              client.SendMessage(t_userName, e.BeingHostedNotification.Channel + " is hosted with " + e.BeingHostedNotification.Viewers + " viewers by " + e.BeingHostedNotification.HostedByChannel);
              logWrite("[BOT] " + e.BeingHostedNotification.Channel + " is hosted with " + e.BeingHostedNotification.Viewers + " viewers by " + e.BeingHostedNotification.HostedByChannel);
              CLog.LogWrite("[BOT] " + e.BeingHostedNotification.Channel + " is hosted with " + e.BeingHostedNotification.Viewers + " viewers by " + e.BeingHostedNotification.HostedByChannel);

          }
          
        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            CLog.LogWrite("[" + date + $"] Connected to {e.AutoJoinChannel} channel !");
        }


        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            if (botMSGControl == "1")
            {
                logWrite("[Bot Start Message]: " + StartMessage);
                CLog.LogWrite("[Bot Start Message]: " + StartMessage);
                client.SendMessage(e.Channel, StartMessage);
            }
        }

       

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            //Display in logwindow the chat messages
            string date2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (e.ChatMessage.Message.Length > 0)
            {
                logWrite("[" + date2 + "] " + e.ChatMessage.Username + " : " + e.ChatMessage.Message);
                CLog.LogWrite("[" + date2 + "] " + e.ChatMessage.Username + " : " + e.ChatMessage.Message);
            }
            //--------------------------------------------


            //Text commands mamagement for mods. Example:
            //!create-!test-message
            //!update-!test-message2
            //!delete-!test
            using (var sReader = new StringReader(e.ChatMessage.Message))
            {

                string line;
                while ((line = sReader.ReadLine()) != null)
                {
                   
                    if (line.StartsWith("!update"))
                    {
                        List<string> mod = new List<string>();
                        foreach (var m in mods)
                        {
                            if (m.Length > 0)
                            {
                                mod.Add(m.ToLower());
                            }
                        }

                        string modList = string.Join("|", mod);

                        if (modList.Contains(e.ChatMessage.Username) || e.ChatMessage.Username == t_userName)
                        {
                            string cmd_lst = File.ReadAllText(comDirectory);
                            string[] cmd_lineA = File.ReadAllLines(comDirectory);
                            List<string> fList = new List<string>();
                            //Grab only comands list not the message 
                            List<string> clst = new List<string>();
                            foreach (var lineC in cmd_lineA)
                            {
                                if (lineC.Length > 0)
                                {
                                    string[] cmdS = lineC.Split(':');
                                    clst.Add(cmdS[0]);
                                    fList.Add(lineC);
                                }
                            }

                            //------------------------------------

                            string[] command = e.ChatMessage.Message.Split('-');
                            string finalCommand=string.Empty;
                            try
                            {
                                date2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                if (clst.Contains(command[1]))
                                {
                                    foreach (var cLine in cmd_lineA)
                                    {
                                        if (cLine.StartsWith(command[1]))
                                        {
                                            fList.Remove(cLine);
                                        }
                                    }

                                    if (!command[2].Contains("!") && !command[2].Contains(":") && !command[2].Contains("-"))
                                    {
                                        if (command[1].StartsWith("!"))
                                        {
                                            finalCommand = command[1] + ":" + command[2];
                                            fList.Add(finalCommand);
                                            string FinalCommandList = string.Join(Environment.NewLine, fList);
                                            File.WriteAllText(comDirectory, FinalCommandList);
                                            logWrite("[" + date2 + "] Command " + command[1] + " was updated by " + e.ChatMessage.Username);
                                            CLog.LogWrite("[" + date2 + "] Command " + command[1] + " was updated by " + e.ChatMessage.Username);
                                            client.SendMessage(e.ChatMessage.Channel, "Command " + command[1] + " was updated by " + e.ChatMessage.Username);
                                        }
                                        else
                                        {
                                            logWrite("@" + e.ChatMessage.Username + ", the command you want to update must start with character: ! ");
                                            CLog.LogWrite("@" + e.ChatMessage.Username + ", the command you want to update must start with character: ! ");
                                            client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + ", the command you want to update must start with character: ! ");
                                        }
                                    }
                                    else
                                    {
                                        logWrite("[" + date2 + "] @" + e.ChatMessage.Username + ", command message should not contain following characters: !, :, -");
                                        CLog.LogWrite("[" + date2 + "] @" + e.ChatMessage.Username + ", command message should not contain following characters: !, :, -");
                                        client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + ", command message should not contain following characters: !, :, -");
                                    }

                                }
                                else
                                {
                                    logWrite("[" + date2 + "] @" + e.ChatMessage.Username + ", command " + command[1] + " dose not exist!");
                                    CLog.LogWrite("[" + date2 + "] @" + e.ChatMessage.Username + ", command " + command[1] + " dose not exist!");
                                    client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + ", command " + command[1] + " dose not exist!");
                                }
                            }
                            catch (Exception c)
                            {
                                logWrite("[" + date2 + "]Error: Command update, check logs!");
                                CLog.LogWriteError("[" + date2 + "]Error: Command update:" + c.ToString());
                            }
                        }
                    }
                    else if (line.StartsWith("!create"))
                    {

                        List<string> mod = new List<string>();
                        foreach (var m in mods)
                        {
                            if (m.Length > 0)
                            {
                                mod.Add(m.ToLower());
                            }
                        }

                        string modList = string.Join("|", mod);

                        if (modList.Contains(e.ChatMessage.Username) || e.ChatMessage.Username == t_userName)
                        {

                            string cmd_lst = File.ReadAllText(comDirectory);
                            string[] cmd_lineA = File.ReadAllLines(comDirectory);
                            List<string> fList = new List<string>();
                            //Grab only comands list not the message 
                            List<string> clst = new List<string>();
                            foreach (var lineC in cmd_lineA)
                            {
                                if (lineC.Length > 0)
                                {
                                    string[] cmdS = lineC.Split(':');
                                    clst.Add(cmdS[0]);
                                    fList.Add(lineC);
                                }
                            }

                            //------------------------------------

                            string[] command = e.ChatMessage.Message.Split('-');
                            string finalCommand;
                            try
                            {
                                date2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                if (!clst.Contains(command[1]))
                                {

                                    if (!command[2].Contains("!") && !command[2].Contains(":") && !command[2].Contains("-"))
                                    {
                                        if (command[1].StartsWith("!"))
                                        {
                                            finalCommand = command[1] + ":" + command[2];
                                            fList.Add(finalCommand);
                                            string FinalCommandList = string.Join(Environment.NewLine, fList);
                                            File.WriteAllText(comDirectory, FinalCommandList);
                                            logWrite("[" + date2 + "] Command " + command[1] + " was created by " + e.ChatMessage.Username);
                                            CLog.LogWrite("[" + date2 + "] Command " + command[1] + " was created by " + e.ChatMessage.Username);
                                            client.SendMessage(e.ChatMessage.Channel, "Command " + command[1] + " was created by " + e.ChatMessage.Username);
                                        }
                                        else
                                        {
                                            logWrite("@" + e.ChatMessage.Username + ", the command you want to created must start with character: ! ");
                                            CLog.LogWrite("@" + e.ChatMessage.Username + ", the command you want to created must start with character: ! ");
                                            client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + ", the command you want to create must start with character: ! ");
                                        }
                                    }
                                    else
                                    {
                                        logWrite("[" + date2 + "] @" + e.ChatMessage.Username + ", command message should not contain following characters: !, :, -");
                                        CLog.LogWrite("[" + date2 + "] @" + e.ChatMessage.Username + ", command message should not contain following characters: !, :, -");
                                        client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + ", command message should not contain following characters: !, :, -");
                                    }

                                }
                                else
                                {
                                    logWrite("[" + date2 + "] @" + e.ChatMessage.Username + ", command " + command[1] + " already exist!");
                                    CLog.LogWrite("[" + date2 + "] @" + e.ChatMessage.Username + ", command " + command[1] + " already exist!");
                                    client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + ", command " + command[1] + " already exist!");
                                }
                            }
                            catch (Exception c)
                            {
                                logWrite("[" + date2 + "]Error: Command create, check logs!");
                                CLog.LogWriteError("[" + date2 + "]Error: Command create:" + c.ToString());
                            }
                        }
                    }
                    else if (line.StartsWith("!delete"))
                    {
                        List<string> mod = new List<string>();
                        foreach (var m in mods)
                        {
                            if (m.Length > 0)
                            {
                                mod.Add(m.ToLower());
                            }
                        }

                        string modList = string.Join("|", mod);

                        if (modList.Contains(e.ChatMessage.Username) || e.ChatMessage.Username == t_userName)
                        {

                            string cmd_lst = File.ReadAllText(comDirectory);
                            string[] cmd_lineA = File.ReadAllLines(comDirectory);
                            List<string> fList = new List<string>();
                            //Grab only comands list not the message 
                            List<string> clst = new List<string>();
                            foreach (var lineC in cmd_lineA)
                            {
                                if (lineC.Length > 0)
                                {
                                    string[] cmdS = lineC.Split(':');
                                    clst.Add(cmdS[0]);
                                    fList.Add(lineC);
                                }
                            }

                            //------------------------------------

                            string[] command = e.ChatMessage.Message.Split('-');
                            string finalCommand;
                            try
                            {
                                date2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                if (clst.Contains(command[1]))
                                {
                                    foreach (var cLine in cmd_lineA)
                                    {
                                        if (cLine.StartsWith(command[1]))
                                        {
                                            fList.Remove(cLine);
                                        }
                                    }

                                    if (command[1].StartsWith("!"))
                                    {
                                        string FinalCommandList = string.Join(Environment.NewLine, fList);
                                        File.WriteAllText(comDirectory, FinalCommandList);
                                        logWrite("[" + date2 + "] Command " + command[1] + " was deleted by " + e.ChatMessage.Username);
                                        CLog.LogWrite("[" + date2 + "] Command " + command[1] + " was deleted by " + e.ChatMessage.Username);
                                        client.SendMessage(e.ChatMessage.Channel, "Command " + command[1] + " was deleted by " + e.ChatMessage.Username);
                                    }
                                    else
                                    {
                                        logWrite("@" + e.ChatMessage.Username + ", the command you want to delete must start with character: ! ");
                                        CLog.LogWrite("@" + e.ChatMessage.Username + ", the command you want to delete must start with character: ! ");
                                        client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + ", the command you want to delete must start with character: ! ");
                                    }


                                }
                                else
                                {
                                    logWrite("[" + date2 + "] @" + e.ChatMessage.Username + ", command " + command[1] + " dose not exist!");
                                    CLog.LogWrite("[" + date2 + "] @" + e.ChatMessage.Username + ", command " + command[1] + " dose not exist!");
                                    client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + ", command " + command[1] + " dose not exist!");
                                }
                            }
                            catch (Exception c)
                            {
                                logWrite("[" + date2 + "]Error: Command delete, check logs!");
                                CLog.LogWriteError("[" + date2 + "]Error: Command delete:" + c.ToString());
                            }
                        }
                    }

                }
            }

            //---------------------------------------------

            //on bad word/spam received (chat ban or user ban)

            if (bWord == "1")
            {

                badWordList = File.ReadAllLines(badWordDir);
                foreach (var bad in badWordList)//here we check every bad word from list
                {
                    if (bad.Length > 0)
                    {
                        if (e.ChatMessage.Message.Contains(bad))
                        {
                            client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(timeBan), "Bad word ban! " + Convert.ToString(timeBan) + " minute(s) timeout!");
                            logWrite(e.ChatMessage.Channel + " | " + e.ChatMessage.Username + " | " + TimeSpan.FromMinutes(timeBan) + " | " + "[BOT] Bad word ban! " + Convert.ToString(timeBan) + " minute(s) timeout!");
                            CLog.LogWrite(e.ChatMessage.Channel + " | " + e.ChatMessage.Username + " | " + TimeSpan.FromMinutes(timeBan) + " | " + "[BOT] Bad word ban! " + Convert.ToString(timeBan) + " minute(s) timeout!");
                        }
                    }
                }

            }
            if (bWord == "2")
            {
               
                badWordList = File.ReadAllLines(badWordDir);
                foreach (var bad in badWordList)//here we check every bad word from list
                {
                    if (bad.Length > 0)
                    {
                        if (e.ChatMessage.Message.Contains(bad))
                        {
                            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            client.BanUser(e.ChatMessage.Channel, e.ChatMessage.Username, "Spam message!");
                            logWrite("[" + date + "][BOT] User banned: " + e.ChatMessage.Username);
                            CLog.LogWrite("[" + date + "][BOT] User banned: " + e.ChatMessage.Username);
                        }
                    }
                }

            }
            //-----------------------------------------------

            //on command received
            comandList = File.ReadAllLines(comDirectory);
            foreach (var com in comandList)
            {
                if (com.Length > 0)
                {
                    string[] s = com.Split(':');

                    if (e.ChatMessage.Message == s[0])
                    {
                        client.SendMessage(e.ChatMessage.Channel, s[1]);
                        logWrite("[BOT] " + s[1]);
                        CLog.LogWrite("[BOT] " + s[1]);
                    }

                }
            }
            //-------------------------------------------------

            //help message display
            string[] cmdList = File.ReadAllLines(comDirectory);
            string listCMD;
            List<string> lst = new List<string>();
            foreach (var line in cmdList)
            {
                if (line.Length > 0)
                {
                    string[] cmdS = line.Split(':');
                    lst.Add(cmdS[0]);
                }
            }
            listCMD = string.Join("; ", lst);
            if (weatherKey == "1")// we check if weather command is activated and set
            {
                if (e.ChatMessage.Message == "!help")
                {
                    client.SendMessage(e.ChatMessage.Channel, "List of commands is: " + listCMD + "; !yt; !weather; !time, !8ball");
                    logWrite("[BOT] List of commands is: " + listCMD + "; !yt; !weather; !time, !8ball");
                    CLog.LogWrite("[BOT] List of commands is: " + listCMD + "; !yt; !weather; !time, !8ball ");
                }
            }
            else
            {
                if (e.ChatMessage.Message == "!help")
                {
                    client.SendMessage(e.ChatMessage.Channel, "List of commands is: " + listCMD + "; !yt; !time, !8ball ");
                    logWrite("[BOT] List of commands is: " + listCMD + "; !yt; !time, !8ball ");
                    CLog.LogWrite("[BOT] List of commands is: " + listCMD + "; !yt; !time, !8ball ");
                }
            }
            //----------------------------

            //shout streamer commad
            using (var sReader = new StringReader(e.ChatMessage.Message))
            {
              
                string line;
                while ((line = sReader.ReadLine()) != null)
                {
                    if (line.StartsWith("!so"))
                    {
                        List<string> mod=new List<string>();
                        foreach(var m in mods)
                        {
                            if (m.Length > 0)
                            {
                                mod.Add(m.ToLower());
                            }
                        }
                        string modList = string.Join("|", mod);
                        if (modList.Contains(e.ChatMessage.Username) || e.ChatMessage.Username == t_userName)
                        {
                            string[] lS = line.Split('@');
                            try
                            {
                                if (lS[1].Length > 0)
                                {
                                    client.SendMessage(e.ChatMessage.Channel, "Shoutout for @" + lS[1] + " which is also a streamer! https://twitch.tv/" + lS[1]);
                                    logWrite("[BOT] Shoutout for @" + lS[1] + " which is also a streamer! https://twitch.tv/" + lS[1]);
                                    CLog.LogWrite("[BOT] Shoutout for @" + lS[1] + " which is also a streamer! https://twitch.tv/" + lS[1]);
                                }
                                else
                                {
                                    client.SendMessage(e.ChatMessage.Channel, "You must add the name of streamer for shoutout with @ character!");
                                    logWrite("[BOT] You must add the name of streamer for shoutout with @ character!");
                                    CLog.LogWrite("[BOT] You must add the name of streamer for shoutout with @ character!");
                                }
                            }
                            catch
                            {
                                client.SendMessage(e.ChatMessage.Channel, "[BOT] You must add the name of streamer for shoutout with @ character!");
                                logWrite("[BOT] You must add the name of streamer for shoutout with @ character!");
                            }
                        }
                        else
                        {
                            client.SendMessage(e.ChatMessage.Channel, "Only @" + t_userName + " and moderators can use the !so command!");
                            logWrite("[BOT] Only @" + t_userName + " and moderators can use the !so command!");
                            CLog.LogWrite("[BOT] Only @" + t_userName + " and moderators can use the !so command!");
                        }
                    }

                }
            }
            //----------------------------


            //good luck commad
            using (var sReader = new StringReader(e.ChatMessage.Message))
            {
                string line;
                while ((line = sReader.ReadLine()) != null)
                {
                    if (line.StartsWith("!gl"))
                    {
                        List<string> mod = new List<string>();
                        foreach (var m in mods)
                        {
                            if (m.Length > 0)
                            {
                                mod.Add(m.ToLower());
                            }
                        }
                        string modList = string.Join("|", mod);
                        if (modList.Contains(e.ChatMessage.Username) || e.ChatMessage.Username == t_userName)
                        {
                            string[] lS = line.Split('@');
                            try
                            {
                                if (lS[1].Length > 0)
                                {
                                    client.SendMessage(e.ChatMessage.Channel, e.ChatMessage.Username + " wishes " + lS[1] + " good luck. May you succeed in whatever you're planning to do!");
                                    logWrite("[BOT] " + e.ChatMessage.Username + " wishes " + lS[1] + " good luck. May you succeed in whatever you're planning to do!");
                                    CLog.LogWrite("[BOT] " + e.ChatMessage.Username + " wishes " + lS[1] + " good luck. May you succeed in whatever you're planning to do!");
                                }
                                else
                                {
                                    client.SendMessage(e.ChatMessage.Channel, "You must add the name of chatter for good luck command with @ character!");
                                    logWrite("[BOT] You must add the name of chatter for good luck command with @ character!");
                                    CLog.LogWrite("[BOT] You must add the name of chatter for good luck command with @ character!");
                                }
                            }
                            catch
                            {
                                client.SendMessage(e.ChatMessage.Channel, "[BOT] You must add the name of chatter for good luck command with @ character!");
                                logWrite("[BOT] You must add the name of chatter for good luck command with @ character!");
                            }
                        }
                        else
                        {
                            client.SendMessage(e.ChatMessage.Channel, "Only @" + t_userName + " can use the !gl command!");
                            logWrite("[BOT] Only @" + t_userName + " can use the !gl command!");
                            CLog.LogWrite("[BOT] Only @" + t_userName + " can use the !gl command!");
                        }
                    }
                }
            }
            //----------------------------

            //timezone data display 
            weatheCond = e.ChatMessage.Message;
            try
            {
                string[] we = weatheCond.Split(' ');
                string cn1 = string.Empty;
                string cn2 = string.Empty;
                if (we[0].StartsWith("!time"))
                {
                    cn1 = we[1]; //grab Continent name
                    cn2 = we[2]; //grab City Name
                    if (cn1.Length > 0 && cn2.Length > 0)
                    {

                        client.SendMessage(e.ChatMessage.Channel, "The time in " + cn2 + " (" + cn1 + ") is:" + Environment.NewLine + TimeZone(cn1, cn2));
                        logWrite("[BOT] The time in " + cn2 + " (" + cn1 + ") is:" + Environment.NewLine + TimeZone(cn1, cn2));
                        CLog.LogWrite("[BOT] The time in " + cn2 + " (" + cn1 + ") is:" + Environment.NewLine + TimeZone(cn1, cn2));


                    }
                    else
                    {
                        client.SendMessage(e.ChatMessage.Channel, "The time command should look like this: !time Continet City_Name");
                        logWrite("[BOT] The time command should look like this: !time Continet City_Name");
                        CLog.LogWrite("[BOT] The time command should look like this: !time Continet City_Name");
                    }
                }
            }
            catch
            {
                client.SendMessage(e.ChatMessage.Channel, "The time command should look like this: !time Continet City_Name. Or some information is wrong. Only Cities avaible on http://worldtimeapi.org are displyed!");
                logWrite("[BOT] The time command should look like this: !time Continet City_Name. Or some information is wrong.  Only Cities avaible on http://worldtimeapi.org are displyed!");
               
            }

            //----------------------------

            //!yt command display play link

            if (ytControl == "1")
            {
                if (e.ChatMessage.Message == "!yt")
                {
                    client.SendMessage(e.ChatMessage.Channel, "Now playing: " + YtLink);
                    logWrite("[BOT] Now playing: " + YtLink);
                    CLog.LogWrite("[BOT] Now playing: " + YtLink);
                }
            }
            else
            {
                if (e.ChatMessage.Message == "!yt")
                {
                    client.SendMessage(e.ChatMessage.Channel, "Nothing playing at the moment! ");
                    logWrite("[BOT] Nothing playing at the moment! ");
                    CLog.LogWrite("[BOT] Nothing playing at the moment! ");
                }
            }
            //----------------------------


            //!playlist command
            if (ytRequest == "1")
            {

                if (e.ChatMessage.Message == "!playlist")
                {
                    playListLoad();//we reload the playlist to catch the new songs added
                    string pOut = string.Join("", pList);
                    client.SendMessage(e.ChatMessage.Username, "Current playlist: " + Environment.NewLine + pOut + Environment.NewLine);
                    logWrite("[BOT] Current playlist: " + Environment.NewLine + pOut);
                    CLog.LogWrite("[BOT] Current playlist: " + Environment.NewLine + pOut);

                }


           
                if (e.ChatMessage.Message.StartsWith("!rsong"))
                {
                    string sR = e.ChatMessage.Message;
                    string[] sRequest = sR.Split(' ');
                    WebClient wClient = new WebClient();
                    try
                    {
                        foreach (var line in cList)
                        {
                            Thread.Sleep(200);//i hope is anty spam
                            string[] s = line.Split('|');

                            if (s[0].Contains(sRequest[1]))
                            {
                                //Download youtube link source code
                                string titleParse = wClient.DownloadString(s[1]);

                                //grabing the title from source
                                int pFrom = titleParse.IndexOf("<title>") + "<title>".Length;
                                int pTo = titleParse.LastIndexOf("</title>");

                                //store the title
                                string ytTitle = titleParse.Substring(pFrom, pTo - pFrom);
                                int c = File.ReadAllLines(playListRequest).Count();
                                int i = c + 1;
                                File.AppendAllText(playListRequest, i + "|" + s[1]);
                                client.SendMessage(e.ChatMessage.Username, "Song added in queue: " + ytTitle);
                                logWrite("[BOT] Song added in queue: " + ytTitle);
                                CLog.LogWrite("[BOT] Song added in queue: " + ytTitle);
                            }
                        }
                    }catch(Exception ex)
                    {
                        client.SendMessage(e.ChatMessage.Username, "Something went wrong with adding song in queue! ");
                        logWrite("[BOT] Something went wrong with adding song in queue! ");
                        CLog.LogWriteError("!rsong Error: " + ex.ToString());
                    }


                    
                }
                if (e.ChatMessage.Message.StartsWith("!showrequest"))
                {
                    string[] playList = File.ReadAllLines(playListRequest);

                    //we clear for reaload of list
                    qList.Clear();

                    WebClient wClient = new WebClient();

                    if (File.Exists(playListRequest))
                    {

                        int index = 0;

                        foreach (var line in playList)
                        {
                            if (line.Length > 0)
                            {
                                Thread.Sleep(200);//I hope is anty spam
                                try
                                {
                                    //Download youtube link source code
                                    string[] s = line.Split('|');
                                    string titleParse = wClient.DownloadString(s[1]);

                                    //grabing the title from source
                                    int pFrom = titleParse.IndexOf("<title>") + "<title>".Length;
                                    int pTo = titleParse.LastIndexOf("</title>");

                                    //store the title
                                    string ytTitle = titleParse.Substring(pFrom, pTo - pFrom);
                                    index++;
                                    qList.Add(index.ToString() + " | " + ytTitle + "  " + Environment.NewLine);
                                }
                                catch (Exception ex)
                                {
                                    bool ot = false;
                                    if (ot == false)
                                    {
                                        CLog.LogWriteError("[BOT] showrequest: " + ex.ToString());
                                        ot = true;
                                    }

                                }
                            }

                        }
                        index = 0;
                    }

                    if (qList.Count > 0)
                    {
                        
                        string pOut = string.Join(" ", qList);
                        client.SendMessage(e.ChatMessage.Username, "Request playlist: " + Environment.NewLine + pOut + Environment.NewLine);
                        logWrite("[BOT] Request playlist: " + Environment.NewLine + pOut);
                        CLog.LogWrite("[BOT] Request playlist: " + Environment.NewLine + pOut);
                    }
                    else
                    {
                        client.SendMessage(e.ChatMessage.Username, "There are no songs in request queue!");
                        logWrite("[BOT] There are no songs in request queue!");
                        CLog.LogWrite("[BOT] There are no songs in request queue!");
                    }
                }

            }

            //----------------------------



            //weather data display 
            if (weatherKey == "1" && apiKey != "")
            {
                string timeC = e.ChatMessage.Message;
                try
                {
                    string[] we = timeC.Split(' ');
                    string cn = string.Empty;
                    if (we[0].StartsWith("!weather"))
                    {
                        cn = we[1];
                        if (cn.Length > 0)
                        {
                            if (weatherUnits == "1")
                            {
                                client.SendMessage(e.ChatMessage.Channel, "The weather(Celsius) on " + cn + " is:" + Environment.NewLine + weatherForecast(cn));
                                logWrite("[BOT] The weather(Celsius) in " + cn + " is:" + Environment.NewLine + weatherForecast(cn));
                                CLog.LogWrite("[BOT] The weather(Celsius) in " + cn + " is:" + Environment.NewLine + weatherForecast(cn));
                            }
                            else
                            {
                                client.SendMessage(e.ChatMessage.Channel, "The weather(Fahrenheit) on " + cn + " is:" + Environment.NewLine + weatherForecast(cn));
                                logWrite("[BOT] The weather(Fahrenheit) in " + cn + " is:" + Environment.NewLine + weatherForecast(cn));
                                CLog.LogWrite("[BOT] The weather(Fahrenheit) in " + cn + " is:" + Environment.NewLine + weatherForecast(cn));

                            }
                        }
                        else
                        {
                            client.SendMessage(e.ChatMessage.Channel, "Weathr not avaible on this City or API Key tries excided!");
                            logWrite("[BOT] Weathr not avaible on this City or API Key tries excided!");
                            CLog.LogWrite("[BOT] Weathr not avaible on this City or API Key tries excided!");
                        }
                    }
                }
                catch
                {
                    client.SendMessage(e.ChatMessage.Channel, "Check the City name please!");
                    logWrite("[BOT] Check the City name please!");

                }
            }
            else
            {
                client.SendMessage(e.ChatMessage.Channel, "Weather command is disabled for the moment!");
                logWrite("[BOT] Weather command is disabled for the moment!");
            }
            //----------------------------
            
            //Magic 8Ball game command
            if (e.ChatMessage.Message.StartsWith("!8ball"))
            {
                if (e.ChatMessage.Message.Contains("?"))
                {

                    List<string> randomM = new List<string>();
                    if (File.Exists(ballAnswer))
                    {
                        bool c = false;
                        string[] rand_list = File.ReadAllLines(ballAnswer);
                        foreach (var line in rand_list)
                        {
                            if (line.Length > 0)
                            {
                                randomM.Add(line);
                            }
                            else
                            {
                                if (c == false)
                                {
                                    logWrite("[" + date + "][BOT] 8Ball - Answers files is empty! You need to add something");
                                    c = true;
                                }
                            }
                        }
                        int index = r8.Next(randomM.Count);
                        string rand = randomM[index];
                        date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        logWrite("[" + date + "][BOT] Scott says: " + rand);
                        client.SendMessage(e.ChatMessage.Channel, "Scott says: " + rand);
                        CLog.LogWrite("[" + date + "][BOT] Scott says: " + rand);
                    }
                }
                else
                {
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    logWrite("[" + date + "][BOT] Your question must contain ? for Scott to answer!");
                    client.SendMessage(e.ChatMessage.Channel,"Your question must contain ? for Scott to answer!");
                    CLog.LogWrite("[" + date + "][BOT] Your question must contain ? for Scott to answer!");

                }
            }
            //----------------------------

        }
      
 

       
        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
            {
                client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
                logWrite(e.Channel + $":  Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
                CLog.LogWrite(e.Channel + $":  Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
            }
            else
            {
                client.SendMessage(e.Channel, $" Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
                logWrite(e.Channel + $":  Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
                CLog.LogWrite(e.Channel + $":  Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
            }

            //increase subs count
            SubsCount++;
        }

        private void Client_OnUserJoinedArgs(object sender, OnUserJoinedArgs e)
        {
            //Enebale user joiend chat only when check is on.
            if (botMSGKey == "1")
            {
                client.SendMessage(e.Channel, $"Welcome to my channel {e.Username}. and thank you for joining. For more commands type !help");
                logWrite(e.Channel + $"  Welcome to my channel {e.Username}. and thank you for joining. For more commands type !help");
                CLog.LogWrite(e.Channel + $"  Welcome to my channel {e.Username}. and thank you for joining. For more commands type !help");
            }
            //we increment with 1 integer when a person has joined the chat room
            Viewers++;
        }

        private void Client_OnUserLeftArgs(object sender, OnUserLeftArgs e)
        {
            //we decrement with 1 integer when a person left the chat
            if (Viewers > 0)
            {
                Viewers--;
            }
        }


        #endregion


        /// <summary>
        /// TimeZone extract using http://worldtimeapi.org API
        /// </summary>
        /// <param name="Region">Continent</param>
        /// <param name="CityName">City Name</param>
        /// <returns>string</returns>
        private string TimeZone(string Region, string CityName)
        {
            string _date = DateTime.Now.ToString("yyyy_MM_dd");
            string date2 = string.Empty;
            string errFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\log\errors\" + _date + "_log.txt";
            string outs = string.Empty;
            string html = @"http://worldtimeapi.org/api/timezone/{0}/{1}";
            try
            {

                HttpResponseMessage response = clientH.GetAsync(string.Format(html, Region, CityName)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;

                //parsing the https output 
                string[] oS = responseBody.Split(',');
                string[] oY = oS[2].Split('.');
                oY[0] = oY[0].Replace("\"datetime\":\"", "Date: ");
                oY[0] = oY[0].Replace("T", " Hour: ");
                //-----------------

                //return the final data
                outs = oY[0];


            }
            catch (Exception e)
            {
                outs = "Please check City name or Continent name. Only Cities avaible on http://worldtimeapi.org are displyed!";
                //save the entire error to file
                date2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                if (File.Exists(errFile))
                {
                    string rErrorFile = File.ReadAllText(errFile);

                    if (!rErrorFile.Contains("[" + date2 + "] TimeZone error: "))
                    {
                        CLog.LogWriteError("[" + date2 + "] TimeZone error: " + e.ToString() + Environment.NewLine);
                    }
                }
                else
                {
                    File.WriteAllText(errFile, "");
                    string rErrorFile = File.ReadAllText(errFile);

                    if (!rErrorFile.Contains("[" + date2 + "] TimeZone error: "))
                    {
                        CLog.LogWriteError("[" + date2 + "] TimeZone error: " + e.ToString() + Environment.NewLine);
                    }
                }
                //--------------------------------
            }

            return outs;

        }


        /// <summary>
        /// weather api check and return the output parssed
        /// </summary>
        /// <param name="CityName"></param>
        /// <returns></returns>
        private string weatherForecast(string CityName)
        {

            string _date = DateTime.Now.ToString("yyyy_MM_dd");
            string date2 = string.Empty;
            string errFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\log\errors\" + _date + "_log.txt";
            string outs = string.Empty;
            try
            {

                if (apiKey.Length > 0) // we check the lenght
                {
                    //Open weather map API link with celsius 
                    // TODO: will decide if I put switch for ferenhait
                    string html = @"https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";
                    if (weatherUnits == "1")
                    {
                        html = @"https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";
                    }
                    else
                    {
                        html = @"https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=imperial";
                    }

                    HttpResponseMessage response = clientH.GetAsync(string.Format(html, CityName, Encryption._decryptData(apiKey))).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    string l = "";
                    string line = "";
                    //parssing the oudtput
                    responseBody = responseBody.Replace(",", Environment.NewLine);
                    responseBody = responseBody.Replace("\"", "");
                    responseBody = responseBody.Replace("}", "");
                    responseBody = responseBody.Replace("{", "");
                    responseBody = responseBody.Replace("wind:", "");
                    responseBody = responseBody.Replace("main:", "");
                    //---------------------------------
                    using (var sr = new StringReader(responseBody))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {

                            //we check only for what we need, like: temp, feel, humidity, wind speed
                            if (line.Contains("temp") || line.Contains("feel") || line.Contains("humidity") || line.Contains("speed"))
                            {
                                l += line + Environment.NewLine;
                            }
                        }
                    }
                    outs = l;
                    //renaming output parts
                    outs = outs.Replace("temp:", " Temperature: ");
                    outs = outs.Replace("feels_like:", " Feels Like: ");
                    outs = outs.Replace("temp_min:", " Minim Temperature: ");
                    outs = outs.Replace("temp_max:", " Maxim Temperature: ");
                    outs = outs.Replace("humidity:", " Humidity: ");
                    outs = outs.Replace("speed:", " Wind Speed: ");
                    //---------------------------------
                }
                else
                {
                    //we print the issue on the log viewer console
                    logWrite("No openweathermap.org API Key saved! Please check" + Environment.NewLine);
                }
            }
            catch (Exception e)
            {
                //In case of error we output this in console.
                outs = "Please check city name!";

                //save the entire error to file
                date2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                if (File.Exists(errFile))
                {
                    string rErrorFile = File.ReadAllText(errFile);

                    if (!rErrorFile.Contains("[" + date2 + "] Weather error: "))
                    {
                        CLog.LogWriteError("[" + date2 + "] Weather error: " + e.ToString() + Environment.NewLine);
                    }
                }
                else
                {
                    File.WriteAllText(errFile, "");
                    string rErrorFile = File.ReadAllText(errFile);

                    if (!rErrorFile.Contains("[" + date2 + "] Weather error: "))
                    {
                        CLog.LogWriteError("[" + date2 + "] Weather error: " + e.ToString() + Environment.NewLine);
                    }
                }
                //--------------------------------

            }

            //print the final weather forecast
            return outs;

        }
        /// <summary>
        /// Drag window on mouse click left
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }


        /// <summary>
        /// Close wpf form button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeBTN_Click(object sender, RoutedEventArgs e)
        {
            botConnect();//disconnect the bot on ext

            System.Windows.Application.Current.Shutdown();//close the app
        }

        /// <summary>
        /// Load icon bubble for bot connection status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusLoadIcon(object sender, EventArgs e)
        {
            //Display total chat users and subs from stream
            viewersLbL.Content = Viewers.ToString();
            subsLbL.Content = SubsCount.ToString();
            //todo: add subs count on label
            //-----------------------------------

            //read variables form registry
            t_userName = Reg.regKey_Read(keyName, "UserName");

            try
            {

                t_streamKey = Encryption._decryptData(Reg.regKey_Read(keyName, "StreamKey"));
            }
            catch (Exception x)
            {
                CLog.LogWriteError("oAuth decrypt error: " + x.ToString());
                t_streamKey = "error_key";
            }

            botMSGKey = Reg.regKey_Read(keyName, "BotMSG");
            timeBan = Int32.Parse(Reg.regKey_Read(keyName, "WordBanTime"));
            bWord = Reg.regKey_Read(keyName, "BadWord");
            apiKey = Reg.regKey_Read(keyName, "WeatherAPIKey");
            ytControl = Reg.regKey_Read(keyName, "YTControl");
            weatherKey = Reg.regKey_Read(keyName, "WeatherMSG");
            YtWin = Reg.regKey_Read(keyName, "YtWin");
            StartMessage = Reg.regKey_Read(keyName, "StartMessage");
            YtLink = Reg.regKey_Read(keyName, "YtLink");
            botMSGControl = Reg.regKey_Read(keyName, "botMSGControl");
            weatherUnits = Reg.regKey_Read(keyName, "weatherUnits");
            randomC = Reg.regKey_Read(keyName, "randomC");
            rTime = Reg.regKey_Read(keyName, "rTime");
            ytRequest = Reg.regKey_Read(keyName, "ytRequest");
            //-----------------------------------


            //read mods from file
            if (File.Exists(modFile))
            {
                mods = File.ReadAllLines(modFile);
            }
            //-----------------------------------

            //status checking on internet and bot 
            if (Network.inetCK())
            {
                if (client.IsConnected)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        startBotBTN.Content = "STOP";
                        statIMG.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/green_dot.png"));
                    });
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        statIMG.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/orange_dot.png"));
                    });
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    client.Connect();
                    if (client.IsConnected)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            startBotBTN.Content = "STOP";
                        });
                        logWrite("[" + date + "]Internet up. Reconnected to " + t_userName + " channel !");
                        CLog.LogWrite("[" + date + "]Internet up. Reconnected to " + t_userName + " channel !");
                    }
                }
            }
            else
            {
                if (!client.IsConnected)
                {
                    //we reset the people chat room and new subs counters
                    Viewers = 0;
                    viewersLbL.Content = "0";

                    SubsCount = 0;
                    subsLbL.Content = "0";
                    //-------------------------------------------
                    this.Dispatcher.Invoke(() =>
                    {
                        startBotBTN.Content = "START";
                        date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        statIMG.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/red_dot.png"));
                    });
                    string oRTB = ConvertRichTextBoxContentsToString(logViewRTB);
                    if (!oRTB.Contains("[" + date + "] No internet connection at the moment. Trying to reconnect..."))
                    {
                        logWrite("[" + date + "] No internet connection at the moment. Trying to reconnect...");
                        CLog.LogWrite("[" + date + "] No internet connection at the moment. Trying to reconnect...");
                    }
                }
            }
            //------------------------------------------------

            //Will see if this will solve the mistery crash issue
            GC.Collect();
        }


        /// <summary>
        /// Output string from a rich text box
        /// </summary>
        /// <param name="rtb"></param>
        /// <returns></returns>
        private string ConvertRichTextBoxContentsToString(System.Windows.Controls.RichTextBox rtb)
        {
            TextRange textRange = new TextRange(rtb.Document.ContentStart,
            rtb.Document.ContentEnd);
            return textRange.Text;
        }

        /// <summary>
        /// Openning settings form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsBTN_Click(object sender, RoutedEventArgs e)
        {
            settings st = new settings();
            st.Show();
        }

        /// <summary>
        /// Bot start/stop button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startBTN_Click(object sender, RoutedEventArgs e)
        {
            botConnect();
        }

        /// <summary>
        /// Bot connection function
        /// </summary>

        private void botConnect()
        {
            if (Network.inetCK())//check internet connection first
            {
                if (Network.portCheck("tmi.twitch.tv", 80))//check twitch server
                {

                    //read Twitch chanel name and bot oAuth Key
                    t_userName = Reg.regKey_Read(keyName, "UserName");

                    try
                    {

                        t_streamKey = Encryption._decryptData(Reg.regKey_Read(keyName, "StreamKey"));
                    }
                    catch (Exception x)
                    {
                        CLog.LogWriteError("oAuth decrypt error: " + x.ToString());
                        t_streamKey = "error_key";
                    }
                    //-------------------------------------

                    if (client.IsConnected)
                    {
                        BotStop();
                    }
                    else
                    {
                        if (t_userName.Length > 0)
                        {

                            if (t_streamKey != "error_key")
                            {
                                dispatcherTimer.Stop();
                                dispatcherTimerR.Stop();
                                worker = new BackgroundWorker();
                                worker.DoWork += BotStart;
                                worker.RunWorkerAsync();
                                dispatcherTimer.Start();
                                dispatcherTimerR.Start();
                            }
                            else
                            {
                                logWrite("Please fill in settings your oAuth Twitch key generated from https://twitchapps.com/tmi/ !");
                            }
                        }
                        else
                        {
                            logWrite("Please fill in settings the user name for the Twitch Channel that you want to connect!");
                        }
                    }
                }
                else
                {
                    logWrite("Twitch tmi.twitch.tv server is down!");
                }
            }
            else
            {
                logWrite("No internet connection!");

            }
        }
        /// <summary>
        /// left slide menu open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            btnOpenMenu.Visibility = Visibility.Collapsed;
            btnCloseMenu.Visibility = Visibility.Visible;
            startBotBTN.Visibility = Visibility.Visible;
            logViewRTB.Margin = new Thickness(199, 50, 0, 0);
            Reg.regKey_WriteSubkey(keyName, "Menu", "1");
        }

        /// <summary>
        /// left slide menu close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            logViewRTB.Margin = new Thickness(50, 50, 0, 0);
            btnOpenMenu.Visibility = Visibility.Visible;
            btnCloseMenu.Visibility = Visibility.Collapsed;
            startBotBTN.Visibility = Visibility.Hidden;
            Reg.regKey_WriteSubkey(keyName, "Menu", "0");
        }


        /// <summary>
        /// Minimizr button(label)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miniMizeLBL_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// opening settings window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            sT = new settings();
            sT.ShowDialog();
        }

        //closing all windows
        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                if (yT.IsVisible)
                {
                    yT.Close();
                }
            }
            catch
            {
                //We move on 
            }
        }

        /// <summary>
        /// Display bot message window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_PreviewMouseDownBot(object sender, MouseButtonEventArgs e)
        {
            bM = new botMSG();
            bM.ShowDialog();
        }

        /// <summary>
        /// start bot from icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statIMG_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            botConnect();
        }

        /// <summary>
        /// Display command window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_PreviewMouseDownCMD(object sender, MouseButtonEventArgs e)
        {
            cmD = new command();
            cmD.ShowDialog();
        }

        /// <summary>
        /// Display bad words window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_PreviewMouseDownBAD(object sender, MouseButtonEventArgs e)
        {
            bW = new badWords();
            bW.ShowDialog();
        }

        /// <summary>
        /// Display YouTube player window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_PreviewMouseDownYT(object sender, MouseButtonEventArgs e)
        {
            yT = new youtube();

            //check youtube window control status number
            if (YtWin == "0")
            {
                yT.Show();
            }
            //------------------------------
        }


        /// <summary>
        /// About window open button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutBTN_Click(object sender, RoutedEventArgs e)
        {
            aB = new about();
            aB.ShowDialog();
        }

        /// <summary>
        /// Random messege sender function and set timer interval for future resend
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void randomMessage(object sender, EventArgs e)
        {
            string dateSent = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            if (randomC == "1")
            {
                List<string> randomM = new List<string>();
                if (File.Exists(randomListFile))
                {
                    rand_list = File.ReadAllLines(randomListFile);
                    foreach (var line in rand_list)
                    {
                        randomM.Add(line);
                    }
                    int index = r.Next(randomM.Count);
                    string rand = randomM[index];
                   
                    try
                    {
                        if (client.IsConnected)
                        {
                            client.SendMessage(t_userName, rand);//sending the random message from list
                            logWrite("[Random Message] [Interval set to: " + rTime + " minutes]" + rand);
                            CLog.LogWrite("[" + dateSent + "]Random Message: " + rand);
                        }
                        else
                        {
                            logWrite("[Random Message] Client is disconnected!");
                            CLog.LogWriteError("[" + dateSent + "]Random Message: " + rand);
                        }
                    }
                    catch(Exception ex)
                    {
                        CLog.LogWriteError("[" + dateSent + "]Error - Random Message: " + ex.ToString());
                    }

                    if (Convert.ToInt32(rTime) > 0)
                    {
                        //set timer interval for nex resend
                        dispatcherTimerR.Interval = new TimeSpan(0, Convert.ToInt32(rTime), 0);
                    }
                    else
                    {
                        dispatcherTimerR.Interval = new TimeSpan(0, 10, 0);
                    }
                }
                else
                {
                    logWrite("[Random Message Error] File " + randomListFile + " dose not exist!");
                    CLog.LogWriteError("File " + randomListFile + " dose not exist!");
                }
            }
        }
        /// <summary>
        /// Load playlist and add index number for every song
        /// </summary>
        private void playListLoad()
        {
            string[] playList = File.ReadAllLines(playListFile);
          
            WebClient wClient = new WebClient();
            //we clear for reaload of list
            pList.Clear();
            cList.Clear();
            if (File.Exists(playListFile))
            {
                try
                {
                    int index = 0;

                    foreach (var line in playList)
                    {
                        Thread.Sleep(200);//i hope is anty spam
                        //Download youtube link source code
                        string titleParse = wClient.DownloadString(line);
                        
                        //grabing the title from source
                        int pFrom = titleParse.IndexOf("<title>") + "<title>".Length;
                        int pTo = titleParse.LastIndexOf("</title>");

                        //store the title
                        string ytTitle = titleParse.Substring(pFrom, pTo - pFrom);
                        index++;

                        pList.Add(index.ToString() + " | " + ytTitle + "  " + Environment.NewLine);
                        cList.Add(index.ToString() + "|" + line + Environment.NewLine);
                    }
                    index = 0;
                }
                catch(Exception ex)
                {
                    CLog.LogWriteError("[BOT] playListLoad: "+ex.ToString());
                }
            }
            else
            {
                logWrite("[BOT] File " + playListFile + "dose not exist! Restart application for recreate!");
                CLog.LogWriteError("[BOT] File " + playListFile + "dose not exist! Restart application for recreate!");
            }
        }

        /// <summary>
        /// we load the list with the requested songs 
        /// </summary>
        private void queueListLoad()
        {
            string[] playList = File.ReadAllLines(playListRequest);
            //we clear for reaload of list
            qList.Clear();

            WebClient wClient = new WebClient();

            if (File.Exists(playListRequest))
            {

                int index = 0;

                foreach (var line in playList)
                {
                    if (line.Length > 0)
                    {
                        Thread.Sleep(200);//I hope is anty spam
                        try
                        {
                            //Download youtube link source code
                            string[] s = line.Split('|');
                            string titleParse = wClient.DownloadString(s[1]);

                            //grabing the title from source
                            int pFrom = titleParse.IndexOf("<title>") + "<title>".Length;
                            int pTo = titleParse.LastIndexOf("</title>");

                            //store the title
                            string ytTitle = titleParse.Substring(pFrom, pTo - pFrom);
                            index++;
                            qList.Add(index.ToString() + " | " + ytTitle + "  " + Environment.NewLine);
                        }catch(Exception ex)
                        {
                            CLog.LogWriteError("[BOT] Queuelistload: " + ex.ToString());

                        }
                    }

                }
                index = 0;
            }
            else
            {
                logWrite("[BOT] File " + playListRequest + "dose not exist! Restart application for recreate!");
                CLog.LogWriteError("[BOT] File " + playListRequest + "dose not exist! Restart application for recreate!");
            }
        } 
    }
}
