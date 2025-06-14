using KurumsalWebProjesi.Models.DataContext;
using KurumsalWebProjesi.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KurumsalWebProjesi.Controllers
{
    public class HakkimizdaController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        // GET: Hakkimizda
        public ActionResult Index()
        {
            var hakkimizdalist = db.Hakkimizda.ToList();
            var diller = db.Languages.ToList();

           
            return View(hakkimizdalist);
        }

        public ActionResult Edit(int id)
        {
            var hakkimizda = db.Hakkimizda.Find(id);
            ViewBag.LanguagesId = new SelectList(db.Languages, "Id", "Language",hakkimizda.LanguagesId);
            return View(hakkimizda);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Hakkimizda h)
        {
            ViewBag.LanguagesId = new SelectList(db.Languages, "Id", "Language", h.LanguagesId);
            if (ModelState.IsValid)
            {
                var hakkimizda = db.Hakkimizda.Where(x => x.HakiimizdaId == id).SingleOrDefault();
                hakkimizda.Aciklama = h.Aciklama;
                hakkimizda.LanguagesId = h.LanguagesId;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "Hakkimizda");
            }
          

            return View(h);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.LanguagesId = new SelectList(db.Languages, "Id", "Language");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        //public ActionResult Create(string HakkimizdaAciklama,int LanguageId)
        //{
            
        //    if (!string.IsNullOrWhiteSpace(HakkimizdaAciklama))
        //    {
        //        db.Hakkimizda.Add(new Hakkimizda()
        //        {
        //            Aciklama = HakkimizdaAciklama,
        //            LanguagesId = LanguageId

        //        });
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(); 
        //}
        public ActionResult Create(Hakkimizda h)
        {
            ViewBag.LanguagesId = new SelectList(db.Languages, "Id", "Language",h.LanguagesId);
            if (ModelState.IsValid)
            {
                db.Hakkimizda.Add(h);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(h);
        }
    
        public ActionResult Delete(int id)
        {
            var hakkimizda = db.Hakkimizda.Find(id);
            db.Hakkimizda.Remove(hakkimizda);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}