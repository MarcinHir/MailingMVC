using System.ComponentModel.DataAnnotations;

namespace MailingAppMVC.Models.ViewModels
{
    public class MailSenderViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]        
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Treść")]        
        public string Body { get; set; }

        [Display(Name = "Nadawca")]
        public string SenderName { get; set; }    
    }
}