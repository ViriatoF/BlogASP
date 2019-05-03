using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;

namespace MyBlog.Controllers
{
    public class CommentaireController : Controller
    {
        private ApplicationDbContext Bdd = ApplicationDbContext.Create();

        // GET: Commentaire
        public ActionResult Index()
        {
            return View();
        }

        public readonly static int COMPERPAGE = 5;

        public ActionResult _List(int PageCom = 0)
        {
            try
            {
                List<Commentaire> Liste = Bdd.Commentaires
                                                             .OrderByDescending(com => com.Publication)
                                                             .Skip(PageCom * COMPERPAGE)
                                                             .Take(COMPERPAGE).ToList();
                ViewBag.Page = PageCom;
                return View(Liste);
            }
            catch
            {
                return View(new List<Commentaire>());
            }
        }

        //GET: Commentaire/Create
        public ActionResult _Create()
        {
            return View();
        }

        //POST: Commentaire/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Commentaire com)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return View(com);
        //    }

        //    Commentaire com = new Commentaire();
        //}
    }
}