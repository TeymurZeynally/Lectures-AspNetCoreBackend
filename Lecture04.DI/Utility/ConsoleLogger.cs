namespace Lecture04.DI.Utility
{
    public class ConsoleLogger : IConsoleLogger
    {
        public ConsoleLogger()
        {
            Console.WriteLine($"Создался класс ConsoleLogger {new Random().Next(0, int.MaxValue)}");
        }


        public void Log(string message) => Console.WriteLine(message);
    }
}
