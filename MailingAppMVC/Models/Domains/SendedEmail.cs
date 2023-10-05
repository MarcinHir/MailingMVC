using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MailingAppMVC.Models.Domains
{
    public class SendedEmail
    {
        public int Id { get; set; }
        
        [Display(Name = "Nadawca")]
        public string SenderName { get; set; }
        
        [Display(Name = "Tytuł")]
        public string Title { get; set; }
        
        [Display(Name = "Odbiorca")]
        public string Email { get; set; }
        
        [Display(Name = "Data wysłania")]
        public DateTime SendedDate { get; set; }
        
        [Display(Name = "Treść")]
        public string Body { get; set; }
     
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}