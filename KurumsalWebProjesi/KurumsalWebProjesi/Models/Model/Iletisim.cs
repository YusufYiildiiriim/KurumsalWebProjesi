using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWebProjesi.Models.Model
{
	[Table("Iletisim")]
	public class Iletisim
	{
        public int IletisimId { get; set; }
        [StringLength(250,ErrorMessage ="250 Karakter Olmalıdır")]
        public string Adres { get; set; }
        [StringLength(250, ErrorMessage = "250 Karakter Olmalıdır")]
        public string Telefon { get; set; }
        public string KurumsalTel { get; set; }
        public string Faks { get; set; }
        public string Whatsapp { get; set; }
        public string Facebook{ get; set; }
        public string Twitter{ get; set; }
        public string Instagram{ get; set; }


    }
}