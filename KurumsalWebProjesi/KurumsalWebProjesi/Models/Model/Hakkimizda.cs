﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWebProjesi.Models.Model
{
	[Table("Hakkimizda")]
	public class Hakkimizda
	{
        [Key]
        public int HakiimizdaId { get; set; }
        [Required]
        [DisplayName("Hakkımızda Açıklama")]
        public string Aciklama { get; set; }
        public int ?LanguagesId { get; set; }

        public Languages Languages { get; set; }

    }
}