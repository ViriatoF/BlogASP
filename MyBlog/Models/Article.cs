using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    [Table("blog_Article")]
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("Title")]
        [StringLength(128)]
        public string Titre { get; set; }

        [Column("Content")]
        public string Texte { get; set; }

        public string ThumbnailPath { get; set; }
    }
}