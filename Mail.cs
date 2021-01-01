using System.Net;
using System.Net.Mail;

namespace Stock
{
    public class Mail
    {
        private string host;
        private string username;
        private string password;
        private string from;
        private string recipients;
        private string subject;
        public string body;

        public Mail(
            string host, string username, string password, string from, string recipients,
            string subject, string body
        ) {
            this.host = host;
            this.username = username;
            this.password = password;
            this.from = from;
            this.recipients = recipients;
            this.subject = subject;
            this.body = body;
        }
        public void SendMail(){
            SmtpClient client = new SmtpClient(host, 587){
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };
            client.Send(from, recipients, subject, body);
        }    
    }
}