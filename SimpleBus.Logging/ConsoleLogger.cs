using System;
using SimpleBus.Contract.Core;

namespace SimpleBus.Logging
{
    public class ConsoleLogger : ILogger
    {
        private static readonly object _mutex = new object();
        public static Func<DateTimeOffset> TimestampFunc = () => DateTimeOffset.UtcNow;

        public void Debug(string format, params object[] args)
        {
            lock (_mutex)
            {
                OutputMessage(format, args, ConsoleColor.DarkGreen);
            }
        }

        public void Info(string format, params object[] args)
        {
            lock (_mutex)
            {
                OutputMessage(format, args, ConsoleColor.Blue);
            }
        }

        public void Warn(string format, params object[] args)
        {
            lock (_mutex)
            {
                OutputMessage(format, args, ConsoleColor.Yellow);
            }
        }

        public void Error(string format, params object[] args)
        {
            lock (_mutex)
            {
                OutputMessage(format, args, ConsoleColor.Red, ConsoleColor.Gray);
            }
        }

        public void Error(Exception exc, string format, params object[] args)
        {
            lock (_mutex)
            {
                OutputMessage(format, args, ConsoleColor.Red, ConsoleColor.Gray);
                Console.WriteLine(exc.ToString());
            }
        }

        private static void OutputMessage(string format, object[] args, ConsoleColor textColor = ConsoleColor.White, ConsoleColor backgroundColor=ConsoleColor.Black)
        {
            string prefix = TimestampFunc().ToLocalTime().ToString();
            string message = string.Format(format, args);
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine("{0}: {1}", prefix, message);
            Console.ResetColor();
        }
    }
}