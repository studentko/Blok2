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
        static object lObject = new object();

        EventLog log;

        public SIEM(string source)
        {
            lock (lObject)
            {
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, "Application");
                }
                log = new EventLog("Application", Environment.MachineName, source);
            }
        }

        ~SIEM()
        {
            log.Close();
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
