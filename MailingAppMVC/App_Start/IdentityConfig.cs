using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using MailingAppMVC.Models;
using EmailSender;
using System.Configuration;
using Cipher;
using System.Web.Configuration;

namespace MailingAppMVC
{
    public class EmailService : IIdentityMessageService
    {
        private Email _email;
        private StringCipher _stringCipher = new StringCipher("7D195B61-76A4-404A-AD0C-BB0B9D655634");
        private const string NotEncryptedPasswordPrefix = "encrypt:";
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task SendAsync(IdentityMessage message)
        {

            try
            {
                Logger.Info("Email params zaczyna działanie");

                _email = new Email(new EmailParams
                {
                    HostSmtp = ConfigurationManager.AppSettings["HostSmtp"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    SenderName = ConfigurationManager.AppSettings["SenderName"],
                    SenderEmail = ConfigurationManager.AppSettings["SenderEmail"],
                    SenderEmailPassword = DecryptSenderEmailPassword()
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                Logger.Error("Email params nie działa");
                throw new Exception(ex.Message);
            }

            try
            {
                await _email.Send(message.Subject, message.Body, message.Destination);
            }
            catch (Exception ex)
            {

                Logger.Error(ex, ex.Message);
                Logger.Error("Send email  nie działa");
                throw new Exception(ex.Message);
            }

            Logger.Info("email send działa");
        }



        public async Task SendAsync(IdentityMessage message, string senderName)
        {
            try
            {
                Logger.Info("Email params zaczyna działanie");
                if (senderName == null)
                {
                    _email = new Email(new EmailParams
                    {
                        HostSmtp = ConfigurationManager.AppSettings["HostSmtp"],
                        Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                        EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                        SenderName = ConfigurationManager.AppSettings["SenderName"],
                        SenderEmail = ConfigurationManager.AppSettings["SenderEmail"],
                        SenderEmailPassword = DecryptSenderEmailPassword()
                    });
                }
                else
                {
                    _email = new Email(new EmailParams
                    {
                        HostSmtp = ConfigurationManager.AppSettings["HostSmtp"],
                        Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                        EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                        SenderName = senderName,
                        SenderEmail = ConfigurationManager.AppSettings["SenderEmail"],
                        SenderEmailPassword = DecryptSenderEmailPassword()
                    });
                }
                Logger.Info("Email params działa");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                Logger.Error("Email params nie działa");
                throw new Exception(ex.Message);
            }

            try
            {
               await _email.Send(message.Subject, message.Body, message.Destination);
            }
            catch (Exception ex)
            {

                Logger.Error(ex, ex.Message);
                Logger.Error("Send email  nie działa");
                throw new Exception(ex.Message);
            }
            
                Logger.Info("email send działa");
                //return Task.FromResult(0);
            
            
            
        }
        private string DecryptSenderEmailPassword()
        {
            var encryptedPassword = ConfigurationManager.AppSettings["SenderEmailPassword"];
            if (encryptedPassword.StartsWith(NotEncryptedPasswordPrefix))
            {
                encryptedPassword = _stringCipher.
                    Encrypt(encryptedPassword.Replace(NotEncryptedPasswordPrefix, ""));

                Configuration webConfig = WebConfigurationManager.OpenWebConfiguration("/");
                webConfig.AppSettings.Settings["SenderEmailPassword"].Value = encryptedPassword;
                webConfig.Save();

                //var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //configFile.AppSettings.Settings["SenderEmailPassword"].Value = encryptedPassword;
                //configFile.Save(); 
            }
            Logger.Info("szyfrowanie działa");
            return _stringCipher.Decrypt(encryptedPassword);
        }

        
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
