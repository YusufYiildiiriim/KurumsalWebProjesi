using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWebProjesi.Models.Model
{
	[Table("Kategori")]
	public class Kategori
	{
        [Key]
        public int KategoriId{ get; set; }
        [Required,StringLength(50,ErrorMessage ="50 Karekter Olmalıdır")]
        public string KategoriAd { get; set; }
        public string Aciklama{ get; set; }
        public string txtsinav { get; set; }
        public int? LanguagesId { get; set; }

        public Languages Languages { get; set; }
        public ICollection<Blog>  Blogs { get; set; }

    }
}