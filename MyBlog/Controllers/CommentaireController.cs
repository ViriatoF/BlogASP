using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using Microsoft.AspNet.Identity;

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

        #region _List comments
        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult _List(int id)
        {
            try
            {
                var AuthorId = Bdd.Articles.Find(id);
                int authorNumb = AuthorId.ID;

                //List<Commentaire> Liste = Bdd.Commentaires.ToList();
                //var ListeTri = Liste.Find(c => c.Parent.ID == authorNumb);

                var liste = from c in Bdd.Commentaires
                            where c.Parent.ID == authorNumb
                            orderby c.Publication descending
                            select c;

                return PartialView("_ListComments", liste);
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Create GET
        //GET: Commentaire/Create
        [ChildActionOnly]
        public ActionResult _Create()
        {
            
            //ViewBag.UserMail = UserMail;
            

            return PartialView();
        }
        #endregion

        #region Create POST
        //POST: Commentaire/Create
        [HttpPost, ActionName("_Create")]
        [ValidateAntiForgeryToken]
        //[ChildActionOnly]
        public ActionResult _Create(int id, [Bind(Include = "Pseudo, Contenu ")] Commentaire com)
        {

            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var UserMail = userManager.GetEmail(user.GetUserId());
            //var userName = User.Identity.GetUserName();
            com.Parent = Bdd.Articles.Find(id);
            //com.Pseudo = userName;
            com.Email = UserMail;
            //com.Parent = Bdd.Articles.Include("Parent").Where(c => c.ID == id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                Bdd.Commentaires.Add(com);
                Bdd.SaveChanges();
                return RedirectToAction("Details", "Article", routeValues: new { id });
            }

            return View();
        }
        #endregion

        #region Delete Commentaire
        [HttpPost]
        public JsonResult Delete(int id)
        {
            Commentaire com = Bdd.Commentaires.Find(id);
            Bdd.Commentaires.Remove(com);
            Bdd.SaveChanges();

            return Json(new { Suppression = "OK" });
        }
        #endregion
    }
}