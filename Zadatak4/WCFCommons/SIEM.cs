using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFCommons
{
    public class SIEM
    {
        static SIEM instance = null;

        EventLog log;
        string appName = "Projekat 4";

        private SIEM()
        {
            if (!EventLog.SourceExists(appName))
            {
                EventLog.CreateEventSource(appName, "Application");
            }

            log = new EventLog("Application", Environment.MachineName, appName);
        }

        ~SIEM()
        {
            log.Close();
        }

        public static SIEM GetInstance()
        {
            if(instance == null)
            {
                instance = new SIEM();
            }
            return instance;
        }

        public void LogInformation(string message)
        {
            log.WriteEntry(message, EventLogEntryType.Information);
        }

        public void LogWarning(string message)
        {
            log.WriteEntry(message, EventLogEntryType.Warning);
        }

        public void LogError(string message)
        {
            log.WriteEntry(message, EventLogEntryType.Error);
        }
    }
}
