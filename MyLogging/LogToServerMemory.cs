namespace CollegeApp.MyLogging
{
    public class LogToServerMemory : IMyLogger
    {
        public void Log(string message)
        {
            // Code to log the message to server memory
            Console.WriteLine($"Logging to server memory: {message}");
        }
    }
}