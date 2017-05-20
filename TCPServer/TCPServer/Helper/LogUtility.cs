using System;
namespace TCPServer.Helper
{
    public class LogUtility
    {
        public LogUtility()
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

        /// <summary>
        /// Write to Event Viewer the error (string) you want to save under Application.
        /// </summary>
        /// <param name="sLog"></param>
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

        /// <summary>
        /// Write under the Event Viewer the exception you want to save. Provide only the exception from the try catch.
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteExceptionLog(Exception ex)
        {
            // Create an instance of EventLog
            System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog();

            try
            {
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
                eventLog.WriteEntry(string.Format("Message:\n{0}\nStackTrace:\n{1}", ex.Message, ex.StackTrace),
                                    System.Diagnostics.EventLogEntryType.Error,
                                    eventID);
            }
            catch (Exception exception)
            {
                Console.WriteLine();
                Console.WriteLine("Please fix the LogUtility before using again the solution");
                Console.WriteLine();
                Console.WriteLine(string.Format("Message:\n{0}\nStackTrace:\n{1}", exception.Message, exception.StackTrace));

                Console.WriteLine();
                Console.WriteLine("Exception that was unable to save:");
                Console.WriteLine();
                Console.WriteLine(string.Format("Message:\n{0}\nStackTrace:\n{1}", ex.Message, ex.StackTrace));
            }

            // Close the Event Log
            eventLog.Close();
        }
    }
}
