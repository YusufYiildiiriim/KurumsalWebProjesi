﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWebProjesi.Models.Model
{
	[Table("Blog")]
	public class Blog
	{
        public int BlogId { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public string ResimURL { get; set; }
        public int? KategoriId { get; set; }
        public Kategori kategori{ get; set; }

        public ICollection<Yorum> Yorums { get; set; }
        public int? LanguagesId { get; set; }

        public Languages Languages { get; set; }


    }
}