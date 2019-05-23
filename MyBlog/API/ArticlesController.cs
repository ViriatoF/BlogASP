using MyBlog.Models;
using MyBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyBlog.API
{
    public class ArticlesController : ApiController
    {
        private ApplicationDbContext bdd = new ApplicationDbContext();

        [HttpGet]
        public ArticlesVM GetArticle(int id)
        {
            Article art = bdd.Articles.Find(id);
            ArticlesVM articleApi = new ArticlesVM();
            articleApi.Id = art.ID;
            articleApi.Pseudo = art.Pseudo;
            articleApi.Titre = art.Titre;
            articleApi.Contenu = art.Contenu;
            articleApi.ImageName = art.ImageName;
            articleApi.Publication = art.Publication.ToString("dd/MM/yyyy");
            articleApi.EstPublie = art.EstPublie;

            articleApi.NbrCom = art.Commentaires.Count();

            return articleApi;
        }

        [HttpGet]
        public List<ArticlesVM> GetAllArticles()
        {
            List<ArticlesVM> ArtList = new List<ArticlesVM>();
            List<Article> art = bdd.Articles.ToList();

            foreach(var item in art)
            {
                ArticlesVM articleApi = new ArticlesVM();

                articleApi.Id = item.ID;
                articleApi.Pseudo = item.Pseudo;
                articleApi.Titre = item.Titre;
                articleApi.Contenu = item.Contenu;
                articleApi.ImageName = item.ImageName;
                articleApi.Publication = item.Publication.ToString("dd/MM/yyyy à HH mm");
                articleApi.EstPublie = item.EstPublie;
                articleApi.NbrCom = item.Commentaires.Count();

                ArtList.Add(articleApi);
            }
            


            return ArtList;
        }
    }
}
