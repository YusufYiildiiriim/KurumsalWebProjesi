using KurumsalWebProjesi.Models.DataContext;
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
    public class KimlikController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();

        // GET: Kimlik
        public ActionResult Index()
        {

            var kimlikList = db.Kimlik.ToList();
            var diller = db.Languages.ToList();
            

            return View(kimlikList);
        }





        // GET: Kimlik/Edit/5
        public ActionResult Edit(int id)
        {
           
            var kimlik = db.Kimlik.Where(x => x.KimlikId == id).SingleOrDefault();
            ViewBag.LanguagesId = new SelectList(db.Languages.ToList(), "Id", "Language", kimlik.LanguagesId);
            return View(kimlik);
        }

        // POST: Kimlik/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Kimlik kimlik, HttpPostedFileBase LogoUrl)
        {
            ViewBag.LanguagesId = new SelectList(db.Languages, "Id", "Language", kimlik.LanguagesId);
            if (ModelState.IsValid)
            {
                var k = db.Kimlik.Where(x => x.KimlikId == id).SingleOrDefault();
                if (LogoUrl != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(k.LogoURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(k.LogoURL));
                    }
                    WebImage img = new WebImage(LogoUrl.InputStream);
                    FileInfo imginfo = new FileInfo(LogoUrl.FileName);

                    string Blogimgname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(600, 400);
                    img.Save("~/Uploads/Blog/" + Blogimgname);

                    k.LogoURL= "Uploads/Blog/" + Blogimgname;
                }
                k.Title = kimlik.Title;
                k.Keywords = kimlik.Keywords;
                k.Description = kimlik.Description;
                k.Unvan = kimlik.Unvan;
                k.LanguagesId = kimlik.LanguagesId;

                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(kimlik);
        }
        public ActionResult Create()
        {
            ViewBag.LanguagesId = new SelectList(db.Languages, "Id", "Language");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Kimlik kimlik, HttpPostedFileBase LogoUrl)
        {
            ViewBag.LanguagesId = new SelectList(db.Languages, "Id", "Language", kimlik.LanguagesId);
            if (ModelState.IsValid)
            {
                if (LogoUrl != null)
                {
                    WebImage img = new WebImage(LogoUrl.InputStream);
                    FileInfo imginfo = new FileInfo(LogoUrl.FileName);
                    string logoname = LogoUrl.FileName + imginfo.Extension;
                    img.Resize(300, 200);
                    img.Save("~/Uploads/Kimlik/" + logoname);
                    kimlik.LogoURL = "Uploads/Kimlik/" + logoname;
                }
                db.Kimlik.Add(kimlik);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kimlik);

        }
        // GET: Kimlik/Delete/5
        public ActionResult Delete(int id)
        {
            var Kimlik = db.Kimlik.Find(id);
            if (Kimlik == null)
            {
                return HttpNotFound();
            }
            if (System.IO.File.Exists(Server.MapPath(Kimlik.LogoURL)))
            {
                System.IO.File.Delete(Server.MapPath(Kimlik.LogoURL));
            }
            db.Kimlik.Remove(Kimlik);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


    }

}
