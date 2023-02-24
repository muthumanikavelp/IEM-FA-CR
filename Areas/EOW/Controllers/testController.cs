using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class testController : Controller
    {
        //
        // GET: /EOW/test/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /EOW/test/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /EOW/test/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /EOW/test/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /EOW/test/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /EOW/test/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /EOW/test/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /EOW/test/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
