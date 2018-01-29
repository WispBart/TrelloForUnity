using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wispfire.BugReporting
{
    public class SessionLogger : MonoBehaviour
    {
        public List<LogEntry> SessionLogs = new List<LogEntry>();

        public void HandleLog(string log, string stacktrace, LogType type)
        {
            //Perhaps filter by type?
            SessionLogs.Add(new LogEntry(log, stacktrace, type));
        }

        public string PrintLog()
        {
            string output = string.Empty;
            for (int i = 0; i < SessionLogs.Count; i++)
            {
                output += SessionLogs[i].Log + "\n";
                output += "Stacktrace: \n" + SessionLogs[i].Stacktrace + "\n";
                output += "\n";
            }
            return output;
        }


        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }


        [System.Serializable]
        public struct LogEntry
        {
            public string Log;
            public string Stacktrace;
            public LogType Type;

            public LogEntry(string log, string stacktrace, LogType type)
            {
                Log = log;
                Stacktrace = stacktrace;
                Type = type;
            }
        }
    }
}

