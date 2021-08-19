using System.Collections.Generic;
using System.Windows.Controls;
using System.IO;

namespace xBot_WPF.YouTube
{
    /*Class for load song list requested and play list links load in listbox*/
    public class PlayList
    {

        /// <summary>
        /// Load Song Request in list.
        /// </summary>
        /// <param name="playListRequestedFile"> File containing the requested songs</param>
        /// <param name="songListRequested"> The list where we store the requested songs</param>
        public static void LoadSongsInList(string playListRequestedFile, List<string> songListRequested)
        {
            string[] lines = File.ReadAllLines(playListRequestedFile);
            foreach (var line in lines)
            {
                if (line.Length > 0)
                {
                    songListRequested.Add(line);
                }
            }
        }

        /// <summary>
        /// Load links from playslistfile in listbox.
        /// </summary>
        /// <param name="playListFile">File containing the play list links.</param>
        /// <param name="listBox">ListBox where we display the links.</param>
        public static void LoadPlayListLinks(string playListFile, ListBox listBox)
        {
            string[] yLinks = File.ReadAllLines(playListFile);

            foreach (var line in yLinks)
            {
                if (line.Length > 0)
                {
                    listBox.Items.Add(line);
                }
            }
        }
    }
}
