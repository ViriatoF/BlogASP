using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using Slugify;

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
        public ActionResult Details(int id)
        {
            Article CurrentArticle = Bdd.Articles.Find(id);
            if(CurrentArticle == null)
            {
                return new HttpNotFoundResult();
            }
            return View(CurrentArticle);
        }

        // GET: Article/Create
        public ActionResult Create()
        {
            return View();
        }

        private static string[] AcceptedTypes = new string[] { "image/jpeg", "image/png" };
        private static string[] AcceptedExt = new string[] { "jpeg", "jpg", "png", "gif" };

        //POST: Article/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleViewModels articleVM)
        {
            if (ModelState.IsValid)
            {
                return View(articleVM);
            }
            string FileName = "";
            if (articleVM.Image != null)
            {
                bool HasError = false;
                if (articleVM.Image.ContentLength > 1024 * 1024)
                {
                    ModelState.AddModelError("Image", "Le fichier téléchargé est trop grand.");
                    HasError = true;
                }
                if (!AcceptedTypes.Contains(articleVM.Image.ContentType) || AcceptedExt.Contains(Path.GetExtension(articleVM.Image.FileName).ToLower()))
                {
                    ModelState.AddModelError("Image", "Le fichier doit etre une image.");
                    HasError = true;
                }
                try
                {
                    FileName = Path.GetFileName(articleVM.Image.FileName);
                    string ImagePath = Path.Combine(Server.MapPath("~/Content/Upload"), FileName);
                    articleVM.Image.SaveAs(ImagePath);
                }
                catch
                {
                    FileName = "";
                    ModelState.AddModelError("Image", "Erreur à l'enregistrement.");
                    HasError = true;
                }
                if (HasError)
                    return View(articleVM);
            }

            Article article = new Article
            {
                Contenu = articleVM.Contenu,
                Pseudo = articleVM.Pseudo,
                Titre = articleVM.Titre,
                ImageName = FileName
            };

            Bdd.Articles.Add(article);
            Bdd.SaveChanges();

            return RedirectToAction("List", "Article");
            
        }

        // GET: Article/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Article/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Article/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Article/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private readonly ArticleJSONRepository _repository;

        /// <summary>
        /// Constructeur par défaut, permet d'initialiser le chemin du fichier JSON
        /// </summary>
        public ArticleController()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "liste_article_tuto_full.json");
            _repository = new ArticleJSONRepository(path);
        }

        public readonly static int ARTICLEPERPAGE = 5;
        //delegate bool Where(TEntity entity);

        // GET: List
        public ActionResult List(int Page = 0)
        {
            try
            {
                List<Article> liste = Bdd.Articles
                                                    .Where(article=>article.EstPublie)
                                                    .OrderBy(a => a.ID)
                                                    .Skip(Page * ARTICLEPERPAGE) // Permet de définir le saut à faire
                                                    .Take(ARTICLEPERPAGE).ToList(); // Permet de définir le nombre d'objets à prendre
                ViewBag.Page = Page;
                return View(liste);
            }
            catch
            {
                return View(new List<Article>());
            }
        }
    }
}