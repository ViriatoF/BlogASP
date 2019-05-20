using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            bool afficheBandeau = true;
            if (Request.Cookies["autoriser_analyse"] != null)
            {
                afficheBandeau = Request.Cookies.Get("autoriser_analyse").Value == "vrai" ? false : true;
            }
            ViewBag.DisplayCookie = afficheBandeau;

            return View();
        }

        ApplicationDbContext Bdd = new ApplicationDbContext();

        

        [AllowAnonymous]
        public ActionResult Cookies(int accept)
        {
            string autorizeCookies = "faux";

            //Accepte le cookie
            if (accept == 1)
            {
                autorizeCookies = "vrai";
            }

            Response.Cookies.Set(new HttpCookie("autoriser_analyse", autorizeCookies) { Expires = DateTime.MaxValue });

            return View("Index");
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