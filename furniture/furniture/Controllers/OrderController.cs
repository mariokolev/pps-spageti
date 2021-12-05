using furniture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace furniture.Controllers
{
    public class OrderController : Controller
    {
        private OrderRepository orderRepository;

        // GET: Order
        public ActionResult Index()
        {
            orderRepository = new OrderRepository();
            List<Order> orders = orderRepository.FindAll();

            ViewData["orders"] = orders;
            ViewBag.Title = "Продажби";

            return View(orders);
        }

        // GET: Order/Details/5
        public ActionResult Details(int id)
        {
            orderRepository = new OrderRepository();
            Order order = orderRepository.FindOrderDetailsById(id);
            ViewData["order"] = order;
            ViewBag.Title = "Детайли за продажба";


            return View(order.OrderDetails);
        }

        // POST: Order/Create
        [HttpPost]
        //public ActionResult Create(FormCollection collection)
        public ActionResult Create(int client_id, List<string> item_id, List<string> quantity)

        {
            orderRepository = new OrderRepository();
            try
            {
                quantity.RemoveAll(str => String.IsNullOrEmpty(str));

                Dictionary<string, string> itemAndQuantity = new Dictionary<string, string>();

                for (int index = 0; index < item_id.Count; index++)
                {
                    itemAndQuantity.Add(item_id[index], quantity[index]);
                }

                Order order = orderRepository.Save(client_id, itemAndQuantity);

                //return RedirectToAction("Index");
                ViewData["order"] = order;
                return View("Receipt", order);
            }
            catch
            {
                return View("Index");
            }
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Order/Edit/5
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

        // GET: Order/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Order/Delete/5
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
