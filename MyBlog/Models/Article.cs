﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    [Table("blog_Article")]
    public class Article
    {
        public Article()
        {
            bool EstPublie = true;
            Publication = DateTime.Now; // On donne une valeur automatique à la publicaton qui sera à la date du jour
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(128)]
        [RegularExpression(@"^[^,\.\^]+$")]
        [DataType(DataType.Text)]
        public string Pseudo { get; set; }

        [Column("Title")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(128)]
        [DataType(DataType.Text)]
        public string Titre { get; set; }


        [Column("Content")]
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.MultilineText)]
        public string Contenu { get; set; }

        [DisplayName("Illustration")]
        public string ImageName { get; set; }

        [Column("Date_publication",TypeName ="datetime2")] //On force le type de façon à ne pas ressortir d'erreur de type
        public DateTime Publication { get; set; }

        public bool EstPublie { get; set; }

        public virtual List<Commentaire> Commentaires { get; set; }


    }
}