using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    public class Commentaire
    {
        public int ID { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(75)]
        public string Pseudo { get; set; }

        [DataType(DataType.MultilineText)]
        public string Contenu { get; set; }

        public DateTime Publication { get; set; }

    }
}