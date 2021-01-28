using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class Message
    {
        public string event_name { get; set; }
        public string app_id { get; set; }
        public Sender Sender { get; set; }
        public Recipient Recipient { get; set; }
        public Mes message { get; set; }
        public string timestamp { get; set; }
        public string user_id_by_app { get; set; }
    }

    public class Sender
    {
        public string Id { get; set; }
    }

    public class Recipient
    {
        public string Id { get; set; }
    }

    public class Mes
    {
        public string Text { get; set; }
        public string MessageId { get; set; }
    }
}