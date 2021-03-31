using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class AuthenController : Controller
    {
        // GET: Authen
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Account acc)
        {
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);
            if (acc.Username == null || acc.Password == null)
            {
                //Response.Write("<script>alert('username or password is null or wrong');</script>");
            }
            else
            {
                var user = manager.Find(acc.Username, acc.Password);
                if (user != null)
                {
                    var authenticationManager = HttpContext.GetOwinContext().Authentication;
                    var userIdentity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                    authenticationManager.SignIn(new AuthenticationProperties { }, userIdentity);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Response.Write("<script>alert('wrong password or user dosent exist');</script>");
                }
            }


            return View(acc);
        }
        public ActionResult LogOut()
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Login", "Authen");
        }
        public static void CreateAccount(string Username, string Password, string role)
        {
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            var user = new IdentityUser(Username);
            manager.Create(user, Password);
            manager.AddToRole(user.Id, role);
        }
        public static void DeleteAccount(string Username)
        {
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            var user = manager.FindByName(Username);
            manager.Delete(user);
        }
        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(String current, String newPass, String ConfirmNewPass)
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.FindByName(User.Identity.Name);
            if(newPass == ConfirmNewPass) 
            {
            userManager.ChangePassword(user.Id, current, newPass);
            }else
            {
                TempData["message"] = " Confirm Fail , try again! ";
                return View();
            }    
            

            return RedirectToAction("Index", "Home");

        }
    }
}