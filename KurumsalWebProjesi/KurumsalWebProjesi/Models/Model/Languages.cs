using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWebProjesi.Models.Model
{
    [Table("Languages")]
    public class Languages
    {
        [Key]
        public int Id { get; set; }
        public string Language { get; set; }
  
        public string LanguageCode { get; set; }
    }
}