namespace Lecture04.DI.Notifications
{
    public class EmailNotifier : INotifier
    {

        private readonly string _smtpHost;

        public EmailNotifier(string smtpHost)
        {
            _smtpHost = smtpHost;
            Console.WriteLine($"Создался класс EmailNotifier {new Random().Next(0, int.MaxValue)}");
        }

        public void Notify(string message) => Console.WriteLine($"[EMAIL via {_smtpHost}] {message}");
    }

}
