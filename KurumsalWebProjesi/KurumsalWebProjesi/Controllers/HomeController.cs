using KurumsalWebProjesi.Models.DataContext;
using KurumsalWebProjesi.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWebProjesi.Controllers
{
    public class HomeController : Controller
    {
        private string GetLang()
        {
            string lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            return lang;
        }


        [HttpPost]
        public ActionResult DilSec(string dil)
        {
            // Örnek: gelen değer "türkçe"

            // İstersen cookie olarak sakla:
            HttpCookie cookie = new HttpCookie("lang", dil == "türkçe" ? "tr" : (dil == "english" ? "en" : "tr"));
            cookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(cookie);


            // İstersen Session da kullanabilirsin (opsiyonel)
            // Session["Dil"] = dil;

            // Geri yönlendir
            return RedirectToAction("Index", "Home");
        }



        private KurumsalDBContext db = new KurumsalDBContext();
        // GET: Home
        [Route("")]
        [Route("AnaSayfa")]
        public ActionResult Index()
        {
            string lang = GetLang();

            ViewBag.kimlik = db.Kimlik
                .FirstOrDefault(x => x.Languages.LanguageCode == lang)
                ?? db.Kimlik.FirstOrDefault(x => x.Languages.LanguageCode == "tr");

            var hizmetler = db.Hizmet
                .Where(x => x.Languages.LanguageCode == lang)
                .OrderByDescending(x => x.HizmetId)
                .ToList();

            ViewBag.Hizmetler = hizmetler.Any()
                ? hizmetler
                : db.Hizmet
                    .Where(x => x.Languages.LanguageCode == "tr")
                    .OrderByDescending(x => x.HizmetId)
                    .ToList();

            return View();
        }

        //public ActionResult SliderPartial()
        //{
        //    return View(db.Slider.ToList().OrderByDescending(x => x.SliderId));
        //}
        public ActionResult SliderPartial()
        {
            string lang = GetLang();

            var sliders = db.Slider
                .Where(x => x.Languages.LanguageCode == lang)
                .OrderByDescending(x => x.SliderId)
                .ToList();

            if (!sliders.Any())
            {
                sliders = db.Slider
                    .Where(x => x.Languages.LanguageCode == "tr")
                    .OrderByDescending(x => x.SliderId)
                    .ToList();
            }

            return View(sliders);
        }


        public ActionResult HizmetPartial()
        {
            string lang = GetLang();

            var hizmetler = db.Hizmet
                .Where(x => x.Languages.LanguageCode == lang)
                .OrderByDescending(x => x.HizmetId)
                .ToList();

            if (!hizmetler.Any())
            {
                hizmetler = db.Hizmet
                    .Where(x => x.Languages.LanguageCode == "tr")
                    .OrderByDescending(x => x.HizmetId)
                    .ToList();
            }

            return View(hizmetler);
        }

        //[Route("HakkimizdaDetay")]
        //public ActionResult Hakkimizda()
        //{
        //    ViewBag.kimlik = db.Kimlik.SingleOrDefault();
        //    return View(db.Hakkimizda.SingleOrDefault());
        //}
        [Route("HakkimizdaDetay")]
        public ActionResult Hakkimizda()
        {
            string lang = GetLang();

            ViewBag.kimlik = db.Kimlik
                .FirstOrDefault(x => x.Languages.LanguageCode == lang)
                ?? db.Kimlik.FirstOrDefault(x => x.Languages.LanguageCode == "tr");

            var hakkimizda = db.Hakkimizda
                .FirstOrDefault(x => x.Languages.LanguageCode == lang)
                ?? db.Hakkimizda.FirstOrDefault(x => x.Languages.LanguageCode == "tr");

            return View(hakkimizda);
        }

        //[Route("Hizmetlerimiz")]
        //public ActionResult Hizmetlerimiz()
        //{
        //    ViewBag.kimlik = db.Kimlik.SingleOrDefault();
        //    return View(db.Hizmet.ToList().OrderByDescending(x => x.HizmetId));
        //}
        [Route("Hizmetlerimiz")]
        public ActionResult Hizmetlerimiz()
        {
            string lang = GetLang();

            ViewBag.kimlik = db.Kimlik
                .FirstOrDefault(x => x.Languages.LanguageCode == lang)
                ?? db.Kimlik.FirstOrDefault(x => x.Languages.LanguageCode == "tr");

            var hizmetler = db.Hizmet
                .Where(x => x.Languages.LanguageCode == lang)
                .OrderByDescending(x => x.HizmetId)
                .ToList();

            if (!hizmetler.Any())
            {
                hizmetler = db.Hizmet
                    .Where(x => x.Languages.LanguageCode == "tr")
                    .OrderByDescending(x => x.HizmetId)
                    .ToList();
            }

            return View(hizmetler);
        }

        //[Route("Iletisim")]
        //public ActionResult Iletisim()
        //{
        //    ViewBag.kimlik = db.Kimlik.SingleOrDefault();
        //    return View();
        //}
        [Route("Iletisim")]
        public ActionResult Iletisim()
        {
            string lang = GetLang();

            ViewBag.kimlik = db.Kimlik
                .FirstOrDefault(x => x.Languages.LanguageCode == lang)
                ?? db.Kimlik.FirstOrDefault(x => x.Languages.LanguageCode == "tr");

            return View();
        }

        [HttpPost]
        public ActionResult Iletisim(string adsoyad , string email , string konu, string mesaj )
        {
            if (adsoyad != null && email != null)
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "cdogrulama@gmail.com";
                WebMail.Password = "hdupiwpgvputxmvm";
                WebMail.SmtpPort = 587;
                WebMail.Send("cdogrulama@gmail.com", konu, email + "</br>" + mesaj);
                ViewBag.Uyari = "Mesajınız basaşrıyla gönderilmiştir";
                return View();

            }
            else
            {
                ViewBag.Uyari = "Hata oluştu tekrar deneyiniz";

            }

            return View();
        }
        //[Route("BlogPost")]
        //public ActionResult Blog()
        //{
        //    ViewBag.kimlik = db.Kimlik.SingleOrDefault();
        //    return View(db.Blog.ToList().OrderByDescending(x => x.BlogId));
        //    //Include("Kategori").
        //}
        [Route("BlogPost")]
        public ActionResult Blog()
        {
            string lang = GetLang();

            ViewBag.kimlik = db.Kimlik
                .FirstOrDefault(x => x.Languages.LanguageCode == lang)
                ?? db.Kimlik.FirstOrDefault(x => x.Languages.LanguageCode == "tr");

            var bloglar = db.Blog
                .Where(x => x.Languages.LanguageCode == lang)
                .OrderByDescending(x => x.BlogId)
                .ToList();

            if (!bloglar.Any())
            {
                bloglar = db.Blog
                    .Where(x => x.Languages.LanguageCode == "tr")
                    .OrderByDescending(x => x.BlogId)
                    .ToList();
            }

            return View(bloglar);
        }

        //[Route("BlogPost/{Kategoriad}/{id:int}")]
        //public ActionResult KategoriBlog(int id)
        //{
        //    var b = db.Blog.Include("Kategori").Where(x=>x.kategori.KategoriId == id).ToList();
        //    return View(b);
        //}
        [Route("BlogPost/{Kategoriad}/{id:int}")]
        public ActionResult KategoriBlog(int id)
        {
            string lang = GetLang();

            var b = db.Blog
                .Include("Kategori")
                .Where(x => x.kategori.KategoriId == id && x.Languages.LanguageCode == lang)
                .ToList();

            if (!b.Any())
            {
                b = db.Blog
                    .Include("Kategori")
                    .Where(x => x.kategori.KategoriId == id && x.Languages.LanguageCode == "tr")
                    .ToList();
            }

            return View(b);
        }

        //[Route("BlogPost/{baslik}-{id:int}")]
        //public ActionResult BlogDetay(int id)
        //{
        //    ViewBag.kimlik = db.Kimlik.SingleOrDefault();
        //    var blog = db.Blog.Include("Kategori").Include("Yorums").Where(x => x.BlogId == id).SingleOrDefault();

        //    return View(blog);
        //}
        [Route("BlogPost/{baslik}-{id:int}")]
        public ActionResult BlogDetay(int id)
        {
            string lang = GetLang();

            ViewBag.kimlik = db.Kimlik
                .FirstOrDefault(x => x.Languages.LanguageCode == lang)
                ?? db.Kimlik.FirstOrDefault(x => x.Languages.LanguageCode == "tr");

            var blog = db.Blog
                .Include("Kategori")
                .Include("Yorums")
                .Where(x => x.BlogId == id && x.Languages.LanguageCode == lang)
                .SingleOrDefault();

            if (blog == null)
            {
                blog = db.Blog
                    .Include("Kategori")
                    .Include("Yorums")
                    .Where(x => x.BlogId == id && x.Languages.LanguageCode == "tr")
                    .SingleOrDefault();
            }

            return View(blog);
        }

        public JsonResult YorumYap(string adsoyad, string eposta, string icerik, int blogid)
        {
            if (icerik == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            db.Yorum.Add(new Yorum { AdSoyad = adsoyad, Eposta = eposta, Icerik = icerik, BlogId = blogid,Onay=false });
            db.SaveChanges();
            return Json(false, JsonRequestBehavior.AllowGet);
        }



        //public ActionResult FooterPartial()
        //{
        //    ViewBag.kimlik = db.Kimlik.SingleOrDefault();
        //    ViewBag.Iletisim = db.Iletisim.SingleOrDefault();
        //    ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);
        //    ViewBag.Blog = db.Blog.ToList().OrderByDescending(x => x.BlogId);
        //    return PartialView();
        //}
        public ActionResult FooterPartial()
        {
            string lang = GetLang();

            var kimlik = db.Kimlik
                .FirstOrDefault(x => x.Languages.LanguageCode == lang)
                ?? db.Kimlik.FirstOrDefault(x => x.Languages.LanguageCode == "tr");

            var hizmetler = db.Hizmet
                .Where(x => x.Languages.LanguageCode == lang)
                .OrderByDescending(x => x.HizmetId)
                .ToList();

            if (!hizmetler.Any())
            {
                hizmetler = db.Hizmet
                    .Where(x => x.Languages.LanguageCode == "tr")
                    .OrderByDescending(x => x.HizmetId)
                    .ToList();
            }

            var bloglar = db.Blog
                .Where(x => x.Languages.LanguageCode == lang)
                .OrderByDescending(x => x.BlogId)
                .ToList();

            if (!bloglar.Any())
            {
                bloglar = db.Blog
                    .Where(x => x.Languages.LanguageCode == "tr")
                    .OrderByDescending(x => x.BlogId)
                    .ToList();
            }

            var iletisim = db.Iletisim.SingleOrDefault();

            ViewBag.kimlik = kimlik;
            ViewBag.Iletisim = iletisim;
            ViewBag.Hizmetler = hizmetler;
            ViewBag.Blog = bloglar;

            return PartialView();
        }



        //public ActionResult BlogKategoriPartial()
        //{
        //    ViewBag.kimlik = db.Kimlik.SingleOrDefault();

        //    return PartialView(db.Kategori.Include("Blogs").ToList());
        //}
        public ActionResult BlogKategoriPartial()
        {
            string lang = GetLang();

            ViewBag.kimlik = db.Kimlik
                .FirstOrDefault(x => x.Languages.LanguageCode == lang)
                ?? db.Kimlik.FirstOrDefault(x => x.Languages.LanguageCode == "tr");

            var kategoriler = db.Kategori
                .Include("Blogs")
                .Where(x => x.Languages.LanguageCode == lang)
                .ToList();

            if (!kategoriler.Any())
            {
                kategoriler = db.Kategori
                    .Include("Blogs")
                    .Where(x => x.Languages.LanguageCode == "tr")
                    .ToList();
            }

            return PartialView(kategoriler);
        }


    }
}