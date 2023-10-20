using ProdctandCatagory.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProdctandCatagory.Controllers
{
    public class CatagoryController : Controller
    {
        // GET: Catagory
        public ActionResult CatagoryView()
        {
            var db = new ProductandCatagoryEntities1();
            var data=db.Catagories.ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult CreateCatagory() 
        {
            var db = new ProductandCatagoryEntities1();
            return View();
        }
        [HttpPost]
        public ActionResult CreateCatagory(Catagory catagory)
        {
            var db = new ProductandCatagoryEntities1();
            bool catagoryExists = db.Catagories.Any(p => p.Name == catagory.Name);
            if (catagoryExists)
            {
                ModelState.AddModelError("Name", "A Catagory with the same name already exists.");
                return View(catagory);
            }
            db.Catagories.Add(catagory);
            db.SaveChanges();
            return RedirectToAction("CatagoryView");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new ProductandCatagoryEntities1();
            var res = db.Products.Find(id);
            return View(res);
        }
        [HttpPost]
        public ActionResult Edit(Catagory catagory)
        {
            var db = new ProductandCatagoryEntities1();

            bool catagoryExists = db.Catagories.Any(p => p.Name == catagory.Name);
            if (catagoryExists)
            {
                ModelState.AddModelError("Name", "A Catagory with the same name already exists.");
                return View(catagory);
            }
            var exdata = db.Catagories.Find(catagory.Id);
            exdata.Name = catagory.Name;
            db.SaveChanges();

            return RedirectToAction("CatagoryView");
        }
        public ActionResult Delete(int id)
        {
            var db = new ProductandCatagoryEntities1();
            var exdata = db.Catagories.Find(id);
            var productsToDelete = db.Products.Where(p => p.Cid == id);
            db.Products.RemoveRange(productsToDelete);

            if (exdata != null)
            {
                db.Catagories.Remove(exdata);
                db.SaveChanges();
            }

            return RedirectToAction("CatagoryView");
        }

    }
}