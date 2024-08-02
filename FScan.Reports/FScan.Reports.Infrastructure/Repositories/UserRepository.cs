using Azure;
using FScan.Reports.Application.Contracts;
using FScan.Reports.Application.Models.DTOs;
using FScan.Reports.Application.Models.Requests;
using FScan.Reports.Application.Models.Responses;
using FScan.Reports.Application.Models.ViewModels;
using FScan.Reports.Domain.Entities;
using FScan.Reports.Infrastructure.Data;
using FScan.Reports.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FScan.Reports.Infrastructure.Helpers.Settings;
using Response = FScan.Reports.Application.Models.Responses.Response;

namespace FScan.Reports.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<NGAC_USERINFO>, IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly UserClaimsGetter _userClaims;
        private readonly string? _ID;

        public UserRepository(FScanContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _configuration = configuration;
            _userClaims = new UserClaimsGetter(httpContextAccessor);
            _ID = _userClaims.GetClaims().ID;
        }

        public async Task<Response> ChangePasswordAsync(ChangePasswordVM vm)
        {
            Response response = new();

            try
            {

                var user = await _context.NGAC_USERINFO
                                            .Where(s => s.ID.Trim() == _ID)
                                            .FirstOrDefaultAsync();

                string decryptedPassword = Encryptor.Decrypt(user.FSPassword);

                if (!decryptedPassword.Equals(vm.OldPassword))
                {
                    response.Flag = false;
                    response.Message = "Invalid old password";
                }
                else
                {
                    user.FSPassword = Encryptor.Encrypt(vm.NewPassword);
                    await _context.SaveChangesAsync();

                    response.Flag = true;
                    response.Message = "Password changed successfully.";
                }


            }
            catch (Exception e)
            {
                response.Flag = false;
                response.Message = "An error occurred while changing the password.";
            }
            return response;
        }

        public async Task<Response> FChangePasswordAsync(ChangePasswordVM vm)
        {
            Response response = new();

            try
            {
                var user = await _context.NGAC_USERINFO
                                            .Where(s => s.ID.Trim() == vm.Usercode)
                                            .FirstOrDefaultAsync();

                string decryptedPassword = Encryptor.Decrypt(user.FSPassword);

                if (!decryptedPassword.Equals(vm.OldPassword))
                {
                    response.Flag = false;
                    response.Message = "Invalid old password";
                }
                else
                {
                    user.FSPassword = Encryptor.Encrypt(vm.NewPassword);
                    await _context.SaveChangesAsync();

                    response.Flag = true;
                    response.Message = "Password changed successfully.";
                }


            }
            catch (Exception e)
            {
                response.Flag = false;
                response.Message = "An error occurred while changing the password.";
            }
            return response;
        }

        public async Task ProcessPasswordReset(string usercode, string email)
        {
            string defaultPass = GenerateRandomPassword();

            //send email

            var user = await _context.NGAC_USERINFO.Where(s => s.ID == usercode).FirstOrDefaultAsync();

            if (user != null)
            {
                user.FSPassword = Encryptor.Encrypt(defaultPass);
                user.MustChangePW = true;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            MailDTO mail = new();

            //mail.Body = string.Format("Your new password is: {0}, you will change your password upon login.", defaultPass);
            mail.Body = string.Format("<div style='font-family: Default Sans Serif;'>Hi <b>" + user.Name + "</b>"
                           + "<br><br>" +
                           "Your new Password is: <b style='color: green;'>{0}</b>, you will change your password upon login." +
                           "<br><br>" +
                           "Thank you,</div>", defaultPass);
            mail.MailFrom = Settings.Config.MAIL_FROM;
            mail.MailTo = email;
            mail.Subject = "FScan Password Reset";
            EmailUtil util = new EmailUtil();
            util.SendMail(mail);
            //email
        }

        public static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
                        "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                        "abcdefghijkmnopqrstuvwxyz",    // lowercase
                        "0123456789",                   // digits
                        "!@#$%^&*"                        // non-alphanumeric
                    };
            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();
            string pText = "";

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            pText = string.Join("", chars);
            return (pText);
        }

        private string CheckForRegisteredEmail(string usercode)
        {
            string regEmail = string.Empty;

            var email = _context.NGAC_USERINFO.Where(s => s.ID == usercode).First();

            if (email != null)
            {
                if (!string.IsNullOrWhiteSpace(email.BankcomEmail) && email.BankcomEmail != "DECLINED")
                {
                    regEmail = email.BankcomEmail;

                    //TempData["SuccessChangePassword"] = "Please check your email to get New Password.";
                }
                else if (string.IsNullOrWhiteSpace(email.BankcomEmail))
                {
                    //ViewData["Error"] = string.Format("Error encountered:" +
                    //    "Please contact your System Administrator to Reset Password.", Environment.NewLine);
                }
            }

            return regEmail;
        }

        public async Task<ResetPasswordDetailsDTO> ResetPasswordDetailsAsync(string ID)
        {
            var data = await (from A in _context.NGAC_USERINFO
                              join B in _context.NGAC_GROUP on A.GroupID equals B.ID into grouptb
                              from grp in grouptb.DefaultIfEmpty()
                              where A.Name != "admin" && A.ID == ID
                              select new ResetPasswordDetailsDTO
                              {
                                  IndexKey = A.IndexKey,
                                  ID = A.ID,
                                  Name = A.Name,
                                  GroupName = grp.Name,
                                  AccessType = A.AccessType,
                                  BankcomEmail = A.BankcomEmail
                              }).FirstOrDefaultAsync();
            return data;
        }

        //        public async Task<Response> ResetPasswordAsync(PasswordResetRequest request)
        //        {
        //            Response response = new();
        //            string defaultPassword = "kY5rpCilcn1p/pIHxKBPIO1pQdJciMKu+6KwIFQcznM=";
        //            var userToUpdate = await _context.NGAC_USERINFO.FirstOrDefaultAsync(s => s.ID == request.ID);

        //            try
        //            {
        //                var app = new NGAC_USERINFO();
        //                userToUpdate.FSPassword = defaultPassword;
        //                userToUpdate.BankcomEmail = request.BankcomEmail;
        //                userToUpdate.MustChangePW = true;

        //                await _context.SaveChangesAsync();

        //                //SEND NEW CREATED LOGS APPLICATION
        //                EmailUtil throwemail = new EmailUtil();

        //                var set = new MailDTO();
        //                set.MailFrom = Config.MAIL_FROM;
        //                set.MailTo = userToUpdate.BankcomEmail;

        //                set.Subject = "Finger Scan Reset Password";
        //                string? name = userToUpdate.Name;
        //                string emailBody = $@"<!DOCTYPE html>
        //<html>
        //<head>
        //    <style>
        //        body {{
        //            font-family: Arial, sans-serif;
        //        }}
        //        .content {{
        //            padding: 20px;
        //        }}
        //        .password {{
        //            color: green;
        //            font-weight: bold;
        //        }}
        //    </style>
        //</head>
        //<body>
        //    <div class='content'>
        //        Hi <b>{name}</b>,
        //        <br><br> 
        //        Please be informed that your request for Reset Password has been successfully done.
        //        <br><br>
        //        Kindly use this default password: <span class='password'>Defaul32#$%!</span>
        //        <br><br>
        //        Thank you,
        //    </div>
        //</body>
        //</html>";

        //                set.Body = emailBody;

        //                //set.Body = string.Format("<div style='font-family: Default Sans Serif;'>Hi <b>" + userToUpdate.Name + "</b>"
        //                //        + "<br><br> Please be inform that your request for Reset Password has been successfully done.<br><br>" +
        //                //        "Kindly use this default password: <b style='color: green;'>Defaul32#$%!</b>" +
        //                //        "<br><br>" +
        //                //        "Thank you,</div>");

        //                throwemail.SendMail(set);

        //                response.Flag = true;
        //                response.Message = "Finger scan reset password successfully sent.";
        //                return response;
        //            }
        //            catch (Exception ex)
        //            {
        //                response.Flag = false; 
        //                response.Message = ex.Message;
        //                return response;
        //            }
        //        }

        public async Task<Response> ResetPasswordAsync(PasswordResetRequest request)
        {
            Response response = new();
            string defaultPassword = "kY5rpCilcn1p/pIHxKBPIO1pQdJciMKu+6KwIFQcznM=";
            var userToUpdate = await _context.NGAC_USERINFO.FirstOrDefaultAsync(s => s.ID == request.ID);

            if (userToUpdate == null)
            {
                response.Flag = false;
                response.Message = "User not found.";
                return response;
            }

            try
            {
                userToUpdate.FSPassword = defaultPassword;
                userToUpdate.BankcomEmail = request.BankcomEmail;
                userToUpdate.MustChangePW = true;

                await _context.SaveChangesAsync();

                // Send email notification
                EmailUtil emailUtil = new EmailUtil();
                var mail = new MailDTO
                {
                    MailFrom = Config.MAIL_FROM,
                    MailTo = userToUpdate.BankcomEmail,
                    Subject = "Finger Scan Reset Password",
                    Body = $@"<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
        }}
        .content {{
            padding: 20px;
        }}
        .password {{
            color: green;
            font-weight: bold;
        }}
    </style>
</head>
<body>
    <div class='content'>
        Hi <b>{userToUpdate.Name}</b>,
        <br><br> 
        Please be informed that your request for Reset Password has been successfully done.
        <br><br>
        Kindly use this default password: <span class='password'>Defaul32#$%!</span>
        <br><br>
        Thank you,
    </div>
</body>
</html>"
                };

                emailUtil.SendMail(mail);

                response.Flag = true;
                response.Message = "Finger scan reset password successfully sent.";
                return response;
            }
            catch (Exception ex)
            {
                response.Flag = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<Response> EmailRegistrationAsync(EmailRegistrationVM vm)
        {
           

            try
            {
                var user = await _context.NGAC_USERINFO
                                            .Where(s => s.ID.Trim() == vm.Usercode)
                                            .FirstOrDefaultAsync();

                if (user == null)
                {
                    return new Response
                    {
                        Flag = false,
                        Message = "User not found"
                    };
                }
                else
                {
                    user.BankcomEmail = vm.Email;
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    return new Response
                    {
                        Flag = true,
                        Message = "Succesfully Registered."
                    };
                }


            }
            catch (Exception e)
            {
                return new Response
                {
                    Flag = false,
                    Message = e.Message,
                };
               
            }
           
        }


        public async Task<Response> ForgotPasswordRequestAsync(ForgotPasswordVM vm)
        {
            string regEmail = CheckForRegisteredEmail(vm.Usercode);

            if (!string.IsNullOrWhiteSpace(regEmail))
            {
                await ProcessPasswordReset(vm.Usercode, regEmail);
                return new Response
                {
                    Flag = true,
                    Message = "Check your email for your password reset details."
                };
            }
            else {
                return new Response
                {
                    Flag = false,
                    Message = "Failed to reset your password please contact the administrator."
                };
            }
           

            //return View("Login");
        }
    }
}
