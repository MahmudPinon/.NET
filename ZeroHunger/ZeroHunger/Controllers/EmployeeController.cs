using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.DTOs;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CollectionRequests()
        {
            int? restaurantOwnerId = null;

            var cookie = HttpContext.Request.Cookies["UserInfo"];

            if (cookie != null && !string.IsNullOrEmpty(cookie["Id"]))
            {
                int parsedId;
                if (int.TryParse(cookie["Id"], out parsedId))
                {
                    restaurantOwnerId = parsedId;
                }
            }

            var db = new ZeroHungerEntities();

            var collectionRequests = db.CollectionRequests.Where(c => c.Status == "Not Collected" && c.employeeId == restaurantOwnerId).ToList();
            if(collectionRequests.Any())
            {
                var config1 = new MapperConfiguration(cfg => {
                    cfg.CreateMap<CollectionRequest, CollectionRequestDTO>();
                });
                var mapper1 = new Mapper(config1);
                var collectionRequestDTOs = mapper1.Map<List<CollectionRequestDTO>>(collectionRequests);

               
                return View(collectionRequestDTOs);
            }
            else
            {
                ViewBag.Message = "No Requests found.";
                return View();
            }
        }

        [HttpPost]
        public ActionResult CollectionRequests(CollectionRequestDTO collectionRequest)
        {
            if (ModelState.IsValid)
            {

                using (var db = new ZeroHungerEntities())
                {
                    var existingEntity = db.CollectionRequests.Find(collectionRequest.Id);

                    if (existingEntity != null)
                    {
                        existingEntity.CollectTime = collectionRequest.CollectTime;
                        existingEntity.Status = "Collected"; 
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("CollectionRequests"); 
            }

            return View(); 
        }






        [HttpGet]
        public ActionResult DistributionRequests()
        {

            int? id = null;

            var cookie = HttpContext.Request.Cookies["UserInfo"];

            if (cookie != null && !string.IsNullOrEmpty(cookie["Id"]))
            {
                int parsedId;
                if (int.TryParse(cookie["Id"], out parsedId))
                {
                    id = parsedId;
                }
            }

            var db = new ZeroHungerEntities();
            var collectedItems = db.Distributions.Where(d => d.DistributedTime == null && d.EmployeeId == id).ToList();
            var config1 = new MapperConfiguration(cfg => {
                cfg.CreateMap<Distribution, DistributionDTO>();
            });
            var mapper1 = new Mapper(config1);
            var distributedItemsDTOs = mapper1.Map<List<DistributionDTO>>(collectedItems);
            if (distributedItemsDTOs.Count == 0)
            {
                ViewBag.ErrorMessage = "No collected items found.";
            }

            return View(distributedItemsDTOs);
        }


        [HttpPost]
        public ActionResult DistributionRequests(DistributionDTO distribution)
        {


            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<DistributionDTO, Distribution>();
            });

            var mapper = new Mapper(config);
            var data = mapper.Map<Distribution>(distribution);


            using (var db = new ZeroHungerEntities())
            {
                var existingEntity = db.Distributions.Find(distribution.Id);

                if (existingEntity != null)
                {
                    existingEntity.DistributedTime = distribution.DistributedTime;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Distribution", "Home");

        }



        [HttpGet]
        public ActionResult SubmitCollectedFood()
        {
            int? id = null;

            var cookie = HttpContext.Request.Cookies["UserInfo"];

            if (cookie != null && !string.IsNullOrEmpty(cookie["Id"]))
            {
                int parsedId;
                if (int.TryParse(cookie["Id"], out parsedId))
                {
                    id = parsedId;
                }
            }

            var db = new ZeroHungerEntities();

            try
            {
                var collectionRequests = db.CollectionRequests
                    .Where(c => c.Status.Equals("Collected") && c.employeeId == id)
                    .ToList();
                var collectedItems = db.CollectedItems.ToList();
                if (collectionRequests.Any())
                {
                    var config1 = new MapperConfiguration(cfg => {
                        cfg.CreateMap<CollectionRequest, CollectionRequestDTO>();
                    });

                    var mapper1 = new Mapper(config1);
                    var collectionRequestDTOs = mapper1.Map<List<CollectionRequestDTO>>(collectionRequests);


                    var config2 = new MapperConfiguration(cfg => {
                        cfg.CreateMap<CollectedItem, CollectedItemDTO>();
                    });
                    var mapper2 = new Mapper(config2);
                    var collecteditemDTOs = mapper2.Map<List<CollectedItemDTO>>(collectedItems);

                    var viewModel = new CompositeDTO2
                    {
                        CollectionRequests = collectionRequestDTOs,
                        CollectedItems = collecteditemDTOs
                    };

                    ViewBag.Id = id;
                    return View(viewModel);
                }
                else
                {
                    ViewBag.Message = "No collection found.";
                    ViewBag.Id = id;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error occurred while processing the request.";
                ViewBag.ErrorDetails = ex.ToString();
                return View();
            }
        }



        [HttpPost]
        public ActionResult SubmitCollectedFood(CollectedItemDTO collecteditems)
        {

                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<CollectedItemDTO, CollectedItem>();
                });

                var mapper = new Mapper(config);
                var data = mapper.Map<CollectedItem>(collecteditems);

                using (var db = new ZeroHungerEntities())
                {
                    try
                    {
                        db.CollectedItems.Add(data);
                        db.SaveChanges();

                        var foodName = data.FoodName;
                        var employeeId = data.EmployeeId;
                        var savedItemId = data.Id;
                        var today = DateTime.Now;
                       var distributionData = new Distribution
                       {
                        FoodName = foodName,
                        CollectionId = savedItemId, 
                        Date = today,
                        Quantity = data.Quantity,      
                       };
                    db.Distributions.Add(distributionData);
                    db.SaveChanges();
                    return View();
                }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Error", "Home");
                    }
                }

            
        }





    }
}