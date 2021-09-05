using System;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using System.Text.RegularExpressions;

namespace xBot_WPF.Settings
{
    public class ModeratorManagement
    {
        /// <summary>
        /// Add moderator toa listview.
        /// </summary>
        /// <param name="modFile">File name where the moderators name is stored.</param>
        /// <param name="modName">Moderator name for textbox.</param>
        /// <param name="modList">List name where wil be displayed.</param>
        public static void AddModeratorToList(string modFile, TextBox modName, ListView modList)
        {

            if (File.Exists(modFile))
            {
                MessageBox.Show("File " + modFile + " dose not exist!");
                return;
            }

            if (modName.Text.Length > 0)
            {
                File.AppendAllText(modFile, modName.Text + Environment.NewLine);
                modList.Items.Add(modName.Text);
                modName.Clear();
                return;
            }
            MessageBox.Show("You must fill the empy text box!");
        }

        /// <summary>
        /// Remove a selected moderator from list.
        /// </summary>
        /// <param name="modFile">File name where the moderators name is stored.</param>
        /// <param name="modListView">List name where moderators are displayed</param>
        public static void RemoveSelectedModerator(string modFile, ListView modListView)
        {
            string line;
            string bFile = File.ReadAllText(modFile);

            if (!File.Exists(modFile))
            {
                MessageBox.Show("File " + modFile + " dose not exist!");
                return;
            }

            if (modListView.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Moderator username for delete!");
                return;
            }

            using (var sr = new StringReader(modListView.SelectedItem.ToString()))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    bFile = bFile.Replace(line, "");
                }

                modListView.Items.Remove(modListView.SelectedItem.ToString());
                bFile = Regex.Replace(bFile, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);//remove empty lines
                File.WriteAllText(modFile, bFile);
            }
        }

        /// <summary>
        /// Load mods list in listview.
        /// </summary>
        /// <param name="modFile"> File name where the moderators name is stored. </param>
        /// <param name="modList"> List name where moderators will be displayed. </param>
        public static void LoadModerators(string modFile, ListView modList)
        {
            if (!File.Exists(modFile))
            {
                MessageBox.Show("File " + modFile + " dose not exist!");
                return;
            }
            // We add every mod from external file to listview.
            string[] bList = File.ReadAllLines(modFile);
            foreach (var line in bList)
            {
                if (line.Length > 0)
                {
                    modList.Items.Add(line);
                }
            }
        }
    }
}
