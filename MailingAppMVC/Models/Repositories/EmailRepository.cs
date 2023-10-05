using MailingAppMVC.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace MailingAppMVC.Models.Repositories
{
    public class EmailRepository
    {
        public List<SendedEmail> GetEmails(string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.SendedEmails
                    .Where(x => x.UserId == userId)
                    .ToList();
            }
        }
        public void Add(SendedEmail sendedEmail)
        {
            using (var context = new ApplicationDbContext())
            {
                sendedEmail.SendedDate = DateTime.Now;
                context.SendedEmails.Add(sendedEmail);
                context.SaveChanges();
            }
        }
    }
}