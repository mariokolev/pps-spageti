using furniture.Models;
using FurnitureShop;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc; 

namespace furniture.Controllers
{
    public class ItemController : Controller
    {
        private ItemRepository itemRepository;
        // GET: Item
        public ActionResult Index()
        {
            itemRepository = new ItemRepository();
            List<Item> items = itemRepository.FindAll();
            
            ViewData["items"] = items;
            ViewBag.Title = "Артикули";

            return View(items);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            itemRepository = new ItemRepository();
            List<Item> items = itemRepository.FindAll();

            ViewData["items"] = items;
            ViewData["client_id"] = collection["client_id"];
            ViewBag.Title = "Изберете Артикули";

            return View(items);
        }

        // GET: Item/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Добавяне на артикул";

            return View();
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            this.itemRepository = new ItemRepository();

            try
            {
                Item item = new Item();
                item.ItemName = collection["ItemName"];
                item.Price = Double.Parse(collection["Price"]);
                item.Quantity = Int32.Parse(collection["Quantity"]);
                item.Description = collection["Description"];
                item.DateOfManufacture = DateTime.Parse(collection["DateOfManufacture"]);

                this.itemRepository.Save(item);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Item/Edit/5
        public ActionResult Edit(int id)
        {
            this.itemRepository = new ItemRepository();
            ViewBag.Title = "Редактиране на данни за артикул";

            return View(itemRepository.FindById(id));
        }

        // POST: Item/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            this.itemRepository = new ItemRepository();

            try
            {
                Item item = itemRepository.FindById(id);
                item.ItemName = collection["ItemName"];
                item.Price = Double.Parse(collection["Price"]);
                item.Quantity = Int32.Parse(collection["Quantity"]);
                item.Description = collection["Description"];
                item.DateOfManufacture = DateTime.Parse(collection["DateOfManufacture"]);
               
                itemRepository.Update(item);
               
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Item/Delete/5
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
