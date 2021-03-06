﻿using MyBlog.Models;
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
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;

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

        #region Vue partielle des derniers articles parus
        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult _LastArticles()
        {
            List<Article> liste = Bdd.Articles.Where(a => a.EstPublie).OrderByDescending(a => a.Publication).Take(6).ToList();

            return PartialView("_LastArticles",liste);
        }
        #endregion

        #region Vue partielle de mes articles dans mon espace
        [ChildActionOnly]
        public ActionResult _MyArticles()
        {
            string user = User.Identity.GetUserName();
            var liste = from c in Bdd.Articles
                        where c.Pseudo == user
                        orderby c.Publication descending
                        select c;

            var countArt = liste.Count();
            ViewBag.ContArt = countArt;

            return PartialView("_MyArticles", liste);
        }
        #endregion

        #region ListValidation for admin
        [Authorize(Roles ="Admin")]
        public ActionResult ListValidation()
        {
            List<Article> liste = Bdd.Articles.Where(a => !a.EstPublie).OrderBy(a => a.Publication).ToList();

            return View(liste);
        }
        #endregion

        #region Details
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
        #endregion

        //public readonly static int ARTICLEPERPAGE = 5;

        #region List articles with gridmvc
        // GET: List
        [AllowAnonymous]
        public ActionResult List(int Page = 0)
        {
            try
            {
                //Si on utilise gridmvc
                List<Article> liste = Bdd.Articles.Where(art => art.EstPublie).ToList();
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
        #endregion

        #region Create Get
        // GET: Article/Create
        public ActionResult Create()
        {
            var user = User.Identity;
            ViewBag.User =  user.GetUserName();
            return View();
        }
        #endregion

        private static string[] AcceptedTypes = new string[] { "image/jpeg", "image/png" };
        private static string[] AcceptedExt = new string[] { "jpeg", "jpg", "png", "gif" };
        private static string UFileName = "";
        private static string imageFileName = "";

        #region Create controller with image
        //POST: Article/Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create( [Bind(Include = "Pseudo, Titre, Contenu, Image")]ArticleViewModels articleVm)
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
                    //Afin d'éviter les doublons de nom de fichier on leur attribut un identifiant unique avec GUID
                    Guid id = Guid.NewGuid();
                    UFileName = Path.GetFileName(articleVm.Image.FileName);
                    // On nettoie le nom du fichier en remplacant les caractères spéciaux par... be rien en fait, on les supprime et on passe le résultat en minuscule
                    string valeurSortie = Regex.Replace(UFileName, "[éèà' !?ê]", "").ToLower();
                    imageFileName = id.ToString() + "-" + valeurSortie;
                    string ImagePath = Path.Combine(Server.MapPath("~/Content/Upload/"), imageFileName);
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
                ImageName = imageFileName
            };
            
            Bdd.Articles.Add(article);
            Bdd.SaveChanges();
            return RedirectToAction("List", "Article");

        }
        #endregion

        #region Edit Get
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
        #endregion

        #region Edit POST
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
        #endregion

        #region Delete
        // POST: Article/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            Article article = Bdd.Articles.Find(id);
            Bdd.Articles.Remove(article);
            Bdd.SaveChanges();

            return Json(new { Suppression = "OK"});
        }
        #endregion


    }
}