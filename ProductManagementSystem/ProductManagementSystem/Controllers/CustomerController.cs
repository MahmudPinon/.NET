using Newtonsoft.Json;
using ProductManagementSystem.EF;
using ProductManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult BuyProduct()
        {
            var db = new ProductManagementEntities();
            var product=db.Products.ToList();
            var catagory = db.Catagories.ToList();
            int customerId = int.Parse(Request.Cookies["CustomerInfo"]["CustomerId"]);
            ViewBag.CustomerId = customerId;
            return View(product);
        }
        [HttpPost]
        /* public ActionResult BuyProduct(List<SelectedProduct> selectedProducts)
         {
             int customerId = 0;
             if (Request.Cookies["CustomerInfo"] != null)
             {
                 customerId = int.Parse(Request.Cookies["CustomerInfo"]["CustomerId"]);
             }
             var selectedProduct = new List<SelectedProduct>();
             if (selectedProducts != null && selectedProducts.Any())
             {
                 DateTime orderDate = DateTime.Now;

                 using (var db = new ProductManagementEntities1())
                 {
                     var order = new Order
                     {
                         CustomerId = customerId,
                         Date = orderDate,
                         Status = "Pending" // Set the status to "Pending"
                     };

                     db.Orders.Add(order);

                     // Save the order
                     db.SaveChanges();


                 }

                 var selectedProductsJson = JsonConvert.SerializeObject(selectedProducts);
                 var cookie = new HttpCookie("SelectedProducts", selectedProductsJson);
                 Response.Cookies.Add(cookie);
             }

             return RedirectToAction("Confirmation", "Customer");
         }*/
        public ActionResult BuyProduct(FormCollection form)
        {
           /* Dictionary<int, SelectedProduct> selectedProducts = new Dictionary<int, SelectedProduct>();
            int customerId = 0;
            if (Request.Cookies["CustomerInfo"] != null)
            {
                customerId = int.Parse(Request.Cookies["CustomerInfo"]["CustomerId"]);
            }
            foreach (var key in form.AllKeys)
            {
                if (key.StartsWith("selectedProducts[") && key.EndsWith("].IsSelected"))
                {
                    int productId = int.Parse(key.Split('[')[1].Split(']')[0]);
                    int quantity = int.Parse(form[key]);
                    bool isSelected = form[key] == "true";

                }
            }*/


            return RedirectToAction("Confirmation"); 
        }






        public ActionResult Confirmation()
        {
            return View();
        }

        public ActionResult SlectedProducts()
        {
            var selectedProductsCookie = Request.Cookies["SelectedProducts"];
            List<SelectedProduct> selectedProducts = new List<SelectedProduct>();

            if (selectedProductsCookie != null)
            {
                string selectedProductsJson = selectedProductsCookie.Value;
                selectedProducts = JsonConvert.DeserializeObject<List<SelectedProduct>>(selectedProductsJson);
            }

            // Now, you have the selected products from the cookie.

            return View(selectedProducts);
        }






    }
}