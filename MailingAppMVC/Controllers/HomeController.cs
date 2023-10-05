using MailingAppMVC.Models.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity.Utilities;
using MailingAppMVC.Models.Repositories;
using MailingAppMVC.Models.Domains;

namespace MailingAppMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private EmailRepository _emailRepository = new EmailRepository();
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Aplikacja do wysyłania maili";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Formularz E-mail";

            return View();
        }

        public ActionResult MailSender()
        {
            ViewBag.Message = "Wysyłka E-mail";

            return View();
        }

        public ActionResult History() 
        {
            ViewBag.Message = "Historia Wysłanych E-maili";
            var userId = User.Identity.GetUserId();
            var emails = _emailRepository.GetEmails(userId);
            return View(emails);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MailSender(MailSenderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                SendedEmail sendedEmail = new SendedEmail();
                EmailService emailService = new EmailService();    
                    IdentityMessage identityMessage = new IdentityMessage();
                    IdentityMessage identityMessage2 = identityMessage;
                    identityMessage2.Destination = model.Email;
                    identityMessage.Subject = model.Title;
                    identityMessage.Body = model.Body;
                    await emailService.SendAsync(identityMessage, model.SenderName);
                sendedEmail.Email = model.Email;
                sendedEmail.SenderName = model.SenderName;
                sendedEmail.Title = model.Title;
                sendedEmail.Body = model.Body;
                sendedEmail.Title = model.Title;
                sendedEmail.UserId = userId;
                _emailRepository.Add(sendedEmail);




                return RedirectToAction("Index", "Home");
             
            }
            return View(model);
        }
    }
}