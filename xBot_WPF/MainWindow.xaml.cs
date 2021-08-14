﻿using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

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
        private readonly static string s_DataDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data";
        private readonly static string s_LogDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\log";
        private readonly static string s_LogErrorDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\log\errors";
        //------------------------------------------------


        //Declare keyname and subkey
        private readonly static string s_KeyName = "xBot";
        private static string s_WeatherKey = "0";
        private static string s_BotMSGKey = "0";
        private static string s_BotMSGControl = "0";
        private static string s_WeatherUnits = "0";
        private static string s_MenuStatus = "0";
        //------------------------------------------------

        //declare twitch credential info
        private static string s_UserName;
        private static string s_StreamKey;
        //-------------------------------------------------


        //command file variables
        private readonly static string s_ComDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\command.txt";
        private static string[] s_ComandList;
        //-------------------------------------------------


        //bad word lists path declaration and variable
        readonly static string s_BadWordDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\badwords.txt";
        private static string[] s_BadWordList;
        private static string s_BadWord = "0";
        int _TimeBan = 0;
        //-------------------------------------------------

        //date variable declar
        private static string s_Date;
        //-------------------------------------------------

        //declare path to bot message
        private static string s_StartMessage;
        //-------------------------------------------------

        //Media player declaration
        private readonly static string s_PlayListFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\playList.txt";
        private static string s_YtLink;
        private static string s_YtControl = "0";
        private static string s_YtWin = "0";
        private static string s_ytRequest = "0";
        readonly static string s_PlayListRequest = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\playListRequest.txt";
        List<string> CList = new List<string>();
        List<string> PList = new List<string>();
        List<string> QList = new List<string>();
        List<string> RegistryKeys = new List<string>() { "UserName", "StreamKey", "WeatherAPIKey", "StartMessage", "YtLink", "YtUrl" };
        List<string> RegistryKeysNull = new List<string>() { "WeatherMSG", "BotMSG", "BadWord", "WordBanTime", "YTControl", "YtWin", "botMSGControl", "weatherUnits", "Menu", "randomC", "rTime", "ytRequest" };
        List<string> FilesList = new List<string>() { s_ModsFile, s_BadWordDir, s_ComDirectory, s_PlayListFile, s_PlayListRequest, s_RandomListFile, s_PlayListRequest, s_RandomListFile, s_BallAnswer };
        List<string> DirectoryList = new List<string> { s_DataDirectory, s_LogDirectory, s_LogErrorDirectory };
        //-------------------------------------------------


        //declare weather variables
        private static string s_ApiKey = string.Empty;
        private static string s_WeatheCond = string.Empty;
        static readonly HttpClient s_ClientH = new HttpClient();
        //--------------------------------------------------

        //declare the bot forms variables
        settings Settings;
        botMSG BotMessage;
        command Command;
        badWords BadWords;
        youtube YouTube;
        about About;
        //--------------------------------

        //Define the background worker for bot start and random message
        BackgroundWorker Worker;
        BackgroundWorker WorkerForPlayList;
        //--------------------------------

        //Declare mutex variable for startup instance check
        Mutex MyMutex;
        //---------------------------------


        //declare timer for load icons and variables read value
        System.Windows.Threading.DispatcherTimer DispatcherTimer;
        //--------------------------------

        private int s_Viewers = 0;
        private int s_SubsCount = 0;

        //declare variables for random message system
        private readonly static string s_RandomListFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\random_msg.txt";
        private static string s_RandomC;
        System.Windows.Threading.DispatcherTimer DispatcherTimerR;
        private string[] s_Rand_list;
        private static string s_RTime;
        Random Random = new Random();
        //--------------------------------

        //declare variables for mod section
        private readonly static string s_ModsFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\mods.txt";
        private string[] s_Mods = null;
        //--------------------------------

        //declare variables for Magic 8ball game
        readonly static string s_BallAnswer = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\data\8ball_answers.txt";
        Random Random8 = new Random();
        //--------------------------------


        public MainWindow()
        {
            InitializeComponent();

            // Aplication startup instance check
            Application_Startup();
            //---------------------------------------------
            // Bot start message
            this.Dispatcher.Invoke(() =>
            {
                logViewRTB.Document.Blocks.Clear();
            });
            logWrite("Welcome to xBot! The Twitch bot that was entirely build on live stream :D");

            // Check existence for necesary directory and files.
            CheckDirectoryStartUp(DirectoryList);
            CheckFileStartUp(FilesList);

            // Checking if reg keys and subkeys exist and if not we recreate.
            Reg.CheckRegKeysStart(RegistryKeys, s_KeyName, "", false);
            Reg.CheckRegKeysStart(RegistryKeysNull, s_KeyName, "", true);

            // Load variables stored in registry.
            ReadRegistryVariables();


            // Menu state check and apply
            MenuStatusChange(s_MenuStatus);


            // Staus check timer declaration
            DispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            DispatcherTimer.Tick += StatusLoadIcon;
            DispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            //----------------------------------

            //timer declaration for random message send
            DispatcherTimerR = new System.Windows.Threading.DispatcherTimer();
            DispatcherTimerR.Tick += RandomMessage;
            if (Convert.ToInt32(s_RTime) > 0)
            {
                // logWrite(Convert.ToInt32(rTime).ToString());
                DispatcherTimerR.Interval = new TimeSpan(0, Convert.ToInt32(s_RTime), 0);
            }
            else
            {
                DispatcherTimerR.Interval = new TimeSpan(0, 10, 0);
            }
            DispatcherTimerR.Stop();
            //----------------------------------

            //reset Youtube Controler/request song and window on bot start in case of crash
            Reg.regKey_WriteSubkey(s_KeyName, "YTControl", "0");
            Reg.regKey_WriteSubkey(s_KeyName, "YtWin", "0");
            //-----------------------------------

            //load connection icon
            this.Dispatcher.Invoke(() =>
            {
                statIMG.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/red_dot.png"));
            });
            //---------------------------------

            //read mods from file
            if (File.Exists(s_ModsFile))
            {
                s_Mods = File.ReadAllLines(s_ModsFile);
            }
            //-----------------------------------

        }



        /// <summary>
        /// Check aplication start instace and close if is already opened.
        /// </summary>
        private void Application_Startup()
        {
            MyMutex = new Mutex(true, "xBot", out bool aIsNewInstance);
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

            s_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            logWrite("[" + s_Date + "] xBot connecting to " + s_UserName + " channel....");

            ConnectionCredentials credentials = new ConnectionCredentials(s_UserName, s_StreamKey, null, true);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, s_UserName);
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
                logWrite("[" + s_Date + "] xBot Connected to " + s_UserName + " channel !");
                Task.Delay(2);
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
                DispatcherTimer.Stop();
                DispatcherTimerR.Stop();
                this.Dispatcher.Invoke(() =>
                {
                    statIMG.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/red_dot.png"));
                });
            }
            if (!client.IsConnected)
            {
                s_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                logWrite("[" + s_Date + "] xBot Disconncted!");
                CLog.LogWrite("[" + s_Date + "] xBot Disconncted!");
                startBotBTN.Content = "START";

                //reset viewers/new subs counter
                s_Viewers = 0;
                viewersLbL.Content = "0";

                s_SubsCount = 0;
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


        // We write log in RTB/File/Both
        private void FullLogWrite(string logData, LogTypeArg type)
        {
            switch (type)
            {
                case LogTypeArg.Display: // Write in display
                    logWrite(logData);
                    break;
                case LogTypeArg.File: // Write in file
                    CLog.LogWrite(logData);
                    break;
                case LogTypeArg.Both: //Write both
                    logWrite(logData);
                    CLog.LogWrite(logData);
                    break;
            }
        }

        // We enumerate the types of Log types
        private enum LogTypeArg
        {
            Display = 1,
            File = 2,
            Both = 3
        }

        private void Client_OnRaidNotificationArgs(object sender, OnRaidNotificationArgs e)
        {
            client.SendMessage(e.Channel, "We are RAIDED by " + e.RaidNotification.DisplayName + ". Shoutout for @" + e.RaidNotification.DisplayName + " which is also a streamer! https://twitch.tv/" + e.RaidNotification.DisplayName);
            FullLogWrite("[BOT] We are raided by " + e.RaidNotification.DisplayName, LogTypeArg.Both);
        }


        //disbaled untill we get respons from TwitchLib Devs
        //enabled for test only
        private void Client_OnBeingHosted(object sender, OnBeingHostedArgs e)
        {
            client.SendMessage(s_UserName, e.BeingHostedNotification.Channel + " is hosted with " + e.BeingHostedNotification.Viewers + " viewers by " + e.BeingHostedNotification.HostedByChannel);
            FullLogWrite("[BOT] " + e.BeingHostedNotification.Channel + " is hosted with " + e.BeingHostedNotification.Viewers + " viewers by " + e.BeingHostedNotification.HostedByChannel, LogTypeArg.Both);
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            s_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            FullLogWrite("[" + s_Date + $"] Connected to {e.AutoJoinChannel} channel !", LogTypeArg.File);
        }


        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            if (s_BotMSGControl == "1")
            {
                client.SendMessage(e.Channel, s_StartMessage);
                FullLogWrite("[Bot Start Message]: " + s_StartMessage, LogTypeArg.Both);
            }
        }



        private async void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            //Display in logwindow the chat messages
            string date2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (e.ChatMessage.Message.Length > 0)
            {
                FullLogWrite("[" + date2 + "] " + e.ChatMessage.Username + " : " + e.ChatMessage.Message, LogTypeArg.Both);
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
                        foreach (var m in s_Mods)
                        {
                            if (m.Length > 0)
                            {
                                mod.Add(m.ToLower());
                            }
                        }

                        string modList = string.Join("|", mod);

                        if (modList.Contains(e.ChatMessage.Username) || e.ChatMessage.Username == s_UserName)
                        {
                            string cmd_lst = File.ReadAllText(s_ComDirectory);
                            string[] cmd_lineA = File.ReadAllLines(s_ComDirectory);
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
                            string finalCommand = string.Empty;
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
                                            File.WriteAllText(s_ComDirectory, FinalCommandList);
                                            FullLogWrite("[" + date2 + "] Command " + command[1] + " was updated by " + e.ChatMessage.Username, LogTypeArg.Both);
                                            client.SendMessage(e.ChatMessage.Channel, "Command " + command[1] + " was updated by " + e.ChatMessage.Username);
                                        }
                                        else
                                        {
                                            FullLogWrite("@" + e.ChatMessage.Username + ", the command you want to update must start with character: ! ", LogTypeArg.Both);
                                            client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + ", the command you want to update must start with character: ! ");
                                        }
                                    }
                                    else
                                    {
                                        FullLogWrite("[" + date2 + "] @" + e.ChatMessage.Username + ", command message should not contain following characters: !, :, -", LogTypeArg.Both);
                                        client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + ", command message should not contain following characters: !, :, -");
                                    }

                                }
                                else
                                {
                                    FullLogWrite("[" + date2 + "] @" + e.ChatMessage.Username + ", command " + command[1] + " dose not exist!", LogTypeArg.Both);
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
                        foreach (var m in s_Mods)
                        {
                            if (m.Length > 0)
                            {
                                mod.Add(m.ToLower());
                            }
                        }

                        string modList = string.Join("|", mod);

                        if (modList.Contains(e.ChatMessage.Username) || e.ChatMessage.Username == s_UserName)
                        {

                            string cmd_lst = File.ReadAllText(s_ComDirectory);
                            string[] cmd_lineA = File.ReadAllLines(s_ComDirectory);
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
                                            File.WriteAllText(s_ComDirectory, FinalCommandList);
                                            FullLogWrite("[" + date2 + "] Command " + command[1] + " was created by " + e.ChatMessage.Username, LogTypeArg.Both);
                                            client.SendMessage(e.ChatMessage.Channel, "Command " + command[1] + " was created by " + e.ChatMessage.Username);
                                        }
                                        else
                                        {
                                            FullLogWrite("@" + e.ChatMessage.Username + ", the command you want to created must start with character: ! ", LogTypeArg.Display);
                                            client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + ", the command you want to create must start with character: ! ");
                                        }
                                    }
                                    else
                                    {
                                        FullLogWrite("[" + date2 + "] @" + e.ChatMessage.Username + ", command message should not contain following characters: !, :, -", LogTypeArg.Both);
                                        client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + ", command message should not contain following characters: !, :, -");
                                    }

                                }
                                else
                                {
                                    FullLogWrite("[" + date2 + "] @" + e.ChatMessage.Username + ", command " + command[1] + " already exist!", LogTypeArg.Both);
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
                        foreach (var m in s_Mods)
                        {
                            if (m.Length > 0)
                            {
                                mod.Add(m.ToLower());
                            }
                        }

                        string modList = string.Join("|", mod);

                        if (modList.Contains(e.ChatMessage.Username) || e.ChatMessage.Username == s_UserName)
                        {

                            string cmd_lst = File.ReadAllText(s_ComDirectory);
                            string[] cmd_lineA = File.ReadAllLines(s_ComDirectory);
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
                                        File.WriteAllText(s_ComDirectory, FinalCommandList);
                                        FullLogWrite("[" + date2 + "] Command " + command[1] + " was deleted by " + e.ChatMessage.Username, LogTypeArg.Both);
                                        client.SendMessage(e.ChatMessage.Channel, "Command " + command[1] + " was deleted by " + e.ChatMessage.Username);
                                    }
                                    else
                                    {
                                        FullLogWrite("@" + e.ChatMessage.Username + ", the command you want to delete must start with character: ! ", LogTypeArg.Display);
                                        client.SendMessage(e.ChatMessage.Channel, "@" + e.ChatMessage.Username + ", the command you want to delete must start with character: ! ");
                                    }
                                }
                                else
                                {
                                    FullLogWrite("[" + date2 + "] @" + e.ChatMessage.Username + ", command " + command[1] + " dose not exist!", LogTypeArg.Both);
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

            if (s_BadWord == "1")
            {

                s_BadWordList = File.ReadAllLines(s_BadWordDir);
                foreach (var bad in s_BadWordList)//here we check every bad word from list
                {
                    if (bad.Length > 0)
                    {
                        if (e.ChatMessage.Message.Contains(bad))
                        {
                            FullLogWrite(e.ChatMessage.Channel + " | " + e.ChatMessage.Username + " | " + TimeSpan.FromMinutes(_TimeBan) + " | " + "[BOT] Bad word ban! " + Convert.ToString(_TimeBan) + " minute(s) timeout!", LogTypeArg.Both);
                            client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(_TimeBan), "Bad word ban! " + Convert.ToString(_TimeBan) + " minute(s) timeout!");
                        }
                    }
                }

            }
            if (s_BadWord == "2")
            {

                s_BadWordList = File.ReadAllLines(s_BadWordDir);
                foreach (var bad in s_BadWordList)//here we check every bad word from list
                {
                    if (bad.Length > 0)
                    {
                        if (e.ChatMessage.Message.Contains(bad))
                        {
                            s_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            client.BanUser(e.ChatMessage.Channel, e.ChatMessage.Username, "Spam message!");
                            FullLogWrite("[" + s_Date + "][BOT] User banned: " + e.ChatMessage.Username, LogTypeArg.Both);
                        }
                    }
                }

            }
            //-----------------------------------------------

            //on command received
            s_ComandList = File.ReadAllLines(s_ComDirectory);
            foreach (var com in s_ComandList)
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
            string[] cmdList = File.ReadAllLines(s_ComDirectory);
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
            if (s_WeatherKey == "1")// we check if weather command is activated and set
            {
                if (e.ChatMessage.Message == "!help")
                {
                    client.SendMessage(e.ChatMessage.Channel, "List of commands is: " + listCMD + "; !yt; !weather; !time, !8ball");
                    FullLogWrite("[BOT] List of commands is: " + listCMD + "; !yt; !weather; !time, !8ball", LogTypeArg.Both);
                }
            }
            else
            {
                if (e.ChatMessage.Message == "!help")
                {
                    client.SendMessage(e.ChatMessage.Channel, "List of commands is: " + listCMD + "; !yt; !time, !8ball ");
                    FullLogWrite("[BOT] List of commands is: " + listCMD + "; !yt; !time, !8ball ", LogTypeArg.Both);
                }
            }
            //----------------------------

            //shout streamer commad
            using (var sReader = new StringReader(e.ChatMessage.Message))
            {

                string line;
                while ((line = sReader.ReadLine()) != null)
                {
                    if (line.StartsWith("!so") && !line.Contains("sorry")) //fixing the !sorry command 
                    {
                        List<string> mod = new List<string>();
                        foreach (var m in s_Mods)
                        {
                            if (m.Length > 0)
                            {
                                mod.Add(m.ToLower());
                            }
                        }
                        string modList = string.Join("|", mod);
                        if (modList.Contains(e.ChatMessage.Username) || e.ChatMessage.Username == s_UserName)
                        {
                            string[] lS = line.Split('@');
                            try
                            {
                                if (lS[1].Length > 0)
                                {
                                    client.SendMessage(e.ChatMessage.Channel, "Shoutout for @" + lS[1] + " which is also a streamer! https://twitch.tv/" + lS[1]);
                                    FullLogWrite("[BOT] Shoutout for @" + lS[1] + " which is also a streamer! https://twitch.tv/" + lS[1], LogTypeArg.Both);
                                }
                                else
                                {
                                    client.SendMessage(e.ChatMessage.Channel, "You must add the name of streamer for shoutout with @ character!");
                                    FullLogWrite("[BOT] You must add the name of streamer for shoutout with @ character!", LogTypeArg.Both);
                                }
                            }
                            catch
                            {
                                client.SendMessage(e.ChatMessage.Channel, "[BOT] You must add the name of streamer for shoutout with @ character!");
                                FullLogWrite("[BOT] You must add the name of streamer for shoutout with @ character!", LogTypeArg.Display);
                            }
                        }
                        else
                        {
                            client.SendMessage(e.ChatMessage.Channel, "Only @" + s_UserName + " and moderators can use the !so command!");
                            FullLogWrite("[BOT] Only @" + s_UserName + " and moderators can use the !so command!", LogTypeArg.Both);
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
                        foreach (var m in s_Mods)
                        {
                            if (m.Length > 0)
                            {
                                mod.Add(m.ToLower());
                            }
                        }
                        string modList = string.Join("|", mod);
                        if (modList.Contains(e.ChatMessage.Username) || e.ChatMessage.Username == s_UserName)
                        {
                            string[] lS = line.Split('@');
                            try
                            {
                                if (lS[1].Length > 0)
                                {
                                    client.SendMessage(e.ChatMessage.Channel, e.ChatMessage.Username + " wishes " + lS[1] + " good luck. May you succeed in whatever you're planning to do!");
                                    FullLogWrite("[BOT] " + e.ChatMessage.Username + " wishes " + lS[1] + " good luck. May you succeed in whatever you're planning to do!", LogTypeArg.Both);
                                }
                                else
                                {
                                    client.SendMessage(e.ChatMessage.Channel, "You must add the name of chatter for good luck command with @ character!");
                                    FullLogWrite("[BOT] You must add the name of chatter for good luck command with @ character!", LogTypeArg.Both);
                                }
                            }
                            catch
                            {
                                client.SendMessage(e.ChatMessage.Channel, "[BOT] You must add the name of chatter for good luck command with @ character!");
                                FullLogWrite("[BOT] You must add the name of chatter for good luck command with @ character!", LogTypeArg.Display);
                            }
                        }
                        else
                        {
                            client.SendMessage(e.ChatMessage.Channel, "Only @" + s_UserName + " can use the !gl command!");
                            FullLogWrite("[BOT] Only @" + s_UserName + " can use the !gl command!", LogTypeArg.Both);
                        }
                    }
                }
            }
            //----------------------------

            //timezone data display 
            s_WeatheCond = e.ChatMessage.Message;
            try
            {
                string[] we = s_WeatheCond.Split(' ');
                string cn1 = string.Empty;
                string cn2 = string.Empty;
                if (we[0].StartsWith("!time"))
                {
                    cn1 = we[1]; //grab Continent name
                    cn2 = we[2]; //grab City Name
                    if (cn1.Length > 0 && cn2.Length > 0)
                    {
                        client.SendMessage(e.ChatMessage.Channel, "The time in " + cn2 + " (" + cn1 + ") is:" + Environment.NewLine + TimeZone(cn1, cn2));
                        FullLogWrite("[BOT] The time in " + cn2 + " (" + cn1 + ") is:" + Environment.NewLine + TimeZone(cn1, cn2), LogTypeArg.Both);
                    }
                    else
                    {
                        client.SendMessage(e.ChatMessage.Channel, "The time command should look like this: !time Continet City_Name");
                        FullLogWrite("[BOT] The time command should look like this: !time Continet City_Name", LogTypeArg.Both);
                    }
                }
            }
            catch
            {
                client.SendMessage(e.ChatMessage.Channel, "The time command should look like this: !time Continet City_Name. Or some information is wrong. Only Cities avaible on http://worldtimeapi.org are displyed!");
                FullLogWrite("[BOT] The time command should look like this: !time Continet City_Name. Or some information is wrong.  Only Cities avaible on http://worldtimeapi.org are displyed!", LogTypeArg.Display);
            }

            //----------------------------

            //!yt command display play link

            if (s_YtControl == "1")
            {
                if (e.ChatMessage.Message == "!yt")
                {
                    client.SendMessage(e.ChatMessage.Channel, "Now playing: " + s_YtLink);
                    FullLogWrite("[BOT] Now playing: " + s_YtLink, LogTypeArg.Both);
                }
            }
            else
            {
                if (e.ChatMessage.Message == "!yt")
                {
                    client.SendMessage(e.ChatMessage.Channel, "Nothing playing at the moment! ");
                    FullLogWrite("[BOT] Nothing playing at the moment! ", LogTypeArg.Both);
                }
            }
            //----------------------------


            //!playlist command
            if (s_ytRequest == "1")
            {

                if (e.ChatMessage.Message == "!playlist")
                {
                    await PlayListLoad();//we reload the playlist to catch the new songs added
                    string pOut = string.Join("", PList);
                    client.SendMessage(e.ChatMessage.Username, "Current playlist: " + Environment.NewLine + pOut + Environment.NewLine);
                    FullLogWrite("[BOT] Current playlist: " + Environment.NewLine + pOut, LogTypeArg.Both);
                }



                if (e.ChatMessage.Message.StartsWith("!rsong"))
                {
                    string sR = e.ChatMessage.Message;
                    string[] sRequest = sR.Split(' ');
                    WebClient wClient = new WebClient();
                    try
                    {
                        foreach (var line in CList)
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
                                int c = File.ReadAllLines(s_PlayListRequest).Count();
                                int i = c + 1;
                                File.AppendAllText(s_PlayListRequest, i + "|" + s[1]);
                                client.SendMessage(e.ChatMessage.Username, "Song added in queue: " + ytTitle);
                                FullLogWrite("[BOT] Song added in queue: " + ytTitle, LogTypeArg.Both);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        client.SendMessage(e.ChatMessage.Username, "Something went wrong with adding song in queue! ");
                        logWrite("[BOT] Something went wrong with adding song in queue! ");
                        CLog.LogWriteError("!rsong Error: " + ex.ToString());
                    }



                }
                if (e.ChatMessage.Message.StartsWith("!showrequest"))
                {
                    string[] playList = File.ReadAllLines(s_PlayListRequest);

                    //we clear for reaload of list
                    QList.Clear();

                    WebClient wClient = new WebClient();

                    if (File.Exists(s_PlayListRequest))
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
                                    QList.Add(index.ToString() + " | " + ytTitle + "  " + Environment.NewLine);
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

                    if (QList.Count > 0)
                    {
                        string pOut = string.Join(" ", QList);
                        client.SendMessage(e.ChatMessage.Username, "Request playlist: " + Environment.NewLine + pOut + Environment.NewLine);
                        FullLogWrite("[BOT] Request playlist: " + Environment.NewLine + pOut, LogTypeArg.Both);
                    }
                    else
                    {
                        client.SendMessage(e.ChatMessage.Username, "There are no songs in request queue!");
                        FullLogWrite("[BOT] There are no songs in request queue!", LogTypeArg.Both);
                    }
                }

            }

            //----------------------------



            //weather data display 
            if (s_WeatherKey == "1" && s_ApiKey != "")
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
                            if (s_WeatherUnits == "1")
                            {
                                client.SendMessage(e.ChatMessage.Channel, "The weather(Celsius) on " + cn + " is:" + Environment.NewLine + WeatherForecast(cn));
                                FullLogWrite("[BOT] The weather(Celsius) in " + cn + " is:" + Environment.NewLine + WeatherForecast(cn), LogTypeArg.Both);
                            }
                            else
                            {
                                client.SendMessage(e.ChatMessage.Channel, "The weather(Fahrenheit) on " + cn + " is:" + Environment.NewLine + WeatherForecast(cn));
                                FullLogWrite("[BOT] The weather(Fahrenheit) in " + cn + " is:" + Environment.NewLine + WeatherForecast(cn), LogTypeArg.Both);
                            }
                        }
                        else
                        {
                            client.SendMessage(e.ChatMessage.Channel, "Weathr not avaible on this City or API Key tries excided!");
                            FullLogWrite("[BOT] Weathr not avaible on this City or API Key tries excided!", LogTypeArg.Both);
                        }
                    }
                }
                catch
                {
                    client.SendMessage(e.ChatMessage.Channel, "Check the City name please!");
                    FullLogWrite("[BOT] Check the City name please!", LogTypeArg.Display);
                }
            }
            else
            {
                client.SendMessage(e.ChatMessage.Channel, "Weather command is disabled for the moment!");
                FullLogWrite("[BOT] Weather command is disabled for the moment!", LogTypeArg.Display);
            }
            //----------------------------

            //Magic 8Ball game command
            if (e.ChatMessage.Message.StartsWith("!8ball"))
            {
                if (e.ChatMessage.Message.Contains("?"))
                {

                    List<string> randomM = new List<string>();
                    if (File.Exists(s_BallAnswer))
                    {
                        bool c = false;
                        string[] rand_list = File.ReadAllLines(s_BallAnswer);
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
                                    FullLogWrite("[" + s_Date + "][BOT] 8Ball - Answers files is empty! You need to add something", LogTypeArg.Display);
                                    c = true;
                                }
                            }
                        }
                        int index = Random8.Next(randomM.Count);
                        string rand = randomM[index];
                        s_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        client.SendMessage(e.ChatMessage.Channel, "Scott says: " + rand);
                        FullLogWrite("[" + s_Date + "][BOT] Scott says: " + rand, LogTypeArg.Both);
                    }
                }
                else
                {
                    s_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    client.SendMessage(e.ChatMessage.Channel, "Your question must contain ? for Scott to answer!");
                    FullLogWrite("[" + s_Date + "][BOT] Your question must contain ? for Scott to answer!", LogTypeArg.Both);
                }
            }
            //----------------------------

        }


        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
            {
                client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
                FullLogWrite(e.Channel + $":  Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!", LogTypeArg.Both);
            }
            else
            {
                client.SendMessage(e.Channel, $" Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
                FullLogWrite(e.Channel + $":  Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!", LogTypeArg.Both);
            }

            //increase subs count
            s_SubsCount++;
        }

        private void Client_OnUserJoinedArgs(object sender, OnUserJoinedArgs e)
        {
            //Enebale user joiend chat only when check is on.
            if (s_BotMSGKey == "1")
            {
                client.SendMessage(e.Channel, $"Welcome to my channel {e.Username}. and thank you for joining. For more commands type !help");
                FullLogWrite(e.Channel + $"  Welcome to my channel {e.Username}. and thank you for joining. For more commands type !help", LogTypeArg.Both);
            }

            //we increment with 1 integer when a person has joined the chat room
            s_Viewers++;
        }

        private void Client_OnUserLeftArgs(object sender, OnUserLeftArgs e)
        {
            //we decrement with 1 integer when a person left the chat
            if (s_Viewers > 0)
            {
                s_Viewers--;
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

                HttpResponseMessage response = s_ClientH.GetAsync(string.Format(html, Region, CityName)).GetAwaiter().GetResult();
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
        private string WeatherForecast(string CityName)
        {

            string _date = DateTime.Now.ToString("yyyy_MM_dd");
            string date2 = string.Empty;
            string errFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\log\errors\" + _date + "_log.txt";
            string outs = string.Empty;
            try
            {

                if (s_ApiKey.Length > 0) // we check the lenght
                {
                    //Open weather map API link with celsius 
                    // TODO: will decide if I put switch for ferenhait
                    string html = @"https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";
                    if (s_WeatherUnits == "1")
                    {
                        html = @"https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";
                    }
                    else
                    {
                        html = @"https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=imperial";
                    }

                    HttpResponseMessage response = s_ClientH.GetAsync(string.Format(html, CityName, Encryption._decryptData(s_ApiKey))).GetAwaiter().GetResult();
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
            viewersLbL.Content = s_Viewers.ToString();
            subsLbL.Content = s_SubsCount.ToString();
            //todo: add subs count on label
            //-----------------------------------

            // Load variables stored in registry.
            ReadRegistryVariables();

            //read mods from file
            if (File.Exists(s_ModsFile))
            {
                s_Mods = File.ReadAllLines(s_ModsFile);
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
                    s_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    client.Connect();
                    if (client.IsConnected)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            startBotBTN.Content = "STOP";
                        });
                        FullLogWrite("[" + s_Date + "]Internet up. Reconnected to " + s_UserName + " channel !", LogTypeArg.Both);
                    }
                }
            }
            else
            {
                if (!client.IsConnected)
                {
                    //we reset the people chat room and new subs counters
                    s_Viewers = 0;
                    viewersLbL.Content = "0";

                    s_SubsCount = 0;
                    subsLbL.Content = "0";
                    //-------------------------------------------
                    this.Dispatcher.Invoke(() =>
                    {
                        startBotBTN.Content = "START";
                        s_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        statIMG.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/red_dot.png"));
                    });
                    string oRTB = ConvertRichTextBoxContentsToString(logViewRTB);
                    if (!oRTB.Contains("[" + s_Date + "] No internet connection at the moment. Trying to reconnect..."))
                    {
                        FullLogWrite("[" + s_Date + "] No internet connection at the moment. Trying to reconnect...", LogTypeArg.Both);
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
                    s_UserName = Reg.regKey_Read(s_KeyName, "UserName");

                    try
                    {

                        s_StreamKey = Encryption._decryptData(Reg.regKey_Read(s_KeyName, "StreamKey"));
                    }
                    catch (Exception x)
                    {
                        FullLogWrite("oAuth decrypt error: " + x.Message, LogTypeArg.Both);
                        s_StreamKey = "error_key";
                    }
                    //-------------------------------------

                    if (client.IsConnected)
                    {
                        BotStop();
                    }
                    else
                    {
                        if (s_UserName.Length > 0)
                        {

                            if (s_StreamKey != "error_key")
                            {
                                DispatcherTimer.Stop();
                                DispatcherTimerR.Stop();
                                Worker = new BackgroundWorker();
                                Worker.DoWork += BotStart;
                                Worker.RunWorkerAsync();
                                DispatcherTimer.Start();
                                DispatcherTimerR.Start();
                            }
                            else
                            {
                                FullLogWrite("Please fill in settings your oAuth Twitch key generated from https://twitchapps.com/tmi/ !", LogTypeArg.Display);

                            }
                        }
                        else
                        {
                            logWrite("Please fill in settings the user name for the Twitch Channel that you want to connect!");
                            FullLogWrite("Please fill in settings the user name for the Twitch Channel that you want to connect!", LogTypeArg.Display);
                        }
                    }
                }
                else
                {
                    FullLogWrite("Twitch tmi.twitch.tv server is down!", LogTypeArg.Display);
                }
            }
            else
            {
                FullLogWrite("No internet connection", LogTypeArg.Display);
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
            Reg.regKey_WriteSubkey(s_KeyName, "Menu", "1");
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
            Reg.regKey_WriteSubkey(s_KeyName, "Menu", "0");
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
            Settings = new settings();
            Settings.ShowDialog();
        }

        //closing all windows
        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                if (YouTube.IsVisible)
                {
                    YouTube.Close();
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
            BotMessage = new botMSG();
            BotMessage.ShowDialog();
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
            Command = new command();
            Command.ShowDialog();
        }

        /// <summary>
        /// Display bad words window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_PreviewMouseDownBAD(object sender, MouseButtonEventArgs e)
        {
            BadWords = new badWords();
            BadWords.ShowDialog();
        }

        /// <summary>
        /// Display YouTube player window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_PreviewMouseDownYT(object sender, MouseButtonEventArgs e)
        {
            YouTube = new youtube();

            //check youtube window control status number
            if (s_YtWin == "0")
            {
                YouTube.Show();
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
            About = new about();
            About.ShowDialog();
        }

        /// <summary>
        /// Random messege sender function and set timer interval for future resend
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RandomMessage(object sender, EventArgs e)
        {
            string dateSent = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            if (s_RandomC == "1")
            {
                List<string> randomM = new List<string>();
                if (File.Exists(s_RandomListFile))
                {
                    s_Rand_list = File.ReadAllLines(s_RandomListFile);
                    foreach (var line in s_Rand_list)
                    {
                        randomM.Add(line);
                    }
                    int index = Random.Next(randomM.Count);
                    string rand = randomM[index];

                    try
                    {
                        if (client.IsConnected)
                        {
                            client.SendMessage(s_UserName, rand);//sending the random message from list
                            logWrite("[Random Message] [Interval set to: " + s_RTime + " minutes]" + rand);
                            CLog.LogWrite("[" + dateSent + "]Random Message: " + rand);
                        }
                        else
                        {
                            logWrite("[Random Message] Client is disconnected!");
                            CLog.LogWriteError("[" + dateSent + "]Random Message: " + rand);
                        }
                    }
                    catch (Exception ex)
                    {
                        CLog.LogWriteError("[" + dateSent + "]Error - Random Message: " + ex.ToString());
                    }

                    if (Convert.ToInt32(s_RTime) > 0)
                    {
                        //set timer interval for nex resend
                        DispatcherTimerR.Interval = new TimeSpan(0, Convert.ToInt32(s_RTime), 0);
                    }
                    else
                    {
                        DispatcherTimerR.Interval = new TimeSpan(0, 10, 0);
                    }
                }
                else
                {
                    logWrite("[Random Message Error] File " + s_RandomListFile + " dose not exist!");
                    CLog.LogWriteError("File " + s_RandomListFile + " dose not exist!");
                }
            }
        }
        /// <summary>
        /// Load playlist and add index number for every song
        /// </summary>
        private async Task PlayListLoad()
        {
            string[] playList = File.ReadAllLines(s_PlayListFile);
            WebClient wClient = new WebClient();

            // We clear for reaload of list.
            PList.Clear();
            CList.Clear();
            if (File.Exists(s_PlayListFile))
            {
                try
                {
                    int index = 0;

                    foreach (var line in playList)
                    {
                        await Task.Delay(200);//i hope is anty spam

                        // Download youtube link source code
                        string titleParse = await wClient.DownloadStringTaskAsync(line);

                        // Grabing the title from source
                        int pFrom = titleParse.IndexOf("<title>") + "<title>".Length;
                        int pTo = titleParse.LastIndexOf("</title>");

                        // Store the title
                        string ytTitle = titleParse.Substring(pFrom, pTo - pFrom);
                        index++;
                        PList.Add(index.ToString() + " | " + ytTitle + "  " + Environment.NewLine);
                        CList.Add(index.ToString() + "|" + line + Environment.NewLine);
                    }

                    index = 0;
                }
                catch (Exception ex)
                {
                    CLog.LogWriteError("[BOT] playListLoad: " + ex.ToString());
                }
            }
            else
            {
                FullLogWrite("[BOT] File " + s_PlayListFile + "dose not exist! Restart application for recreate!", LogTypeArg.Both);
            }
        }



        /// <summary>
        /// we load the list with the requested songs 
        /// </summary>
        private async Task QueueListLoad()
        {
            string[] playList = File.ReadAllLines(s_PlayListRequest);
            //we clear for reaload of list
            QList.Clear();

            WebClient wClient = new WebClient();

            if (File.Exists(s_PlayListRequest))
            {

                int index = 0;

                foreach (var line in playList)
                {
                    if (line.Length > 0)
                    {
                        await Task.Delay(200);//I hope is anty spam
                        try
                        {
                            //Download youtube link source code
                            string[] s = line.Split('|');
                            string titleParse = await wClient.DownloadStringTaskAsync(s[1]);

                            //grabing the title from source
                            int pFrom = titleParse.IndexOf("<title>") + "<title>".Length;
                            int pTo = titleParse.LastIndexOf("</title>");

                            //store the title
                            string ytTitle = titleParse.Substring(pFrom, pTo - pFrom);
                            index++;
                            QList.Add(index.ToString() + " | " + ytTitle + "  " + Environment.NewLine);
                        }
                        catch (Exception ex)
                        {
                            FullLogWrite("[BOT] Queuelistload: " + ex.Message, LogTypeArg.Both);
                        }
                    }

                }
                index = 0;
            }
            else
            {
                FullLogWrite("[BOT] File " + s_PlayListRequest + "dose not exist! Restart application for recreate!", LogTypeArg.Both);
            }
        }


        // Check the file existence and if not exist create it .
        // Used on bot StartUp
        private static void CheckFileStartUp(List<string> files)
        {
            foreach (var fileName in files)
            {
                if (!File.Exists(fileName))
                    File.Create(fileName);
            }
        }

        // Check the directory existence and if not exist create it .
        // Used on bot StartUp
        private static void CheckDirectoryStartUp(List<string> directories)
        {
            foreach (var directoryName in directories)
            {
                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);
            }
        }

        // We load the varibles stored on registry.
        private void ReadRegistryVariables()
        {
            s_UserName = Reg.regKey_Read(s_KeyName, "UserName");

            try
            {

                s_StreamKey = Encryption._decryptData(Reg.regKey_Read(s_KeyName, "StreamKey"));
            }
            catch (Exception x)
            {
                CLog.LogWriteError("oAuth decrypt error: " + x.ToString());
                s_StreamKey = "error_key";
            }

            s_BotMSGKey = Reg.regKey_Read(s_KeyName, "BotMSG");
            _TimeBan = Int32.Parse(Reg.regKey_Read(s_KeyName, "WordBanTime"));
            s_BadWord = Reg.regKey_Read(s_KeyName, "BadWord");
            s_ApiKey = Reg.regKey_Read(s_KeyName, "WeatherAPIKey");
            s_YtControl = Reg.regKey_Read(s_KeyName, "YTControl");
            s_WeatherKey = Reg.regKey_Read(s_KeyName, "WeatherMSG");
            s_YtWin = Reg.regKey_Read(s_KeyName, "YtWin");
            s_StartMessage = Reg.regKey_Read(s_KeyName, "StartMessage");
            s_YtLink = Reg.regKey_Read(s_KeyName, "YtLink");
            s_BotMSGControl = Reg.regKey_Read(s_KeyName, "botMSGControl");
            s_WeatherUnits = Reg.regKey_Read(s_KeyName, "weatherUnits");
            s_RandomC = Reg.regKey_Read(s_KeyName, "randomC");
            s_RTime = Reg.regKey_Read(s_KeyName, "rTime");
            s_ytRequest = Reg.regKey_Read(s_KeyName, "ytRequest");
        }

        // Set state of menu bar from small to large dependeing on status read from registry and previous stored
        private void MenuStatusChange(string menuStatus)
        {
            if (menuStatus == "1")
            {
                GridMenu.Width = 199;
                btnOpenMenu.Visibility = Visibility.Collapsed;
                btnCloseMenu.Visibility = Visibility.Visible;
                startBotBTN.Visibility = Visibility.Visible;
                logViewRTB.Margin = new Thickness(199, 50, 0, 0);
                return;
            }
            GridMenu.Width = 50;
            logViewRTB.Margin = new Thickness(50, 50, 0, 0);
            btnOpenMenu.Visibility = Visibility.Visible;
            btnCloseMenu.Visibility = Visibility.Collapsed;
            startBotBTN.Visibility = Visibility.Hidden;
        }

        // Load playlist/ and qlist and titles for song request command
        private async void xBot_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.WhenAll(new Task[] { Task.Run(() => PlayListLoad()), Task.Run(() => QueueListLoad()) });
        }
    }
}
