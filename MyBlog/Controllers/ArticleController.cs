using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using Slugify;
using System.Net;
using System.Data.Entity;

namespace MyBlog.Controllers
{
    public class ArticleController : Controller
    {
        private ApplicationDbContext Bdd = ApplicationDbContext.Create();

        // GET: Article
        [AllowAnonymous]
        public ActionResult Index()
        {
            
            return View();
        }

        // GET: Article/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            Article CurrentArticle = Bdd.Articles.Find(id);
            if(CurrentArticle == null)
            {
                return new HttpNotFoundResult();
            }
            return View(CurrentArticle);
        }

        public readonly static int ARTICLEPERPAGE = 5;

        // GET: List
        public ActionResult List(int Page = 0)
        {
            try
            {
                //Si on utilise gridmvc
                List<Article> liste = Bdd.Articles.ToList();
                                                  //Si on n'utilise pas gridmvc, on retire le .tolist() et on ajoute ce qui est dessous

                                                    //.Where(article=>article.EstPublie)
                                                    //.OrderBy(a => a.ID)
                                                    //.Skip(Page * ARTICLEPERPAGE) // Permet de définir le saut à faire
                                                    //.Take(ARTICLEPERPAGE).ToList(); // Permet de définir le nombre d'objets à prendre
                ViewBag.Page = Page;
                return View(liste);
            }
            catch
            {
                return View(new List<Article>());
            }
        }

        // GET: Article/Create
        public ActionResult Create()
        {
            return View();
        }

        private static string[] AcceptedTypes = new string[] { "image/jpeg", "image/png" };
        private static string[] AcceptedExt = new string[] { "jpeg", "jpg", "png", "gif" };
        private static string UFileName = "";


        //POST: Article/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="Pseudo,Titre,Contenu,Image")] ArticleViewModels articleVm)
        {
            if (!ModelState.IsValid)
            {
                return View(articleVm);
                
            }


            if (articleVm.Image != null)
            {
                bool HasError = false;
                if (articleVm.Image.ContentLength > 1024 * 1024)
                {
                    ModelState.AddModelError("Image", "Le fichier téléchargé est trop grand.");
                    HasError = true;
                }
                if (!AcceptedTypes.Contains(articleVm.Image.ContentType) || AcceptedExt.Contains(Path.GetExtension(articleVm.Image.FileName).ToLower()))
                {
                    ModelState.AddModelError("Image", "Le fichier doit etre une image.");
                    HasError = true;
                }
                try
                {
                    UFileName = Path.GetFileName(articleVm.Image.FileName);
                    string ImagePath = Path.Combine(Server.MapPath("~/Content/Upload/"), UFileName);
                    articleVm.Image.SaveAs(ImagePath);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        articleVm.Image.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }
                catch
                {
                    UFileName = "";
                    ModelState.AddModelError("Image", "Erreur à l'enregistrement.");
                    HasError = true;
                }
                if (HasError)
                    return View(articleVm.Image);
            }

            Article article = new Article
            {
                Pseudo = articleVm.Pseudo,
                Titre = articleVm.Titre,
                Contenu = articleVm.Contenu,
                ImageName = UFileName
            };

            Bdd.Articles.Add(article);
            Bdd.SaveChanges();
            return RedirectToAction("List", "Article");

        }

        // GET: Article/Edit/5
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = Bdd.Articles.Find(id);
            ViewBag.UserName = article.Pseudo;

            if(article == null)
            {
                return HttpNotFound();
            }

            return View(article);
        }

        // POST: Article/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include ="ID,Pseudo,titre, Contenu, ImageName, EstPublie ")] Article article)
        {
            if (ModelState.IsValid)
            {
                Bdd.Entry(article).State = EntityState.Modified;
                Bdd.SaveChanges();

                return RedirectToAction("List");
            }
            return View(article);
        }
        

        // POST: Article/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            //Article article = Bdd.Articles.Single(p => p.ID == id);
            Article article = Bdd.Articles.Find(id);
            Bdd.Articles.Remove(article);
            Bdd.SaveChanges();

            return Json(new { Suppression = "OK"});
        }

        

    }
}