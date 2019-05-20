using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    [Table("blog_Comments")]
    public class Commentaire
    {
        public Commentaire()
        {
            Publication = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Column("Pseudo")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(128)]
        [DataType(DataType.Text)]
        public string Pseudo { get; set; }

        [Column("Content")]
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.MultilineText)]
        public string Contenu { get; set; }
        
        [Column("Date_publication",TypeName = "datetime2")]
        public DateTime Publication { get; set; }

        public Article Parent { get; set; }
    }
}