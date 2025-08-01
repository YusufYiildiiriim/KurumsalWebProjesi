﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Windows.Markup;
using KurumsalWebProjesi.Models.DataContext;
using KurumsalWebProjesi.Models.Model;

namespace KurumsalWebProjesi.Controllers
{
    public class SlidersController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();

        // GET: Sliders
        public ActionResult Index()
        {
            var languages = db.Languages.ToList();
            var Slider = db.Slider.ToList();

            return View(Slider);
        }

        // GET: Sliders/Details/5
        public ActionResult Details(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            ViewBag.LanguagesId = new SelectList(db.Languages, "Id", "Language",slider.LanguagesId);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: Sliders/Create
        public ActionResult Create()
        {
            ViewBag.LanguagesId = new SelectList(db.Languages, "Id", "Language");
            return View();
        }

        // POST: Sliders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SliderId,Baslik,Aciklama,ResimURL,LanguagesId")] Slider slider,HttpPostedFileBase ResimURL)
        {

            ViewBag.LanguagesId = new SelectList(db.Languages, "Id", "Language", slider.LanguagesId);
            if (ModelState.IsValid)
            {
                if (ResimURL != null)
                {

                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string Sliderimgname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(1024, 360);
                    img.Save("~/Uploads/Slider/" + Sliderimgname);

                    slider.ResimURL = "Uploads/Slider/" + Sliderimgname;
                }
 
                db.Slider.Add(slider);
                db.SaveChanges();
       
                return RedirectToAction("Index");
            }
          
            return View(slider);
        }

        // GET: Sliders/Edit/5
        public ActionResult Edit(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            ViewBag.LanguagesId = new SelectList(db.Languages, "Id", "Language", slider.LanguagesId);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Sliders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SliderId,Baslik,Aciklama,ResimURL,LanguagesId")] Slider slider ,HttpPostedFileBase ResimUrl,int id)
        {
           
            if (ModelState.IsValid)
            {
                var s = db.Slider.Where(x => x.SliderId == id).SingleOrDefault();

                if (ResimUrl != null)
                {

                    if (System.IO.File.Exists(Server.MapPath(s.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(s.ResimURL));
                    }
                    WebImage img = new WebImage(ResimUrl.InputStream);
                    FileInfo imginfo = new FileInfo(ResimUrl.FileName);

                    string Blogimgname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(600, 400);
                    img.Save("~/Uploads/Blog/" + Blogimgname);

                    s.ResimURL = "Uploads/Blog/" + Blogimgname;

                }

                s.Baslik = slider.Baslik;
                s.Aciklama = slider.Aciklama;
                s.LanguagesId = slider.LanguagesId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        // GET: Sliders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = db.Slider.Find(id);
            if (System.IO.File.Exists(Server.MapPath(slider.ResimURL)))
            {
                System.IO.File.Delete(Server.MapPath(slider.ResimURL));
            }
            db.Slider.Remove(slider);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
