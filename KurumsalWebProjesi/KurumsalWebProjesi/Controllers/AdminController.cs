﻿
using KurumsalWebProjesi.Models;
using KurumsalWebProjesi.Models.DataContext;
using KurumsalWebProjesi.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace KurumsalWebProjesi.Controllers
{
    public class AdminController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        // GET: Admin
        [Route("yonetimpaneli")]
        public ActionResult Index()
        {
            ViewBag.BlogSayisi = db.Blog.Count();
            ViewBag.KategoriSayisi = db.Kategori.Count();
            ViewBag.HizmetSayisi = db.Hizmet.Count();
            ViewBag.YorumSayisi = db.Yorum.Count();





            ViewBag.YorumSayisi = db.Yorum.Where(x => x.Onay != true).Count();
            var sorgu = db.Kategori.ToList();
            return View(sorgu);
        }
        [Route("yonetimpaneli/giris")]
        public ActionResult Login()
        {
            return View();
        }
        [Route("yonetimpaneli/giris")]
        [HttpPost]
        public ActionResult Login(Admin admin, string sifre)
        {
            var md5pass = Crypto.Hash(sifre, "MD5");
            var login = db.Admin.Where(x => x.Eposta == admin.Eposta).SingleOrDefault();
            if(login == null)
            {
                ViewBag.Uyari = "Kullanıcı adı ya da şifre Boş Bırakılamaz";
                return View(admin);
            }
            if(login.Eposta==admin.Eposta && login.Sifre == Crypto.Hash(admin.Sifre,"MD5"))
            {
                Session["adminid"] = login.AdminId;
                Session["eposta"] = login.Eposta;
                Session["yetki"] = login.Yetki;
                return RedirectToAction("Index", "Admin");
            }
            ViewBag.Uyari = "Kullanıcı adı ya da şifre yanlış"; 
            return View(admin);

        }
        public ActionResult LogOut()
        {
            Session["adminid"] = null;
            Session["eposta"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Admin");
        }
        public ActionResult SifremiUnuttum()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SifremiUnuttum(string eposta)
        {
            var mail = db.Admin.Where(x => x.Eposta == eposta).SingleOrDefault();
            if (mail!=null)
            {
                Random rnd = new Random();
                int yeniSifre = rnd.Next();

                Admin Admin = new Admin();
                mail.Sifre= Crypto.Hash(yeniSifre.ToString(), "MD5");
                db.SaveChanges();

                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "cdogrulama@gmail.com";
                WebMail.Password = "hdupiwpgvputxmvm";
                WebMail.SmtpPort = 587;
                WebMail.Send(eposta, "Admin Panel Giriş Şifreniz","Şifreniz :"+yeniSifre);
                ViewBag.Uyari = "Mesajınız basaşrıyla gönderilmiştir";


            }
            else
            {
                ViewBag.Uyari = "Hata oluştu tekrar deneyiniz";

            }

            return View();
        }
        public ActionResult Adminler()
        {
            var adminler = db.Admin.ToList();
            return View(adminler);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Admin Admin,string sifre,string eposta)
        {
            if (ModelState.IsValid)
            {
                Admin.Sifre = Crypto.Hash(sifre,"MD5");
                db.Admin.Add(Admin);
                db.SaveChanges();
                return RedirectToAction("Adminler");

            }
            return View();
        }
        public ActionResult Edit(int id)
        {
            var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();
            return View(a);
        }
        [HttpPost]
        public ActionResult Edit(int id,Admin admin,string sifre ,string eposta)
        {

            if (ModelState.IsValid)
            {
                var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();
                a.Eposta = admin.Eposta;
                a.Yetki = admin.Yetki;
                a.Sifre = Crypto.Hash(sifre, "MD5");
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View(admin);



        }
        public ActionResult Delete(int id)
        {
            var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();
            if (a != null)
            {
                db.Admin.Remove(a);
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View(a);
        }
    }


}