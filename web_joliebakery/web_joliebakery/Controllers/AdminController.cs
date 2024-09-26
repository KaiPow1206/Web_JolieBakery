using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using web_joliebakery.Models;

namespace web_joliebakery.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Login()
        {
            TAIKHOAN tk = new TAIKHOAN();
            return View(tk);
        }
        [HttpPost]
        public ActionResult Login(TAIKHOAN tk)
        {
            if (ModelState.IsValid)
            {
                if (IsValid(tk.EMAIL, tk.PASSWORD))
                {
                    Session["EMAIL"]=tk.EMAIL;
                    FormsAuthentication.SetAuthCookie(tk.EMAIL, false);
                    return RedirectToAction("Admin","Manage");
                }
                else
                {
                    ModelState.AddModelError("", "password or account incorrect");
                }
            }
            return View(tk);
        }

        private bool IsValid(string email, string password)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            bool isValid =false;
            using(var db = new Jolie_bakeryEntities1())
            {
                var user = db.TAIKHOANs.FirstOrDefault(u =>u.EMAIL == email); 
                if (user != null)
                {
                    if (user.PASSWORD == crypto.Compute(password, user.PASSWORDSALT))
                    {
                        isValid = true;
                    }
                }
            }
            return isValid;
        }
    }
}