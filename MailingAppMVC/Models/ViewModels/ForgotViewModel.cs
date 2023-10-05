using System.ComponentModel.DataAnnotations;

namespace MailingAppMVC.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
