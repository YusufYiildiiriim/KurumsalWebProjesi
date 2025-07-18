﻿using KurumsalWebProjesi.Models.DataContext;
using KurumsalWebProjesi.Models.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWebProjesi.Controllers
{
    public class BlogController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();
        //GET: Blog

        //public ActionResult Index()
        //{
        //    var a = db.Blog.Include("Kategori").Include("Languages").ToList();
        //    var b = db.Blog.Include("Kategori").ToList();

        //    db.Configuration.LazyLoadingEnabled = false;
        //    return View(a);
        //}
        public ActionResult Index(string dil, int? kategoriId, string baslik, string icerik)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var bloglar = db.Blog
                .Include("Kategori")
                .Include("Languages")
                .AsQueryable();

            // Dil filtresi
            if (!string.IsNullOrEmpty(dil))
            {
                bloglar = bloglar.Where(b => b.Languages.LanguageCode == dil);
            }

            // Kategori filtresi
            if (kategoriId.HasValue)
            {
                bloglar = bloglar.Where(b => b.kategori.KategoriId == kategoriId.Value);
            }

            // Başlık araması
            if (!string.IsNullOrEmpty(baslik))
            {
                bloglar = bloglar.Where(b => b.Baslik.Contains(baslik));
            }

            // İçerik araması
            if (!string.IsNullOrEmpty(icerik))
            {
                bloglar = bloglar.Where(b => b.Icerik.Contains(icerik));
            }

            ViewBag.Diller = new SelectList(db.Languages.ToList(), "LanguageCode", "Language");
            ViewBag.Kategoriler = new SelectList(db.Kategori.ToList(), "KategoriId", "KategoriAd");

            return View(bloglar.ToList());
        }





        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId","KategoriAd");
            ViewBag.LanguagesId = new SelectList(db.Languages, "Id","Language");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog blog,HttpPostedFileBase ResimURL)
        {

            if (ResimURL !=null)
            {
               
                WebImage img = new WebImage(ResimURL.InputStream);
                FileInfo imginfo = new FileInfo(ResimURL.FileName);

                string blogimgname = Guid.NewGuid().ToString()+ imginfo.Extension;
                img.Resize(600, 400);
                img.Save("~/Uploads/Blog/" + blogimgname);

                blog.ResimURL= "Uploads/Blog/" + blogimgname;

                db.Blog.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            else
            {
             
                
            }
            return RedirectToAction("Index");

        }
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var b = db.Blog.Where(x => x.BlogId == id).SingleOrDefault();
            if (b == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAd", b.KategoriId);
            ViewBag.LanguagesId = new SelectList(db.Languages, "Id", "Language",b.LanguagesId);
            return View(b);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id,Blog blog,HttpPostedFileBase ResimUrl)
        {
            if (ModelState.IsValid)
            {
                var b = db.Blog.Where(x => x.BlogId == id).SingleOrDefault();

                if (ResimUrl != null)
                {

                    if (System.IO.File.Exists(Server.MapPath(b.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(b.ResimURL));
                    }
                    WebImage img = new WebImage(ResimUrl.InputStream);
                    FileInfo imginfo = new FileInfo(ResimUrl.FileName);

                    string Blogimgname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(600, 400);
                    img.Save("~/Uploads/Blog/" + Blogimgname);

                    b.ResimURL = "Uploads/Blog/" + Blogimgname;

                }
                b.Baslik = blog.Baslik;
                b.Icerik = blog.Icerik;
                b.kategori = blog.kategori;
                b.LanguagesId = blog.LanguagesId;
                db.SaveChanges();
                return RedirectToAction("Index");


            }
            return View(blog);

        }
        public ActionResult Delete(int id)
        {
            var b = db.Blog.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            if (System.IO.File.Exists(Server.MapPath(b.ResimURL)))
            {
                System.IO.File.Delete(Server.MapPath(b.ResimURL));
            }
            db.Blog.Remove(b);
            db.SaveChanges();

            return RedirectToAction("Index");
        }



    }
}