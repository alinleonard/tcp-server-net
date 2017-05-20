using System;
namespace TCPServer
{
    public class Log
    {
        public Log()
        {
               
        }

        public static string GetSourceNameInEventViewer()
        {
            return AppDomain.CurrentDomain.FriendlyName;
        }

        public static void WriteInformationLog(string sLog)
        {
            // Create an instance of EventLog
            System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog();

            // Check if the event source exists. If not create it.
            if (!System.Diagnostics.EventLog.SourceExists(AppDomain.CurrentDomain.FriendlyName))
            {
                System.Diagnostics.EventLog.CreateEventSource(AppDomain.CurrentDomain.FriendlyName, "Application");
            }

            // Set the source name for writing log entries.
            eventLog.Source = AppDomain.CurrentDomain.FriendlyName;

            // Create an event ID to add to the event log
            int eventID = 8;

            // Write an entry to the event log.
            eventLog.WriteEntry(sLog,
                                System.Diagnostics.EventLogEntryType.Information,
                                eventID);

            // Close the Event Log
            eventLog.Close();
        }

        public static void WriteErrorLog(string sLog)
        {
            // Create an instance of EventLog
            System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog();

            // Check if the event source exists. If not create it.
            if (!System.Diagnostics.EventLog.SourceExists(AppDomain.CurrentDomain.FriendlyName))
            {
                System.Diagnostics.EventLog.CreateEventSource(AppDomain.CurrentDomain.FriendlyName, "Application");
            }

            // Set the source name for writing log entries.
            eventLog.Source = AppDomain.CurrentDomain.FriendlyName;

            // Create an event ID to add to the event log
            int eventID = 8;

            // Write an entry to the event log.
            eventLog.WriteEntry(sLog,
                                System.Diagnostics.EventLogEntryType.Error,
                                eventID);

            // Close the Event Log
            eventLog.Close();
        }
    }
}
