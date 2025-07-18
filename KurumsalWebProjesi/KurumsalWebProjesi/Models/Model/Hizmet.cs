﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWebProjesi.Models.Model
{
	[Table("Hizmet")]
	public class Hizmet
	{
        [Key]
        public int HizmetId { get; set; }
        [Required,StringLength(150,ErrorMessage ="150 Karekter Olmalı")]
        [DisplayName("Hizmet Başlık")]
        public string Baslik { get; set; }
        [DisplayName("Hizmet Açıklama")]
        public string Aciklama  { get; set; }
        [DisplayName("hizmet Resim")]
        public string ResimURL{ get; set; }
        public int? LanguagesId { get; set; }

        public Languages Languages { get; set; }
    }
}