namespace CollegeApp.MyLogging
{
   public class LogToDB : IMyLogger
    {
        public void Log(string message)
        {
            // Code to log the message to a database
            Console.WriteLine($"Logging to DB: {message}");
        }
    }
}