namespace UniWeb.API.Models
{
    public class SMTPSettings
    {
        public string Server { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool EnableSSL { get; set; }

        public string FromMail { get; set; }
    }
}
