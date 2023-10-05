using System.Data.Entity;
using MailingAppMVC.Models.Domains;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.BuilderProperties;

namespace MailingAppMVC.Models
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<SendedEmail> SendedEmails { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
        }
    }
}