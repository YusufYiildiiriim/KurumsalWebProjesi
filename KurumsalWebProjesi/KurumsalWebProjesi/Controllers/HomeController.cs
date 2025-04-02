using KurumsalWebProjesi.Models.DataContext;
using KurumsalWebProjesi.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KurumsalWebProjesi.Controllers
{
    public class HomeController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();
        // GET: Home
        public ActionResult Index()
        {

            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(x=>x.HizmetId);
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


    }
}