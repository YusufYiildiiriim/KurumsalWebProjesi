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
        public ActionResult ChangeLanguage(string lang)
        {
            HttpCookie cookie = new HttpCookie("lang", lang);
            cookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(cookie);
            return Redirect(Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index", "Home"));
        }



        private KurumsalDBContext db = new KurumsalDBContext();
        // GET: Home
        [Route("")]
        [Route("AnaSayfa")]
        public ActionResult Index()
        {
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();
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
        [Route("Hakkimizda")]
        public ActionResult Hakkimizda()
        {
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hakkimizda.SingleOrDefault());
        }
        [Route("Hizmet")]
        public ActionResult Hizmetlerimiz()
        {
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hizmet.ToList().OrderByDescending(x => x.HizmetId));
        }
        [Route("Iletisim")]
        public ActionResult Iletisim()
        {
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();
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
        [Route("BlogPost")]
        public ActionResult Blog()
        {
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Blog.ToList().OrderByDescending(x => x.BlogId));
            //Include("Kategori").
        }
        [Route("BlogPost/{Kategoriad}/{id:int}")]
        public ActionResult KategoriBlog(int id)
        {
            var b = db.Blog.Include("Kategori").Where(x=>x.kategori.KategoriId == id).ToList();
            return View(b);
        }
        [Route("BlogPost/{baslik}-{id:int}")]
        public ActionResult BlogDetay(int id)
        {
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();
            var blog = db.Blog.Include("Kategori").Include("Yorums").Where(x => x.BlogId == id).SingleOrDefault();
            
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



        public ActionResult FooterPartial()
        {
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.Iletisim = db.Iletisim.SingleOrDefault();
            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);
            ViewBag.Blog = db.Blog.ToList().OrderByDescending(x => x.BlogId);
            return PartialView();
        }
        public ActionResult BlogKategoriPartial()
        {
            ViewBag.kimlik = db.Kimlik.SingleOrDefault();

            return PartialView(db.Kategori.Include("Blogs").ToList());
        }

    }
}