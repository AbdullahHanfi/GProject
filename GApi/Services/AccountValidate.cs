using GApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.ModelBinding;
using System.Xml.Linq;

namespace GApi.Services
{

    static public class AccountValidate
    {
        /// <summary>
        /// insert Student into DataBase
        /// </summary>
        /// <returns>return one if Data inserted correctly or zero if Data isn't Valid or -1 if unknow Error</returns>
        static public int IsValidRegistertion(RegisterBinding item, ModelStateDictionary ModelState)
        {
            using (Gdb db = new Gdb())
            {
                bool ValidEmail = AccountValidate.ValidEmail(item.Email);
                bool ValidName = AccountValidate.ValidName(item.Name);

                if (!ValidEmail)
                {
                    ModelState.AddModelError("Email", "Email Not Valid");
                }
                if (!ValidName)
                {
                    ModelState.AddModelError("Name", "Name Not Valid");
                }
                if (!ModelState.IsValid)
                {
                    return 0;
                }
                else
                {
                    db.Users.Add((User)item);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                    return 1;
                }
            }
        }

        /// <summary>
        /// Make Cookie for user 
        /// </summary>
        /// <returns>return one if Account Data is Valid or Zero if Data isn't Valid</returns>
        static public bool IsVaildAccount(string UserName, string password)
        {
            using (Gdb db = new Gdb())
            {
                var User = db.Users
                .FirstOrDefault(M => (M.Email == UserName || M.Name == UserName)
                && M.Password == password);

                if (User != null)
                {
                    var identity = new ClaimsIdentity(
                      new List<Claim>(){
                                new Claim("ID", User.id.ToString()),
                                new Claim(ClaimTypes.Name, User.UserName),
                                new Claim(ClaimTypes.Role, db.Roles.Find(User.R_ID).Name)
                      }, "GProgectCookies");
                    System.Web.HttpContext.Current.Request.GetOwinContext().Authentication.SignIn(identity);
                    return true;
                }
                return false;
            }
        }

        static private bool ValidEmail(string Email)
        {
            using (Gdb db = new Gdb())
            {
                return !db.Users
                    .Where(M => M.Email == Email)
                    .Any();
            }
        }

        static private bool ValidName(string Name)
        {
            using (Gdb db = new Gdb())
            {
                return !db.Users
                    .Where(M => M.Email == Name)
                    .Any();
            }
        }
    }
}