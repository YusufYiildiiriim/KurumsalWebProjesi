using KurumsalWebProjesi.Models.DataContext;
using KurumsalWebProjesi.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWebProjesi.Controllers
{
    public class HomeController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);


            return View();
        }
        public ActionResult SliderPartial()
        {
            return View(db.Slider.ToList().OrderByDescending(x => x.SliderId));
        }
        public ActionResult HizmetPartial()
        {
            return View(db.Hizmet.ToList());
        }
        public ActionResult Hakkimizda()
        {

            return View(db.Hakkimizda.SingleOrDefault());
        }
        public ActionResult Hizmetlerimiz()
        {
            return View(db.Hizmet.ToList().OrderByDescending(x => x.HizmetId));
        }
        public ActionResult Iletisim()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Iletisim(string adsoyad = null, string email = null, string konu = null, string mesaj = null)
        {
            if (adsoyad != null && email != null)
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "cdogrulama@gmail.com";
                WebMail.Password = "yusuf719";
                WebMail.SmtpPort = 587;
                WebMail.Send("cdogrulama@gmail.com", konu, email+"</br>"+mesaj );
                ViewBag.Uyari = "Mesajınız basaşrıyla gönderilmiştir";


            }
            else
            {
                ViewBag.Uyari = "Hata oluştu tekrar deneyiniz";

            }

            return View();
        }
        public ActionResult Blog()
        {
            return View(db.Blog.Include("Kategori").ToList().OrderByDescending(x=>x.BlogId));
        }

        public ActionResult FooterPartial()
        {
            ViewBag.Iletisim = db.Iletisim.SingleOrDefault();
            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);
            ViewBag.Blog = db.Blog.ToList().OrderByDescending(x => x.BlogId);
            return PartialView();
        }


    }
}