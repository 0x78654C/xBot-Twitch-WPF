![alt text](https://github.com/0x78654C/xBot-Twitch-WPF/blob/main/xBot_WPF/l1.png?raw=true)

# xBot-Twitch-WPF
 
xBot - Twitch bot in csharp WPF



![alt text](https://github.com/0x78654C/xBot-Twitch-WPF/blob/main/xBot_WPF/t1.png?raw=true)

![alt text](https://github.com/0x78654C/xBot-Twitch-WPF/blob/main/xBot_WPF/t2.png?raw=true)


This project was made entirely on stream as fun. I'm not a professional programmer and I do this as a hobby but when I put something in my mind I try solve/create it.

using following libs:

https://github.com/TwitchLib/TwitchLib (for api connect)

https://github.com/davcs86/csharp-uhwid (for oath hwid encrypt/decrypt)
_____________________________________________________

**Requirement**: 

.NET Framework 4.7

_____________________________________________________


Project ongoing live on: https://www.twitch.tv/x_coding

**Features**:

    show status if client is connected
    saveing twitch user name, openweather api key and oauth key to registry
    set custom bot connect message in real time
    add/remove/update commnads in real time to list
    add/remove bad words for chat ban in real time to list
    change time(minutes) for chat ban in real time
    hardcoded !help command for display the list of commands
    hardcoded !ss command for shout a streamer (the command works only for the current streamer)
    hardcoded !yt command for youtube current song played on xBot youtube player
    hardcoded !weather for Weather Forecast(Fahrenheit and Celsius units) command using openweather.org API
    hardcoded !gl for whishing good luck to some one in chat from you  (the command works only for the current streamer)
    hardcoded !time command for display time on a specified city. ex: !time America New_York (Used API from: http://worldtimeapi.org) 
    display people on chat room count on interface
    automaticaly reconnect to twitch channel if internet connection crashed and is up again 
    possibility to send random messages to stream at a certain time interval
    new subscribers counter(the conter will reset when the bot is disconnected or closed)
	play next song on youtube player
	----------------beta-------------------
	Play requested songs feature (beta):
		Viewers can add songs in a list queue that will be played . After song is played it will deleted from list.
		!playlist - see predefined list with songs created by streamer
		!rsong - add a song from playlist in queue Ex.: rsong 1
		!showrequest - displays the requested songs by viewers
	---------------------------------------	

(more to come)
