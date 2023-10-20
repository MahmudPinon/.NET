using ProdctandCatagory.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProdctandCatagory.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult ProductView()
        {
            var db = new ProductandCatagoryEntities1();
            var data=db.Products.ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult AddProduct()
        {
            var db = new ProductandCatagoryEntities1();
            ViewBag.Catagories = new SelectList(db.Catagories, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(Product product) 
        {
            var db = new ProductandCatagoryEntities1();
            bool productExists = db.Products.Any(p => p.Name == product.Name);
            if (productExists)
            {
                ModelState.AddModelError("Name", "A product with the same name already exists.");
                return View(product);
            }
            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("ProductView");
        }

        public ActionResult Details(int id)
        {
            var db=new ProductandCatagoryEntities1();
            var res = db.Products.Find(id);
            if (res != null)
            {
                // Find the associated category by Id
                var category = db.Catagories.FirstOrDefault(c => c.Id == res.Cid);

                if (category != null)
                {
                    ViewBag.CategoryName = category.Name;
                }
                else
                {
                    ViewBag.CategoryName = "Category Not Found";
                }
            }

            return View(res);

        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new ProductandCatagoryEntities1();
            var res = db.Products.Find(id);
            ViewBag.Catagories = new SelectList(db.Catagories, "Id", "Name");
            return View(res);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            var db = new ProductandCatagoryEntities1();

            bool productExists = db.Products.Any(p => p.Name == product.Name);
            if (productExists)
            {
                ModelState.AddModelError("Name", "A product with the same name already exists.");
                return View(product);
            }
            var exdata = db.Products.Find(product.Id);
            exdata.Name = product.Name;
            exdata.Cid = product.Cid;
            db.SaveChanges();

            return RedirectToAction("ProductView");
        }


        public ActionResult Delete(int id)
        {
            var db = new ProductandCatagoryEntities1();
            var exdata = db.Products.Find(id); 
            db.Products.Remove(exdata);
            db.SaveChanges();
            return RedirectToAction("ProductView");
        }




    }
}