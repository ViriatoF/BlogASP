using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    public class ArticleViewModels
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(128)]
        [RegularExpression(@"^[^,\.\^+$")]
        [DataType(DataType.Text)]
        public string Pseudo { get; set; }
        /// <summary>
        /// Le titre de l'article
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(128)]
        [DataType(DataType.Text)]
        public string Titre { get; set; }
        /// <summary>
        /// Le contenu de l'article
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Text)]
        public string Contenu { get; set; }

        /// <summary>
        /// Le fichier image
        /// </summary>
        [DisplayName("Illustration")]
        public HttpPostedFileBase Image { get; set; }
    }
}