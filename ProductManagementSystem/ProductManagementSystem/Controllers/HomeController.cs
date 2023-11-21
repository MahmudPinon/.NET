using ProductManagementSystem.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ProductManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private const string UserAuthenticationCookieName = "UserAuthentication";

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Customer login)
        {
            var db = new ProductManagementEntities();
            var res = db.Customers.FirstOrDefault(c => c.Username == login.Username && c.Password == login.Password);
            if (res != null)
            {
                var cookie = new HttpCookie("CustomerInfo");
                cookie["Username"] = res.Username;
                cookie["Password"] = res.Password;
                cookie["CustomerId"] = res.Id.ToString();
                cookie.Expires = DateTime.Now.AddMinutes(5);
                Response.Cookies.Add(cookie);

                //TempData["Message"] = "Valid";
                //return View();
                 if (login.Username == "Admin")
                 {
                     return RedirectToAction("Index", "Admin");
                 }
                 else
                 {
                    var customer = db.Customers.FirstOrDefault(c => c.Username == login.Username);
                    ViewBag.CustomerId = customer.Id;
                    return RedirectToAction("Index", "Customer", new { customerId = customer.Id });
                 }
            }
            else
            {
                TempData["Message"] = "Invalid username or password.";
                return View();
            }
        }

        public ActionResult Logout()
        {
            // Clear the "CustomerInfo" cookie
            if (Request.Cookies["CustomerInfo"] != null)
            {
                var customerCookie = new HttpCookie("CustomerInfo");
                customerCookie.Expires = DateTime.Now.AddYears(-1); 
                Response.Cookies.Add(customerCookie);
            }

            return RedirectToAction("Login"); 
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}