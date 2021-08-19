using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using Core;

namespace xBot_WPF.Utils
{
    public class LogWriter : Window
    {
        /*Log Writer class for WPF*/

        private void LogWrite(string data, RichTextBox richTextBox)
        {    
            Dispatcher.Invoke(() =>
            {
                if (!string.IsNullOrEmpty(data))
                {
                    //outs += data + Environment.NewLine;
                    richTextBox.Document.Blocks.Add(new Paragraph(new Run(data)));
                    richTextBox.ScrollToEnd();
                }
            });
        }

        /// <summary>
        /// We write log in RTB/File/Both
        /// </summary>
        /// <param name="logData"> Data to be writen or displayed</param>
        /// <param name="richTextBox"> The RichTextBox where the data to be displayed</param>
        /// <param name="type">Action for log: Display (Show in RTB) | File (save data in file) | Both (Save and display the data)</param>
        public void FullLogWrite(string logData, RichTextBox richTextBox, LogTypeArg type)
        {
            switch (type)
            {
                case LogTypeArg.Display: // Write in display
                    LogWrite(logData, richTextBox);
                    break;
                case LogTypeArg.File: // Write in file
                    CLog.LogWrite(logData);
                    break;
                case LogTypeArg.Both: //Write both
                    LogWrite(logData, richTextBox);
                    CLog.LogWrite(logData);
                    break;
            }
        }

        // We enumerate the types of Log types
        public enum LogTypeArg
        {
            Display = 1,
            File = 2,
            Both = 3
        }
    }
}
