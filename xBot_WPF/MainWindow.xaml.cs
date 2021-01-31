using System;
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
using Ini;
using System.IO;
using System.Reflection;
using Core;
using Microsoft.Win32;
using System.Threading;
using System.Net.Http;
using System.ComponentModel;

namespace xBot_WPF
{
    /*
       Description: This a simple Twitch bot created with passion and fun entirely live on steam.
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
        //------------------------------------------------


        //Declare keyname and subkey
        readonly static string keyName = "xBot";
        private static string weatherKey = "0";
        private static string botMSGKey = "0";
        private static string botMSGControl = "0";
        private static string weatherUnits = "0";
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

        //Define the background worker for bot start
        BackgroundWorker worker;
        //--------------------------------

        //Declare mutex variable for startup instance check
        Mutex myMutex;
        //---------------------------------

        int Viewers = 0;

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
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }
            //------------------------------------------------


            //log directory check and create if not exists
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
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

            if (Reg.regKey_Read(keyName, "botMSGControl") == "")
            {
                Reg.regKey_CreateKey(keyName, "botMSGControl", "0");
            }

            if (Reg.regKey_Read(keyName, "weatherUnits") == "")
            {
                Reg.regKey_CreateKey(keyName, "weatherUnits", "0");
            }
            //-----------------------------------------


            #region Load and display username, streamkey and dark mode control from registry

            t_userName = Reg.regKey_Read(keyName, "UserName");

            try
            {

                t_streamKey = Encryption._decryptData(Reg.regKey_Read(keyName, "StreamKey"));
            }
            catch (Exception)
            {
                //we bypass the error if no data found in ini for aouth key
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
            #endregion

            //staus check timer start
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += StatusLoadIcon;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            //----------------------------------

            //hide start button
            startBotBTN.Visibility = Visibility.Hidden;
            //----------------------------------


            //reset Youtube Controler and window on bot start in case of cras
            Reg.regKey_WriteSubkey(keyName, "YTControl", "0");
            Reg.regKey_WriteSubkey(keyName, "YtWin", "0");
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
                logViewRTB.Document.Blocks.Clear();
            });
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnWhisperReceived += Client_OnWhisperReceived;
            client.OnNewSubscriber += Client_OnNewSubscriber;
            client.OnConnected += Client_OnConnected;
            client.OnUserJoined += Client_OnUserJoinedArgs;
            client.OnUserLeft += Client_OnUserLeftArgs;
            client.Connect();
            this.Dispatcher.Invoke(() =>
            {
                startBotBTN.Content = "STOP";
            });
            //we check if bot is connected and display the log info
            if (client.IsConnected)
            {
                logWrite("[" + date + "] xBot Connected to " + t_userName + " channel !");
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
            }
            if (!client.IsConnected)
            {
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                logWrite("[" + date + "] xBot Disconncted!");
                CLog.LogWrite("[" + date + "] xBot Disconncted!");
                startBotBTN.Content = "START";
                Viewers = 0;
                viewersLbL.Content = "0";
            }
        }

        #region Client events
        private void Client_OnLog(object sender, OnLogArgs e)
        {
            //  CLog.LogWrite($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
            //for future work

        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
            //-----------

            //on bad workd received

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
            //-----------------------------------------------

            //on command received
            comandList = File.ReadAllLines(comDirectory);
            foreach (var com in comandList)
            {
                if (com.Length > 0)
                {
                    string[] s = com.Split(':');

                    if (e.ChatMessage.Message.Contains(s[0]))
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
                if (e.ChatMessage.Message.Contains("!help"))
                {
                    client.SendMessage(e.ChatMessage.Channel, "List of commands is: " + listCMD + "; !yt; !weather; !ss");
                    logWrite("[BOT] List of commands is: " + listCMD + "; !yt; !weather; !ss");
                    CLog.LogWrite("[BOT] List of commands is: " + listCMD + "; !yt; !weather; !ss");
                }
            }
            else
            {
                if (e.ChatMessage.Message.Contains("!help"))
                {
                    client.SendMessage(e.ChatMessage.Channel, "List of commands is: " + listCMD + "; !yt");
                    logWrite("[BOT] List of commands is: " + listCMD + "; !yt");
                    CLog.LogWrite("[BOT] List of commands is: " + listCMD + "; !yt");
                }
            }
            //----------------------------

            //shout streamer commad
            using (var sReader = new StringReader(e.ChatMessage.Message))
            {
                string line;
                while ((line = sReader.ReadLine()) != null)
                {
                    if (line.Contains("!ss"))
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
                            CLog.LogWrite("[BOT] You must add the name of streamer for shoutout with @ character!");

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
                    if (line.Contains("!gl"))
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
                            CLog.LogWrite("[BOT] You must add the name of chatter for good luck command with @ character!");
                        }
                    }
                }
            }
            //----------------------------

            //weather data display 
            if (weatherKey == "1" && apiKey != "")
            {
                weatheCond = e.ChatMessage.Message;
                try
                {
                    string[] we = weatheCond.Split(' ');
                    string cn = string.Empty;
                    if (we[0] == "!weather")
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
                    CLog.LogWrite("[BOT] Check the City name please!");
                }
            }
            else
            {
                client.SendMessage(e.ChatMessage.Channel, "Weather command is disabled for the moment!");
                logWrite("[BOT] Weather command is disabled for the moment!");
                CLog.LogWrite("[BOT] Weather command is disabled for the moment!");
            }
            //----------------------------

            //!yt command display play link

            if (ytControl == "1")
            {
                if (e.ChatMessage.Message.Contains("!yt"))
                {
                    client.SendMessage(e.ChatMessage.Channel, "Now playing: " + YtLink);
                    logWrite("[BOT] Now playing: " + YtLink);
                    CLog.LogWrite("[BOT] Now playing: " + YtLink);
                }
            }
            else
            {
                if (e.ChatMessage.Message.Contains("!yt"))
                {
                    client.SendMessage(e.ChatMessage.Channel, "Nothing playing at the moment! ");
                    logWrite("[BOT] Nothing playing at the moment! ");
                    CLog.LogWrite("[BOT] Nothing playing at the moment! ");
                }
            }
            //----------------------------
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            if (e.WhisperMessage.Username == "my_friend")
            {
                client.SendWhisper(e.WhisperMessage.Username, "Hey! Whispers are so cool!!");
                CLog.LogWrite(e.WhisperMessage.Username + ": Hey! Whispers are so cool!! ");
            }
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
            Viewers++;
        }

        private void Client_OnUserLeftArgs(object sender, OnUserLeftArgs e)
        {
            if (Viewers > 0)
            {
                Viewers--;
            }
        }


        #endregion


        /// <summary>
        /// weather api check and return the output parssed
        /// </summary>
        /// <param name="CityName"></param>
        /// <returns></returns>
        private string weatherForecast(string CityName)
        {
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
                logWrite("Weather error: " + e.ToString());
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
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Load icon bubble for bot connection status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusLoadIcon(object sender, EventArgs e)
        {
            //Display total users from stream
            viewersLbL.Content = Viewers.ToString();
            //-----------------------------------

            //read variables form registry
            t_userName = Reg.regKey_Read(keyName, "UserName");

            try
            {

                t_streamKey = Encryption._decryptData(Reg.regKey_Read(keyName, "StreamKey"));
            }
            catch (Exception)
            {
                //we bypass the error if no data found in ini for aouth key
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
            //-----------------------------------

            if (client.IsConnected)
            {


                statIMG.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/green_dot.png"));

            }
            else
            {
                statIMG.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/red_dot.png"));
            }
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
                if (Network.portCheck("tmi.twitch.tv", 80))//check twithc server
                {
                    if (client.IsConnected)
                    {
                        BotStop();
                    }
                    else
                    {
                        if (t_userName.Length > 0)
                        {

                            if (t_streamKey.Length > 0)
                            {
                                worker = new BackgroundWorker();
                                worker.DoWork += BotStart;
                                worker.RunWorkerAsync();
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
    }
}
