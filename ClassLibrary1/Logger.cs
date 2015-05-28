using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace CustomUtil
{
    public static class Logger
    {
        public static string directory;

        public static void logError(string msg)
        {
            log(msg, "Error");
        }

        public static void logInfo(string msg)
        {
            log(msg, "Info");
        }

        public static void logWarning(string msg)
        {
            log(msg, "Warning");
        }

        private static void log(string msg, string keyword)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(new FileInfo(System.AppDomain.CurrentDomain.BaseDirectory).DirectoryName + @"\TeamRating.log", true);
            file.WriteLine(DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + "|" + keyword + ": " + msg);
            file.Close();
        }

        public static void journalEntry(string msg)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(new FileInfo(System.AppDomain.CurrentDomain.BaseDirectory).DirectoryName + @"\journal.txt", true);
            file.WriteLine(msg);
            file.Close();
        }
    }
}
