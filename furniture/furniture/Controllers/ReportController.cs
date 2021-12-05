using furniture.Models;
using FurnitureShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace furniture.Controllers
{
    public class ReportController : Controller
    {
        ItemRepository itemRepository;
        ClientRepository clientRepository;
        OrderRepository orderRepository;

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GenerateReport(FormCollection collection)
        {

            int report = Int32.Parse(collection["report_id"]);

            switch (report)
            {
                case 1:

                    itemRepository = new ItemRepository();
                    MostOrderedItemDTO mostOrderedItem = itemRepository.FindMostOrdered();

                    ViewData["item"] = mostOrderedItem;
                    ViewBag.Title = "Най-продаван артикул";

                    return View("MostOrderedItem", mostOrderedItem);

                case 2:

                    clientRepository = new ClientRepository();
                    ClientMostOrdersDTO client = clientRepository.FindClientWithMostOrders();

                    ViewBag.Title = "Клиент с най-много поръчки";
                    return View("ClientMostOrders", client);

                case 3:

                itemRepository = new ItemRepository();

                MostOrderedItemDTO item = itemRepository.FindMostOrderedByDeliveryAddress(collection["city"]);

                ViewData["item"] = item;
                ViewBag.Title = "Най-продаван артикул в " + collection["city"];

                return View("MostOrderedItem", item);

                case 4:

                    orderRepository = new OrderRepository();
                    List<OrderDetails> ordersBeforeDate = orderRepository.FindOrderDetailsBeforeDate(collection["item_name"], DateTime.Parse(collection["date"]));

                    ViewBag.Title = "Продажби за " + collection["item_name"] + " преди дата: " + collection["date"];

                    return View("OrderDetailsBeforeDataByItemName", ordersBeforeDate);

                case 5:

                    orderRepository = new OrderRepository();
                    List<OrderDetails> orders = orderRepository.FindOrderDetailsByPhoneNumber(collection["phone_number"]);

                    ViewBag.Title = "Продажби за клиент с телефонен номер: "  + collection["phone_number"];

                    return View("OrderedItemsByClient", orders);
            }

            return View();
        }

        // GET: Report/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Report/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Report/Create
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

        // GET: Report/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Report/Edit/5
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

        // GET: Report/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Report/Delete/5
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
