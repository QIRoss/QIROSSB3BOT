using System;
using System.Configuration;
using System.Timers;

namespace Stock {
    class Program {
        private static Timer _aTimer;
        
        public static int Main(string[] arg) {
            if (arg.Length != 3){
                Console.WriteLine(
                    "Use: ./Program <Active stock to monitor> <Price to sell (xx.xx)> <Price to buy (xx.xx)>"
                );
                return 1;
            }
            
            var user = ConfigurationManager.AppSettings["username"];
            var password = ConfigurationManager.AppSettings["password"];
            var apiUrl = ConfigurationManager.AppSettings["apiurl"];
            var host = ConfigurationManager.AppSettings["host"];
            var from = ConfigurationManager.AppSettings["from"];
            var to = ConfigurationManager.AppSettings["recipients"];
            var subject = ConfigurationManager.AppSettings["subject"];
            var defaultBody = ConfigurationManager.AppSettings["defaultBody"];
            var onBuying = ConfigurationManager.AppSettings["bodyOnBuying"];
            var onSelling = ConfigurationManager.AppSettings["bodyOnSelling"];
            var connection = new Api(arg[0],float.Parse(arg[1]),float.Parse(arg[2]),apiUrl);
            var mail = new Mail(host,user,password,from,to,subject,defaultBody+'\n'+'\n');
            
            void OnTimedEvent(Object source, ElapsedEventArgs e) {
                Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
                int result = connection.GetPrice();
                if (result != 0)
                {
                    if (result < 0)
                        mail.body += onBuying + arg[0];
                    else
                        mail.body += onSelling + arg[0];
                    mail.SendMail();
                }
            }

            _aTimer = new Timer {Interval = 300000};
            _aTimer.Elapsed += OnTimedEvent;
            _aTimer.AutoReset = true;
            _aTimer.Enabled = true;
            Console.ReadLine(); 
            return 0;
        }
    }
}