using MailingAppMVC.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MailingAppMVC.Models.ViewModels
{
    public class HistoryViewModel
    {
        public SendedEmail SendedEmail { get; set; }
        public string Heading { get; set; }
    }
}