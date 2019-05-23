using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.ViewModels
{
    public class ArticlesVM
    {
        public int Id { get; set; }

        public string Pseudo { get; set; }

        public string Titre { get; set; }

        public string Contenu { get; set; }

        public string ImageName { get; set; }

        public string Publication { get; set; }

        public bool EstPublie { get; set; }

        public int NbrCom { get; set; }

    }
}