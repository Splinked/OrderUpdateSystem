using OrderUpdateSystem.Interfaces;

namespace OrderUpdateSystem.Services
{
    public class MailService : INotificationService
    {
        readonly string address;
        readonly string subject;

        public MailService(string adrss, string subject)
        {
            this.address = adrss;
            this.subject = subject;
        }

        public bool Notify(string msg)
        {
            //pseudo code
            //once full mail service is in place attempt to send mail and wait for response for 30 seconds
            Console.WriteLine("Email service invoked: " + msg);
            return true;

        }
    }
}
